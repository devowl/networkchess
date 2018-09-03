using System;
using System.Linq;

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
        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            var colors = new[]
            {
                PlayerColor.White,
                PlayerColor.Black
            };

            var randomIndex = DateTime.Now.Millisecond % 2 == 0 ? 1 : 0;
            var player1Color = colors[randomIndex];
            var player2Color = colors.Single(c => c != player1Color);

            Player.SetColor(Player1, player1Color);
            Player.SetColor(Player2, player2Color);

            var chessGameField = VirtualFieldUtils.CreateDefaultField();

            _virtualField = new VirtualField(chessGameField);

            var p1GameInfo = new WcfGameInfo(player1Color, Player2.PlayerName, chessGameField);
            var p2GameInfo = new WcfGameInfo(player2Color, Player1.PlayerName, chessGameField);

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
            // var sideColor = VirtualFieldUtils.GetSideName(piece);

            // Check for hacker movements
            CheaterCheck(iniciator, from, to);

            /***************************/
            _virtualField[to.X, to.Y] = piece;
            _virtualField[from.X, from.Y] = ChessPiece.Empty;
            /**************************/

            NotifyFieldChanged(iniciator, opponent, from, to);
        }

        private void CheaterCheck(Player iniciator, ChessPoint from, ChessPoint to)
        {
            var color = iniciator.PlayerColor;
            var piece = _virtualField[from.X, from.Y];
            var pieceColor = VirtualFieldUtils.GetSideName(piece);

            if (!pieceColor.HasValue)
            {
                throw new InvalidMovementException(from.X, from.Y, "You trying move empty field");
            }

            if (color != pieceColor.Value)
            {
                throw new CheaterException("You trying to move your opponent piece");
            }

            var factory = new PieceMasterFactory(_virtualField);
            PieceMasterBase master;

            if (!factory.TryGetMaster(from.X, from.Y, out master))
            {
                throw new InvalidMovementException(from.X, from.Y, "You trying move unknown piece without master");
            }

            //if(master.GetMovements())
        }

        private void NotifyFieldChanged(Player iniciator, Player opponent, ChessPoint from, ChessPoint to)
        {
            var field = _virtualField.CloneMatrix().ToJaggedArray();

            iniciator.Callback.GameFieldUpdated(field, opponent.PlayerColor, from.X, from.Y, to.X, to.Y);
            opponent.Callback.GameFieldUpdated(field, opponent.PlayerColor, from.X, from.Y, to.X, to.Y);
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

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}