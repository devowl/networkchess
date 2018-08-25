using System.Windows;

using Microsoft.Practices.ServiceLocation;

using NC.Client.Interfaces;
using NC.Client.Shell;

namespace NC.Client.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainWindow
    {
        /// <summary>
        /// Constructor for <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnMainWindowLoaded;

            var navigator = ServiceLocator.Current.GetInstance<LocalNavigator>();
            navigator.DefaultView();
        }
    }
}