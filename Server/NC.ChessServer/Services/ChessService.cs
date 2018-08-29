using System.ServiceModel;

using NC.ChessServer.Interfaces;
using NC.Shared.Contracts;
using NC.Shared.Exceptions;

namespace NC.ChessServer.Services
{
    /// <summary>
    /// Chess service.
    /// </summary>
    [ServiceBehavior]
    public class ChessService : IChessService
    {
        private readonly IPlayerManager _playerManager;

        private readonly IGameManager _gameManager;

        /// <summary>
        /// Constructor for <see cref="ChessService"/>.
        /// </summary>
        public ChessService(IPlayerManager playerManager, IGameManager gameManager)
        {
            _playerManager = playerManager;
            _gameManager = gameManager;
        }

        /// <inheritdoc/>
        public void Ready(string sessionId)
        {
            CheckSession(sessionId);
            _playerManager.Ready(sessionId);
        }

        /// <inheritdoc/>
        public void Move(string sessionId, int x1, int y1, int x2, int y2)
        {
            CheckSession(sessionId);
            _gameManager.Move(sessionId, x1, y1, x2, y2);
        }

        private void CheckSession(string sessionId)
        {
            if (!_playerManager.HasSession(sessionId))
            {
                throw new SessionNotFoundedException();
            }
        }
    }
}