using System;
using System.ServiceModel;

using NC.ChessServer.GamePack;
using NC.ChessServer.Interfaces;
using NC.Shared.Contracts;

namespace NC.ChessServer.Services
{
    /// <summary>
    /// User service.
    /// </summary>
    public class UserService :  IUserService
    {
        private readonly IPlayerManager _playerManager;

        private readonly Player.Factory _playerFactory;

        /// <summary>
        /// Constructor for <see cref="UserService"/>.
        /// </summary>
        public UserService(IPlayerManager playerManager, Player.Factory playerFactory)
        {
            _playerManager = playerManager;
            _playerFactory = playerFactory;
        }

        /// <inheritdoc/>
        public bool Login(string login, out string sessionId)
        {
            sessionId = Guid.NewGuid().ToString();
            var player = _playerFactory(sessionId, login);
            _playerManager.AddToQueue(player);
            return true;
        }

        /// <inheritdoc/>
        public void Logout(string sessionId)
        {
            throw new System.NotImplementedException();
        }
    }
}