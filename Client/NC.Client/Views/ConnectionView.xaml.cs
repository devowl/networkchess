using NC.Client.ViewModels;

namespace NC.Client.Views
{
    /// <summary>
    /// Interaction logic for ConnectView.xaml
    /// </summary>
    public partial class ConnectionView
    {
        /// <summary>
        /// Constructor for <see cref="ConnectionView"/>.
        /// </summary>
        public ConnectionView(ConnectionViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}