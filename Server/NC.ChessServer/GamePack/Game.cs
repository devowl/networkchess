using System;
using System.Linq;

using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.ChessServer.GamePack
{
    /// <summary>
    /// Players game controller.
    /// </summary>
    public class Game : IDisposable
    {
        private bool _isInitialized;

        private string _player1Color;

        private string _player2Color;

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
                Constants.WhiteName,
                Constants.BlackName
            };

            var randomIndex = DateTime.Now.Millisecond % 2 == 0 ? 1 : 0;
            _player1Color = colors[randomIndex];
            _player2Color = colors.Single();

            var chessGameField = VirtualFieldUtils.CreateDefaultField();

            _virtualField = new VirtualField(chessGameField);

            var p1GameInfo = new WcfGameInfo(_player1Color, Player2.PlayerName, chessGameField);
            var p2GameInfo = new WcfGameInfo(_player2Color, Player1.PlayerName, chessGameField);

            Player1.Callback.GameHasStarted(p1GameInfo);
            Player2.Callback.GameHasStarted(p2GameInfo);

            _isInitialized = true;
        }

        public void Move(string sessionId, int x1, int y1, int x2, int y2)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Game isn't initialized");
            }

            // Who makes a movement
            var iniciator = GetPlayer(sessionId);

            // His opponent 
            var opponent = Player1 == iniciator ? Player2 : Player1;

            var piece = _virtualField[x1, x2];
            var sideName = VirtualFieldUtils.GetSideName(piece);
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