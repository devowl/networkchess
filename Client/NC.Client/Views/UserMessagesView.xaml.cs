using NC.Client.ViewModels;

namespace NC.Client.Views
{
    /// <summary>
    /// Interaction logic for UserMessages.xaml
    /// </summary>
    public partial class UserMessagesView
    {
        /// <summary>
        /// Constructor for <see cref="UserMessagesView"/>.
        /// </summary>
        public UserMessagesView(UserMessagesViewModel datacontext)
        {
            InitializeComponent();
            DataContext = datacontext;
        }
    }
}