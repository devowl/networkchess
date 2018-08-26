using Autofac;

using NC.Client.Interfaces;
using NC.Client.Shell;
using NC.Client.ViewModels;
using NC.Client.Views;
using NC.Client.Wcf;

using MainWindow = NC.Client.Windows.MainWindow;

namespace NC.Client
{
    /// <summary>
    /// Client module.
    /// </summary>
    public class ClientModule : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<MainWindow>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ConnectionView>();
            builder.RegisterType<ConnectionViewModel>().SingleInstance();
            builder.RegisterType<ConnectionViewModel>().As<IEndpointInfo>().SingleInstance();
            builder.RegisterType<GameView>();
            builder.RegisterType<GameViewModel>();
            builder.RegisterType<ClientModuleActivator>().AutoActivate().SingleInstance();
            builder.RegisterType<LocalNavigator>().SingleInstance();
            builder.RegisterType<UserMessagesView>().SingleInstance();
            builder.RegisterType<UserMessagesViewModel>().SingleInstance();
            builder.RegisterType<UserMessagesViewModel>().As<IUserMessage>();
            builder.RegisterType<UserMessagesViewModel>();

            builder.RegisterGeneric(typeof(WcfClient<>))
                .As(typeof(IWcfClient<>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();
            
            builder.RegisterGeneric(typeof(WcfClientFactory<>))
                .As(typeof(IWcfClientFactory<>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();
        }
    }
}
