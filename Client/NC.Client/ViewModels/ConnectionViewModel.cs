using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NC.ChessControls.Prism;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// Server connection view model.
    /// </summary>
    public class ConnectionViewModel : NotificationObject
    {
        /// <summary>
        /// Constructor for <see cref="ConnectionViewModel"/>.
        /// </summary>
        public ConnectionViewModel()
        {
            _serverIp = "127.0.0.1";
            ConnectCommand = new DelegateCommand(OnConnect);
        }

        private string _serverIp;

        private string _serverIpError;

        /// <summary>
        /// Server IP address.
        /// </summary>
        public string ServerIp
        {
            get
            {
                return _serverIp;
            }

            set
            {
                _serverIp = value;
                RaisePropertyChanged(() => ServerIp);
            }
        }

        /// <summary>
        /// Connection error.
        /// </summary>
        public string ConnectionError
        {
            get
            {
                return _serverIpError;
            }
            set
            {
                _serverIpError = value;
                RaisePropertyChanged(() => ConnectionError);
            }
        }

        public DelegateCommand ConnectCommand { get; }

        private void OnConnect(object o) 
        {
            
        }
    }
}
