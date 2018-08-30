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
        
        /// <summary>
        /// Constructor for <see cref="LocalNavigator"/>.
        /// </summary>
        public LocalNavigator(IRegionManager regionManager, IServiceLocator serviceLocator)
        {
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
            _regionManager = regionManager;
        }

        /// <summary>
        /// Set default view.
        /// </summary>
        public void DefaultView()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ConnectionView));
            _regionManager.RegisterViewWithRegion(RegionNames.UserMessages, typeof(UserMessagesView));

            _regionManager.Regions[RegionNames.MainRegion].Add(typeof(ConnectionView));
            _regionManager.Regions[RegionNames.UserMessages].Add(typeof(UserMessagesView));
        }

        public void Goto(string game, object[] objects)
        {
            
        }
    }
}