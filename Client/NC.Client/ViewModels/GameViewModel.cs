using System;

using Microsoft.Practices.Prism.Regions;

using NC.ChessControls.Data;
using NC.ChessControls.Prism;
using NC.Client.Constants;
using NC.Client.Interfaces;
using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// Virtual chess field view model.
    /// </summary>
    public class GameViewModel : NotificationObject, INavigationAware, IDisposable
    {
        private readonly IGameServiceProvider _gameServiceProvider;

        private readonly IEndpointInfo _endpointInfo;

        private VirtualField _gameField;

        private GameController _controller;

        /// <summary>
        /// Constructor for <see cref="GameViewModel"/>.
        /// </summary>
        public GameViewModel(IGameServiceProvider gameServiceProvider, IEndpointInfo endpointInfo)
        {
            _gameServiceProvider = gameServiceProvider;
            _endpointInfo = endpointInfo;
            var chessDefaultField = VirtualFieldUtils.CreateDefaultField();
            _gameField = new VirtualField(chessDefaultField);
            _controller = new GameController();
            _controller.Movement += OnChessPieceMovement;
        }

        /// <summary>
        /// Chess game field.
        /// </summary>
        public VirtualField GameField
        {
            get
            {
                return _gameField;
            }

            set
            {
                _gameField = value;
                RaisePropertyChanged(() => GameField);
            }
        }

        /// <summary>
        /// Game controller.
        /// </summary>
        public GameController Controller
        {
            get
            {
                return _controller;
            }

            private set
            {
                _controller = value;
            }
        }

        /// <inheritdoc/>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var callback = _gameServiceProvider.ServiceCallback;
            var field = callback.GameInfo.DefaultField;
            var playerColor = callback.GameInfo.PlayerColor;
            GameField = new VirtualField(field.ToMultiDimensionalArray(), playerColor);
            callback.FieldUpdated += OnFieldUpdated;
        }

        private void OnFieldUpdated(object sender, FieldInfoArgs fieldInfoArgs)
        {
            var field = fieldInfoArgs.VirtualField.ToMultiDimensionalArray();
            GameField = new VirtualField(field);
        }

        /// <inheritdoc/>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc/>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OnChessPieceMovement(object sender, MovementArgs args)
        {
            var service = _gameServiceProvider.ChessClient.Service;
            var from = args.From;
            var to = args.To;

            service.Move(_endpointInfo.SessionId, from.FromBusiness(), to.FromBusiness());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _controller.Movement -= OnChessPieceMovement;
        }
    }
}