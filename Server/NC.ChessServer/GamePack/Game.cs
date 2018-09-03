using System;
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
        private bool _isInitialized;

        private VirtualField _virtualField;

        private PlayerColor _turnColor;

        /// <summary>
        /// Constructor for <see cref="Game"/>.
        /// </summary>
        public Game(Player player1, Player player2)
        {
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
            var player2Color = Invert(player1Color);

            Player.SetColor(Player1, player1Color);
            Player.SetColor(Player2, player2Color);

            var chessGameField = VirtualFieldUtils.CreateDefaultField();

            _turnColor = PlayerColor.White;
            _virtualField = new VirtualField(chessGameField);

            var p1GameInfo = new WcfGameInfo(player1Color, Player2.PlayerName, chessGameField, _turnColor);
            var p2GameInfo = new WcfGameInfo(player2Color, Player1.PlayerName, chessGameField, _turnColor);

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
                    NotifyFieldChanged(iniciator, opponent, from, to);
                });
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        private static PlayerColor Invert(PlayerColor color)
        {
            return color == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
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

            var factory = new PieceMasterFactory(_virtualField);
            PieceMasterBase master;

            if (!factory.TryGetMaster(from.X, from.Y, out master))
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
            _turnColor = Invert(_turnColor);

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