using Autofac;

using NC.Client.Interfaces;
using NC.Client.Models;
using NC.Client.Shell;
using NC.Client.ViewModels;
using NC.Client.Views;
using NC.Client.Wcf;
using NC.Client.Windows;

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
            builder.RegisterType<MainWindow>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionView>();
            builder.RegisterType<ConnectionViewModel>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<GameView>();
            builder.RegisterType<GameViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ClientModuleActivator>().AutoActivate().SingleInstance();
            builder.RegisterType<LocalNavigator>().SingleInstance();
            builder.RegisterType<UserMessagesView>().SingleInstance();
            builder.RegisterType<UserMessagesViewModel>().As<IUserMessage>().AsSelf().SingleInstance();

            builder.RegisterGeneric(typeof(WcfClient<>))
                .As(typeof(IWcfClient<>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterGeneric(typeof(WcfClientFactory<>))
                .As(typeof(IWcfClientFactory<>))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<ChessServiceCallback>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<WaitViewModel>();
        }
    }
}