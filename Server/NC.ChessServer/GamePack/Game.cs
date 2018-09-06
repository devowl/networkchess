using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.Exceptions;
using NC.Shared.GameField;

namespace NC.ChessServer.GamePack
{
    /// <summary>
    /// Players game controller.
    /// </summary>
    public class Game : IDisposable
    {
        private readonly IPieceMasterFactory _masterFactory;

        private bool _isInitialized;

        private VirtualField _virtualField;

        private PlayerColor _turnColor;

        /// <summary>
        /// Constructor for <see cref="Game"/>.
        /// </summary>
        public Game(IPieceMasterFactory masterFactory, Player player1, Player player2)
        {
            _masterFactory = masterFactory;
            Player1 = player1;
            Player2 = player2;
        }

        /// <summary>
        /// <see cref="Game"/> factory.
        /// </summary>
        /// <param name="player1">Player 1.</param>
        /// <param name="player2">Player 2.</param>
        /// <returns><see cref="Game"/> instance.</returns>
        public delegate Game Factory(Player player1, Player player2);

        /// <summary>
        /// Player 1.
        /// </summary>
        public Player Player1 { get; }

        /// <summary>
        /// Player 2.
        /// </summary> 
        public Player Player2 { get; }

        /// <summary>
        /// Prepare game.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            var randomColor = DateTime.Now.Millisecond % 2 == 0 ? PlayerColor.White : PlayerColor.Black;
            var player1Color = randomColor;
            var player2Color = player1Color.Invert();

            Player.SetColor(Player1, player1Color);
            Player.SetColor(Player2, player2Color);

            var chessGameField = VirtualFieldUtils.CreateDefaultField();

            _turnColor = PlayerColor.White;
            _virtualField = new VirtualField(chessGameField);

            var p1GameInfo = new WcfGameInfo(
                player1Color,
                Player2.PlayerName,
                chessGameField.ToJaggedArray(),
                _turnColor);

            var p2GameInfo = new WcfGameInfo(
                player2Color,
                Player1.PlayerName,
                chessGameField.ToJaggedArray(),
                _turnColor);

            Player1.Callback.GameHasStarted(p1GameInfo);
            Player2.Callback.GameHasStarted(p2GameInfo);

            _isInitialized = true;
        }

        /// <inheritdoc/>
        public void Move(string sessionId, ChessPoint from, ChessPoint to)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Game isn't initialized");
            }

            // Who makes a movement
            var iniciator = GetPlayer(sessionId);

            // His opponent 
            var opponent = Player1 == iniciator ? Player2 : Player1;
            var piece = _virtualField[from.X, from.Y];

            // Check for hacker movements
            CheaterCheck(iniciator, from, to);

            ThreadPool.QueueUserWorkItem(
                obj =>
                {
                    _virtualField[to.X, to.Y] = piece;
                    _virtualField[from.X, from.Y] = ChessPiece.Empty;

                    if (IsCheckMate(iniciator, opponent))
                    {
                        NotifyAboutCheckMate(iniciator, opponent);
                    }
                    else
                    {
                        NotifyFieldChanged(iniciator, opponent, from, to);
                    }
                });
        }

        private bool IsCheckMate(Player iniciator, Player opponent)
        {
            var currentField = _virtualField;

            // Check for check now
            if (IsCheck(iniciator.PlayerColor, opponent.PlayerColor, currentField))
            {
                // Otherwise do all possible movements, in the case of no possiblity to prevent being attacked = iniciator wins
                foreach (var opponmentPiecePoint in FindPieces(p => p.GetPlayerColor() == opponent.PlayerColor))
                {
                    PieceMasterBase master;
                    if (_masterFactory.TryGetMaster(_virtualField, opponmentPiecePoint, out master))
                    {
                        foreach (var movement in master.GetMovements())
                        {
                            var fieldCopy = new VirtualField(_virtualField.CloneMatrix());
                            var temp = fieldCopy[opponmentPiecePoint];
                            fieldCopy[opponmentPiecePoint] = fieldCopy[movement];
                            fieldCopy[movement] = temp;
                            if (IsCheck(iniciator.PlayerColor, opponent.PlayerColor, fieldCopy))
                            {
                                return true;
                            }
                        }   
                    }
                }
            }

            return false;
        }

        private bool IsCheck(PlayerColor iniciatorColor, PlayerColor opponentColor, VirtualField field)
        {
            var kingMapping = new Dictionary<PlayerColor, ChessPiece>()
            {
                { PlayerColor.Black, ChessPiece.BlackKing },
                { PlayerColor.White, ChessPiece.WhiteKing },
            };
            
            ChessPoint opponentKingPoint = FindPieces(p => p == kingMapping[opponentColor]).FirstOrDefault();
            
            if (opponentKingPoint == null)
            {
                return false;
            }
            
            var iniciatorAttacked = UnderAttackPoints(iniciatorColor, field);
            return iniciatorAttacked.Any(point => point == opponentKingPoint);
        }

        private IEnumerable<ChessPoint> FindPieces(Func<ChessPiece, bool> checker)
        {
            for (int x = 0; x < _virtualField.Width; x++)
            {
                for (int y = 0; y < _virtualField.Height; y++)
                {
                    if (checker(_virtualField[x, y]))
                    {
                        yield return new ChessPoint(x, y);
                    }
                }
            }
        }

        private IEnumerable<ChessPoint> UnderAttackPoints(PlayerColor iniciatorColor, VirtualField field)
        {
            foreach (var point in FindPieces(piece => piece.GetPlayerColor() == iniciatorColor))
            {
                PieceMasterBase master;
                if (_masterFactory.TryGetMaster(field, point, out master))
                {
                    foreach (var movement in master.GetMovements())
                    {
                        yield return movement;
                    }       
                }
            }
        }
        
        private void NotifyAboutCheckMate(Player iniciator, Player opponent)
        {
            var field = _virtualField.CloneMatrix();
            _turnColor = _turnColor.Invert();
            var winner = iniciator.PlayerColor;

            var winnerGameInfo = new WcfGameInfo(
                iniciator.PlayerColor,
                opponent.PlayerName,
                field.ToJaggedArray(),
                _turnColor,
                winner);

            var loserGameInfo = new WcfGameInfo(
                iniciator.PlayerColor,
                opponent.PlayerName,
                field.ToJaggedArray(),
                _turnColor,
                winner);

            iniciator.Callback.GameHasEnded(winnerGameInfo);
            opponent.Callback.GameHasEnded(loserGameInfo);
        }
        
        /// <inheritdoc/>
        public void Dispose()
        {
        }

        private void CheaterCheck(Player iniciator, ChessPoint from, ChessPoint to)
        {
            var color = iniciator.PlayerColor;
            var piece = _virtualField[from.X, from.Y];
            var pieceColor = piece.GetPlayerColor();

            if (!pieceColor.HasValue)
            {
                throw new InvalidMovementException(from.X, from.Y, "You're trying move empty field");
            }

            if (color != pieceColor.Value)
            {
                throw new CheaterException("You're trying to move your opponent piece");
            }

            if (color != _turnColor)
            {
                throw new CheaterException("You're trying to move your piece in opponent turn");
            }
            
            PieceMasterBase master;

            if (!_masterFactory.TryGetMaster(_virtualField, from, out master))
            {
                throw new InvalidMovementException(from.X, from.Y, "You're trying move unknown piece without master");
            }

            if (master.GetMovements().All(step => !Equals(step, to)))
            {
                throw new CheaterException("You're trying to move your piece to incorrect position");
            }
        }

        private void NotifyFieldChanged(Player iniciator, Player opponent, ChessPoint from, ChessPoint to)
        {
            var field = _virtualField.CloneMatrix().ToJaggedArray();

            var wcfFrom = from.FromBusiness();
            var wcfTo = to.FromBusiness();
            _turnColor = _turnColor.Invert();

            iniciator.Callback.GameFieldUpdated(field, _turnColor, wcfFrom, wcfTo, iniciator.PlayerColor);
            opponent.Callback.GameFieldUpdated(field, _turnColor, wcfFrom, wcfTo, opponent.PlayerColor);
        }

        private Player GetPlayer(string sessionId)
        {
            if (Player1.SessionId == sessionId)
            {
                return Player1;
            }

            if (Player2.SessionId == sessionId)
            {
                return Player2;
            }

            throw new InvalidOperationException("Player not founded");
        }
    }
}