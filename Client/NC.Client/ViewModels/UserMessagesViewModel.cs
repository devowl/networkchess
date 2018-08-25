using System.Collections.ObjectModel;

using NC.ChessControls.Prism;
using NC.Client.Interfaces;

namespace NC.Client.ViewModels
{
    /// <summary>
    /// User massages panel view model.
    /// </summary>
    public class UserMessagesViewModel : NotificationObject, IUserMessage
    {
        /// <summary>
        /// User messages collection.
        /// </summary>
        public ObservableCollection<string> UserMessages { get; } = new ObservableCollection<string>();

        /// <inheritdoc/>
        public void PushInfo(string message)
        {
            UserMessages.Add(message);
        }
    }
}