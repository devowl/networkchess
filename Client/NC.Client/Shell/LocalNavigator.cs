using Autofac;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

using NC.Client.Constants;
using NC.Client.Views;

namespace NC.Client.Shell
{
    /// <summary>
    /// Local pages navigator.
    /// </summary>
    public class LocalNavigator
    {
        private readonly IRegionManager _regionManager;

        private bool _isRegistred;

        /// <summary>
        /// Constructor for <see cref="LocalNavigator"/>.
        /// </summary>
        public LocalNavigator(IRegionManager regionManager, IServiceLocator serviceLocator)
        {
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
            _regionManager = regionManager;
        }

        private void RegionsRegistration()
        {
            if (_isRegistred)
            {
                return;
            }

            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ConnectionView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(GameView));
            _regionManager.RegisterViewWithRegion(RegionNames.UserMessages, typeof(UserMessagesView));

            _isRegistred = true;
        }

        /// <summary>
        /// Set default view.
        /// </summary>
        public void DefaultView()
        {
            RegionsRegistration();
            
        }

        public void Goto(string game, object[] objects)
        {
            var region = _regionManager.Regions[RegionNames.MainRegion];
            //region.RequestNavigate(game,);
            //_regionManager.RequestNavigate("", "", new object());
        }
    }
}