using System;
using System.Windows;

using Autofac;

using Microsoft.Practices.Prism.Logging;

using NC.ChessControls;
using NC.Client.Windows;

using Prism.AutofacExtension;

namespace NC.Client.Bootstrapper
{
    /// <summary>
    /// Client boots trapper.
    /// </summary>
    public class ClientBootstrapper : AutofacBootstrapper
    {
        public override void Run(bool runWithDefaultConfiguration)
        {
            SubscriptionOnUnhandledException();
            base.Run(runWithDefaultConfiguration);
        }

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

        private void SubscriptionOnUnhandledException()
        {
            var domain = AppDomain.CurrentDomain;
            var application = Application.Current;

            domain.UnhandledException += (sender, args) => LogException(args.ExceptionObject as Exception);

            application.DispatcherUnhandledException += (sender, args) =>
                                                        {
                                                            LogException(args.Exception);
                                                            args.Handled = true;
                                                        };
        }

        private void LogException(Exception exception)
        {
            var logger = CreateLogger();
            logger.Log(exception.ToString(), Category.Exception, Priority.High);
        }
    }
}