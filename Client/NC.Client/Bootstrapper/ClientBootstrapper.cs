using System.Windows;

using Autofac;

using Microsoft.Practices.Prism.Logging;

using NC.ChessControls;
using NC.Client.Views;

using Prism.AutofacExtension;

using MainWindow = NC.Client.Windows.MainWindow;

namespace NC.Client.Bootstrapper
{
    /// <summary>
    /// Client boots trapper.
    /// </summary>
    public class ClientBootstrapper : AutofacBootstrapper
    {
        /// <inheritdoc/>
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <inheritdoc/>
        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<ClientModule>();
            builder.RegisterModule<ChessControlsModule>();
        }

        /// <inheritdoc/>
        protected override ILoggerFacade CreateLogger()
        {
            return new TraceLogger();
        }

        /// <inheritdoc/>
        protected override void InitializeShell()
        {
            (Application.Current.MainWindow = Shell as Window)?.Show();
        }
    }
}