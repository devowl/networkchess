using NC.Client.ViewModels;

namespace NC.Client.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView
    {
        /// <summary>
        /// Constructor for <see cref="GameView"/>.
        /// </summary>
        public GameView(GameViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}