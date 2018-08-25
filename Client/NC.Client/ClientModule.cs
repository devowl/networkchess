using Autofac;

using NC.Client.Interfaces;
using NC.Client.Shell;
using NC.Client.ViewModels;
using NC.Client.Views;

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
            builder.RegisterType<ConnectionViewModel>();
            builder.RegisterType<GameView>();
            builder.RegisterType<GameViewModel>();
            builder.RegisterType<ClientModuleActivator>().AutoActivate().SingleInstance();
            builder.RegisterType<LocalNavigator>().SingleInstance();
            builder.RegisterType<UserMessagesView>().SingleInstance();
            builder.RegisterType<UserMessagesViewModel>().SingleInstance();
            builder.RegisterType<UserMessagesViewModel>().As<IUserMessage>(); 
        }
    }
}
