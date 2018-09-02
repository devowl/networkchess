using System;
using System.Threading.Tasks;

using NC.ChessControls.Prism;
using NC.Client.Shell;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// Wait view model.
    /// </summary>
    public class WaitViewModel : NotificationObject
    {
        private string _actionText;

        private bool _waiting;

        private bool _isPressed;

        private readonly string _cancelText;

        private readonly bool _canCancel;

        private readonly Action _cancelCallback;

        private string _defaultText;

        /// <summary>
        /// Constructor for <see cref="WaitViewModel"/>.
        /// </summary>
        public WaitViewModel(string defaultText, string cancelText, bool canCancel, Action cancelCallback)
        {
            ActionText = defaultText;

            _defaultText = defaultText;
            _cancelText = cancelText;
            _canCancel = canCancel;
            _cancelCallback = cancelCallback;
            CancelCommand = new DelegateCommand(CancelPressed, CanExecute);
        }

        private async void CancelPressed(object obj)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    _isPressed = true;
                    CancelCommand.RaiseCanExecuteChanged();
                    ActionText = _cancelText;
                    _cancelCallback?.Invoke();
                });
        }

        /// <summary>
        /// <see cref="WaitViewModel"/> factory.
        /// </summary>
        /// <param name="defaultText">Default text.</param>
        /// <param name="cancelText">Cancel text.</param>
        /// <param name="canCancel">Can press cancel.</param>
        /// <param name="cancelCallback">Cancel button press callback.</param>
        /// <returns></returns>
        public delegate WaitViewModel Factory(string defaultText, string cancelText = null, bool canCancel = false, Action cancelCallback = null);

        /// <summary>
        /// Waiting some operation.
        /// </summary>
        public bool Waiting
        {
            get
            {
                return _waiting;
            }

            set
            {
                _waiting = value;
                RaisePropertyChanged(() => Waiting);
            }
        }

        /// <summary>
        /// Cancel button.
        /// </summary>
        public DelegateCommand CancelCommand { get; }

        /// <summary>
        /// Action text.
        /// </summary>
        public string ActionText
        {
            get
            {
                return _actionText;
            }

            set
            {
                _actionText = value;
                RaisePropertyChanged(() => ActionText);
            }
        }
        
        /// <summary>
        /// Create wait operation.
        /// </summary>
        /// <returns>New wait operation.</returns>
        public WaitOperation Operation()
        {
            _isPressed = false;
            ActionText = _defaultText;
            return new WaitOperation(this);
        }

        private bool CanExecute(object obj)
        {
            return !_isPressed && _canCancel;
        }
    }
}