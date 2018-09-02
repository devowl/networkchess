using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace NC.ChessControls.Prism
{
    /// <summary>
    /// DelegateCommand implementation.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructor for <see cref="DelegateCommand"/>.
        /// </summary>
        public DelegateCommand(Action<object> execute)
                       : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor for <see cref="DelegateCommand"/>.
        /// </summary>
        public DelegateCommand(Action<object> execute,
                       Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <inheritdoc/>
        bool ICommand.CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        /// <inheritdoc/>
        void ICommand.Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Raise can execute command.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                        }));
            }
        }
    }
}
