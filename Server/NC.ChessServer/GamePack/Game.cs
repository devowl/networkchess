using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
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

        private PlayerColor? _checkedPlayer;

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
        /// Game ended event.
        /// </summary>
        public event EventHandler<EventArgs> GameEnded;

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

            // TODO Если теряется связь то надо отдавать победу
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
            var initiator = GetPlayer(sessionId);

            // His opponent 
            var opponent = Player1 == initiator ? Player2 : Player1;
            var piece = _virtualField[from.X, from.Y];

            // Check for hacker movements
            CheaterCheck(initiator, from, to);

            ThreadPool.QueueUserWorkItem(
                obj =>
                {
                    _virtualField[to.X, to.Y] = piece;
                    _virtualField[from.X, from.Y] = ChessPiece.Empty;

                    bool isCheck;
                    var isCheckMate = CheckMateLogic.IsCheckMate(
                        initiator.PlayerColor,
                        _virtualField,
                        _masterFactory,
                        out isCheck);
                    
                    _checkedPlayer = isCheck ? opponent.PlayerColor : (PlayerColor?)null;

                    if (isCheckMate)
                    {
                        NotifyAboutCheckMate(initiator, opponent, from, to);
                    }
                    else
                    {
                        NotifyFieldChanged(initiator, opponent, from, to);
                    }
                });
        }

        private void NotifyAboutCheckMate(Player initiator, Player opponent, ChessPoint from, ChessPoint to)
        {
            var field = _virtualField.CloneMatrix();
            _turnColor = _turnColor.Invert();
            var winner = initiator.PlayerColor;

            var winnerGameInfo = new WcfGameInfo(
                initiator.PlayerColor,
                opponent.PlayerName,
                field.ToJaggedArray(),
                _turnColor,
                winner);

            var loserGameInfo = new WcfGameInfo(
                initiator.PlayerColor,
                opponent.PlayerName,
                field.ToJaggedArray(),
                _turnColor,
                winner);

            initiator.Callback.GameHasEnded(winnerGameInfo, from.FromBusiness(), to.FromBusiness());
            opponent.Callback.GameHasEnded(loserGameInfo, from.FromBusiness(), to.FromBusiness());

            GameEnded?.Invoke(this, EventArgs.Empty);
        }
        
        /// <inheritdoc/>
        public void Dispose()
        {
        }

        private void CheaterCheck(Player initiator, ChessPoint from, ChessPoint to)
        {
            var color = initiator.PlayerColor;
            var piece = _virtualField[from.X, from.Y];
            var pieceColor = piece.GetPlayerColor();

            if (!pieceColor.HasValue)
            {
                throw new FaultException<InvalidMovementException>(
                    new InvalidMovementException(from.X, from.Y, "You're trying move empty field"));
            }

            if (color != pieceColor.Value)
            {
                throw new FaultException<CheaterException>(
                    new CheaterException("You're trying to move your opponent piece"));
            }

            if (color != _turnColor)
            {
                throw new FaultException<CheaterException>(
                    new CheaterException("You're trying to move your piece in opponent turn"));
            }
            
            PieceMasterBase master;

            if (!_masterFactory.TryGetMaster(_virtualField, from, out master))
            {
                throw new FaultException<InvalidMovementException>(
                    new InvalidMovementException(from.X, from.Y, "You're trying move unknown piece without master"));
            }

            if (master.GetMovements().All(step => !Equals(step, to)))
            {
                throw new FaultException<CheaterException>(
                    new CheaterException("You're trying to move your piece to incorrect position"));
            }

            var fieldCopy = new VirtualField(_virtualField.CloneMatrix())
            {
                [to.X, to.Y] = piece,
                [from.X, from.Y] = ChessPiece.Empty
            };

            if (CheckMateLogic.IsCheck(initiator.PlayerColor, fieldCopy, _masterFactory) && _checkedPlayer.HasValue &&
                _checkedPlayer.Value == initiator.PlayerColor)
            {
                throw new FaultException<InvalidMovementException>(
                    new InvalidMovementException(from.X, from.Y, "Wrong step, you still have a check"));
            }
        }

        private void NotifyFieldChanged(Player initiator, Player opponent, ChessPoint from, ChessPoint to)
        {
            var field = _virtualField.CloneMatrix().ToJaggedArray();

            var wcfFrom = from.FromBusiness();
            var wcfTo = to.FromBusiness();
            _turnColor = _turnColor.Invert();

            initiator.Callback.GameFieldUpdated(field, _turnColor, wcfFrom, wcfTo, initiator.PlayerColor, _checkedPlayer);
            opponent.Callback.GameFieldUpdated(field, _turnColor, wcfFrom, wcfTo, opponent.PlayerColor, _checkedPlayer);
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