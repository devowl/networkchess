using NC.ChessControls.Prism;
using NC.Client.Interfaces;
using NC.Client.Models;
using NC.Client.Wcf;
using NC.Shared.Contracts;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// Server connection view model.
    /// </summary>
    public class ConnectionViewModel : NotificationObject, IEndpointInfo
    {
        private readonly IWcfClientFactory<IChessService> _chessService;

        private string _connectionError;

        private string _serverAddress;

        private IChessServiceCallback _serviceCallback;

        private IWcfClient<IChessService> _serviceClient;

        /// <summary>
        /// Constructor for <see cref="ConnectionViewModel"/>.
        /// </summary>
        public ConnectionViewModel(IWcfClientFactory<IChessService> chessService)
        {
            _chessService = chessService;
            _serverAddress = "localhost";
            ConnectCommand = new DelegateCommand(OnConnect);
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
            _serviceCallback = new ChessServiceCallback();
            _serviceClient = _chessService.Create(_serviceCallback);

            var r = _serviceClient.Service.Login("abcd");
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
    }
}