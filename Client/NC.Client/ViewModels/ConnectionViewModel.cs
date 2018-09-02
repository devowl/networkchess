﻿using System;
using System.Threading.Tasks;

using NC.ChessControls.Prism;
using NC.Client.Constants;
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
    public class ConnectionViewModel : NotificationObject, IEndpointInfo, IGameServiceProvider
    {
        private readonly IWcfClientFactory<IUserService> _userService;

        private readonly ChessServiceCallback _serviceCallback;

        private readonly LocalNavigator _navigator;

        private readonly WaitViewModel _waitOpponent;

        private readonly WaitViewModel _waitConnect;

        private readonly IWcfClientFactory<IChessService> _chessService;

        private string _connectionError;

        private string _serverAddress;

        private WaitViewModel _waitViewModel;

        private IWcfClient<IChessService> _chessClient;

        /// <summary>
        /// Constructor for <see cref="ConnectionViewModel"/>.
        /// </summary>
        public ConnectionViewModel(
            IWcfClientFactory<IUserService> userService,
            IWcfClientFactory<IChessService> chessService,
            ChessServiceCallback serviceCallback,
            WaitViewModel.Factory waitFactory,
            LocalNavigator navigator)
        {
            _chessService = chessService;
            _userService = userService;
            _serviceCallback = serviceCallback;
            _navigator = navigator;
            _waitOpponent = waitFactory("Awaiting new opponent...", "Disconnecting...", true, CancelCallback);
            _waitViewModel = _waitConnect = waitFactory("Connecting to server...");
            _serverAddress = "localhost";
            ConnectCommand = new DelegateCommand(OnConnect);

            _serviceCallback.GameStarted -= OnGameStarted;
            _serviceCallback.GameStarted += OnGameStarted;
        }

        /// <inheritdoc/>
        public IWcfClient<IChessService> ChessClient => _chessClient;

        /// <inheritdoc/>
        public ChessServiceCallback ServiceCallback => _serviceCallback;

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

        /// <inheritdoc/>
        public DelegateCommand ConnectCommand { get; }

        /// <inheritdoc/>
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

        private void CancelCallback()
        {
            _userService.Use(service => service.Logout(SessionId));
            _chessClient?.Dispose();
            _waitViewModel.Waiting = false;
        }

        private async void OnConnect(object o)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        using (ConnectionView())
                        {
                            string sessionId = null;
                            if (_userService.Use(service => service.Login(Guid.NewGuid().ToString(), out sessionId)))
                            {
                                SessionId = sessionId;
                                _chessClient = _chessService.Create(_serviceCallback);
                                _chessClient.Service.Ready(sessionId);

                                OpponentView();
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
                });
        }

        private void OnGameStarted(object sender, EventArgs eventArgs)
        {
            _navigator.Goto(RegionNames.Game);
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
    }
}