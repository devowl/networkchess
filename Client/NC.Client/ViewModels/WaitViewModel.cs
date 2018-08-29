using System;

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

        /// <summary>
        /// Constructor for <see cref="WaitViewModel"/>.
        /// </summary>
        public WaitViewModel(string actionText, bool canCancel, Action cancelCallback)
        {
            ActionText = actionText;
            CanCancel = canCancel;
            CancelCommand = new DelegateCommand(obj => cancelCallback?.Invoke());
        }

        /// <summary>
        /// <see cref="WaitViewModel"/> factory.
        /// </summary>
        /// <param name="actionText">Action text.</param>
        /// <param name="canCancel">Can press cancel.</param>
        /// <param name="cancelCallback">Cancel button press callback.</param>
        /// <returns></returns>
        public delegate WaitViewModel Factory(string actionText, bool canCancel = false, Action cancelCallback = null);

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
        /// Can press cancel button.
        /// </summary>
        public bool CanCancel { get; }

        /// <summary>
        /// Create wait operation.
        /// </summary>
        /// <returns>New wait operation.</returns>
        public WaitOperation Operation()
        {
            return new WaitOperation(this);
        }
    }
}