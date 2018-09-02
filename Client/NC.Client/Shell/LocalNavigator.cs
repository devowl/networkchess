using System;
using System.Windows;
using System.Windows.Threading;

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

        /// <summary>
        /// Go to view.
        /// </summary>
        /// <param name="viewName">View name.</param>
        public void Goto(string viewName)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (dispatcher.CheckAccess())
            {
                InternalGoto(viewName);
            }
            else
            {
                dispatcher.Invoke(
                    new Action(
                        () =>
                        {
                            InternalGoto(viewName);
                        }));
            }
        }

        private void InternalGoto(string viewName) 
        {
            var region = _regionManager.Regions[RegionNames.MainRegion];
            region.RequestNavigate(viewName);
        }
    }
}