using NC.ChessControls.Prism;
using NC.Client.Views;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// <see cref="MainWindow"/> view model.
    /// </summary>
    public class MainWindowViewModel : NotificationObject
    {
        /// <summary>
        /// Constructor for <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            ChessFieldViewModel = new ChessFieldViewModel();
        }

        /// <summary>
        /// Chess field view model.
        /// </summary>
        public ChessFieldViewModel ChessFieldViewModel { get; private set; }
    }
}