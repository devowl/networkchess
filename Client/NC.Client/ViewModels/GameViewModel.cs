using System;
using System.Linq;

using Microsoft.Practices.Prism.Regions;

using NC.ChessControls.Data;
using NC.ChessControls.Prism;
using NC.Client.Constants;
using NC.Client.Interfaces;
using NC.Client.Shell;
using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.GameField;

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

        private string _opponentName;

        private string _gameLog;

        private IPieceMasterFactory _masterFactory;

        private readonly LocalNavigator _navigator;

        private bool _isGameEnded;

        private string _gameResultText;

        private string _flowParameter = null;

        /// <summary>
        /// Constructor for <see cref="GameViewModel"/>.
        /// </summary>
        public GameViewModel(
            IGameServiceProvider gameServiceProvider,
            IEndpointInfo endpointInfo,
            IUserMessage userMessage,
            IPieceMasterFactory masterFactory,
            LocalNavigator navigator)
        {
            _masterFactory = masterFactory;
            _navigator = navigator;
            _gameServiceProvider = gameServiceProvider;
            _endpointInfo = endpointInfo;
            _userMessage = userMessage;
            var chessDefaultField = VirtualFieldUtils.CreateDefaultField();
            _gameField = new VirtualField(chessDefaultField);
            _controller = new GameController();
            _controller.Movement += OnChessPieceMovement;
            PlayMoreCommand = new DelegateCommand(PlayMoreHandler);
            DisconnectCommand = new DelegateCommand(DisconnectHandler);
        }

        private void DisconnectHandler(object obj)
        {
            _flowParameter = ApplicationWorkflow.LoginForm;
            _navigator.Goto(RegionNames.Connection);
        }

        private void PlayMoreHandler(object obj)
        {
            _flowParameter = ApplicationWorkflow.WaitNext;
            _navigator.Goto(RegionNames.Connection);
        }

        /// <summary>
        /// Player wants play more.
        /// </summary>
        public DelegateCommand PlayMoreCommand { get; }

        /// <summary>
        /// Player wants disconnect.
        /// </summary>
        public DelegateCommand DisconnectCommand { get; }

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
        public IPieceMasterFactory MasterFactory
        {
            get
            {
                return _masterFactory;
            }

            private set
            {
                _masterFactory = value;
                RaisePropertyChanged(() => MasterFactory);
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
                RaisePropertyChanged(() => Controller);
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

        /// <summary>
        /// Opponent name.
        /// </summary>
        public string OpponentName
        {
            get
            {
                return _opponentName;
            }

            set
            {
                _opponentName = value;
                RaisePropertyChanged(() => OpponentName);
            }
        }

        /// <summary>
        /// Game log text.
        /// </summary>
        public string GameLog
        {
            get
            {
                return _gameLog;
            }

            set
            {
                _gameLog = value;
                RaisePropertyChanged(() => GameLog);
            }
        }

        /// <summary>
        /// Is game ended.
        /// </summary>
        public bool IsGameEnded
        {
            get
            {
                return _isGameEnded;
            }

            set
            {
                _isGameEnded = value;
                RaisePropertyChanged(() => IsGameEnded);
            }
        }

        /// <summary>
        /// Game result text.
        /// </summary>
        public string GameResultText
        {
            get
            {
                return _gameResultText;
            }

            set
            {
                _gameResultText = value;
                RaisePropertyChanged(() => GameResultText); 
            }
        }

        /// <inheritdoc/>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsGameEnded = false;
            GameLog = string.Empty;
            MasterFactory.CheckedPlayer = null;

            var callback = _gameServiceProvider.ServiceCallback;
            var gameInfo = callback.GameInfo;
            var field = callback.GameInfo.GameField;
            var playerColor = callback.GameInfo.PlayerColor;

            TurnColor = gameInfo.TurnColor;
            YourColor = gameInfo.PlayerColor;
            GameField = new VirtualField(field.ToMultiDimensionalArray(), playerColor);
            OpponentName = gameInfo.OpponentName;

            callback.FieldUpdated -= OnFieldUpdated;
            callback.GameEnded -= OnGameEnded;

            callback.FieldUpdated += OnFieldUpdated;
            callback.GameEnded += OnGameEnded;
        }

        /// <inheritdoc/>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc/>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(nameof(ApplicationWorkflow), _flowParameter);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _controller.Movement -= OnChessPieceMovement;
        }

        private void OnGameEnded(object sender, GameEndedArgs args)
        {
            IsGameEnded = true;
            GameResultText = args.WinnerColor == _yourColor ? "Congratulations! You are winner!" : "You have lose.";
        }

        private void OnFieldUpdated(object sender, FieldInfoArgs args)
        {
            var field = args.VirtualField.ToMultiDimensionalArray();
            GameField = new VirtualField(field, args.PlayerColor);
            TurnColor = args.TurnColor;
            MasterFactory.CheckedPlayer = args.CheckedPlayer;

            LogSteps(args);
        }

        private void LogSteps(FieldInfoArgs args)
        {
            var turnChar = args.TurnColor.Invert().ToString().First();
            string step = $"{turnChar}: {args.FromPoint}-{args.ToPoint}";
            GameLog = $"{GameLog}{step}; ";
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