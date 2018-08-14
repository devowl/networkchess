using NC.ChessControls.Prism;
using NC.Shared.Data;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// Virtual chess field view model.
    /// </summary>
    public class ChessFieldViewModel : NotificationObject
    {
        /// <summary>
        /// Constructor for <see cref="ChessFieldViewModel"/>.
        /// </summary>
        public ChessFieldViewModel()
        {
            _gameField = new VirtualField();
        }

        private VirtualField _gameField;

        /// <summary>
        /// Chess game field.
        /// </summary>
        public VirtualField GameField
        {
            get
            {
                return _gameField;
            }

            private set
            {
                RaisePropertyChanged(() => GameField);
                _gameField = value;
            }
        }
    }
}