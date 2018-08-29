using System;

using NC.ChessControls.Prism;
using NC.Client.Interfaces;
using NC.Client.Models;
using NC.Client.Shell;
using NC.Client.Wcf;
using NC.Shared.Contracts;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// Server connection view model.
    /// </summary>
    public class ConnectionViewModel : NotificationObject, IEndpointInfo
    {
        private readonly IWcfClientFactory<IUserService> _userService;

        private string _connectionError;

        private string _serverAddress;

        private readonly IChessServiceCallback _serviceCallback;

        private readonly LocalNavigator _navigator;

        private readonly ChessCallback _chessCallback;

        private readonly WaitViewModel _waitOpponent;

        private readonly WaitViewModel _waitConnect;

        private IWcfClientFactory<IChessService> _chessService;

        private WaitViewModel _waitViewModel;

        private IWcfClient<IChessService> _chessClient;

        /// <summary>
        /// Constructor for <see cref="ConnectionViewModel"/>.
        /// </summary>
        public ConnectionViewModel(
            IWcfClientFactory<IUserService> userService,
            IWcfClientFactory<IChessService> chessService,
            IChessServiceCallback serviceCallback,
            WaitViewModel.Factory waitFactory,
            LocalNavigator navigator,
            ChessCallback chessCallback)
        {
            _chessService = chessService;
            _userService = userService;
            _serviceCallback = serviceCallback;
            _navigator = navigator;
            _chessCallback = chessCallback;
            _waitOpponent = waitFactory("Awaiting new opponent...", true, CancelCallback);
            _waitViewModel = _waitConnect = waitFactory("Connecting to server...");
            _serverAddress = "localhost";
            ConnectCommand = new DelegateCommand(OnConnect);
        }

        private void CancelCallback()
        {
            
        }

        /// <summary>
        /// Wait control view model.
        /// </summary>
        public WaitViewModel WaitViewModel
        {
            get
            {
                return _waitViewModel;
            }

            set
            {
                _waitViewModel = value;
                RaisePropertyChanged(() => WaitViewModel);
            }
        }

        /// <summary>
        /// Connection error.
        /// </summary>
        public string ConnectionError
        {
            get
            {
                return _connectionError;
            }
            set
            {
                _connectionError = value;
                RaisePropertyChanged(() => ConnectionError);
            }
        }

        public DelegateCommand ConnectCommand { get; }
        
        private void OnConnect(object o)
        {
            try
            {
                using (ConnectionView())
                {
                    string sessionId = null;
                    if (_userService.Use(service => service.Login(Guid.NewGuid().ToString(), out sessionId)))
                    {
                        SessionId = sessionId;
                        _chessClient = _chessService.Create(_chessCallback);
                        _chessClient.Service.Ready(sessionId);
                    }
                    else
                    {
                        ConnectionError = "Login failed";
                    }
                }
            }
            catch (Exception ex)
            {
                ConnectionError = ex.Message;
            }
        }

        private WaitOperation ConnectionView()
        {
            if (WaitViewModel != _waitConnect)
            {
                WaitViewModel = _waitConnect;
            }

            ConnectionError = null;
            return _waitConnect.Operation();
        }

        private WaitOperation OpponentView() 
        {
            if (WaitViewModel != _waitOpponent)
            {
                WaitViewModel = _waitOpponent;
            }

            ConnectionError = null;
            return _waitOpponent.Operation();
        }

        public string ServerAddress
        {
            get
            {
                return _serverAddress;
            }

            set
            {
                _serverAddress = value;
                RaisePropertyChanged(() => ServerAddress);
            }
        }

        /// <inheritdoc/>
        public string SessionId { get; private set; }
    }
}