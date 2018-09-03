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

        private readonly IUserMessage _userMessage;

        private VirtualField _gameField;

        private GameController _controller;

        private PlayerColor _yourColor;

        private PlayerColor _turnColor;

        /// <summary>
        /// Constructor for <see cref="GameViewModel"/>.
        /// </summary>
        public GameViewModel(
            IGameServiceProvider gameServiceProvider,
            IEndpointInfo endpointInfo,
            IUserMessage userMessage)
        {
            _gameServiceProvider = gameServiceProvider;
            _endpointInfo = endpointInfo;
            _userMessage = userMessage;
            var chessDefaultField = VirtualFieldUtils.CreateDefaultField();
            _gameField = new VirtualField(chessDefaultField);
            _controller = new GameController();
            _controller.Movement += OnChessPieceMovement;
        }

        /// <summary>
        /// Your pieces color.
        /// </summary>
        public PlayerColor YourColor
        {
            get
            {
                return _yourColor;
            }

            set
            {
                _yourColor = value;
                RaisePropertyChanged(() => YourColor);
            }
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

        /// <summary>
        /// Turn color.
        /// </summary>
        public PlayerColor TurnColor
        {
            get
            {
                return _turnColor;
            }

            set
            {
                _turnColor = value;
                RaisePropertyChanged(() => TurnColor);
            }
        }

        /// <inheritdoc/>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var callback = _gameServiceProvider.ServiceCallback;
            var gameInfo = callback.GameInfo;
            var field = callback.GameInfo.DefaultField;
            var playerColor = callback.GameInfo.PlayerColor;

            TurnColor = gameInfo.TurnColor;
            YourColor = gameInfo.PlayerColor;
            GameField = new VirtualField(field.ToMultiDimensionalArray(), playerColor);
            callback.FieldUpdated += OnFieldUpdated;
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

        /// <inheritdoc/>
        public void Dispose()
        {
            _controller.Movement -= OnChessPieceMovement;
        }

        private void OnFieldUpdated(object sender, FieldInfoArgs args)
        {
            var field = args.VirtualField.ToMultiDimensionalArray();
            GameField = new VirtualField(field, args.PlayerColor);
            TurnColor = args.TurnColor;
        }

        private void OnChessPieceMovement(object sender, MovementArgs args)
        {
            var service = _gameServiceProvider.ChessClient.Service;
            var from = args.From;
            var to = args.To;

            service.Move(_endpointInfo.SessionId, from.FromBusiness(), to.FromBusiness());
        }
    }
}