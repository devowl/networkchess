using System;
using System.Windows;

namespace NC.Client.Bootstrapper
{
    /// <summary>
    /// Startup application.
    /// </summary>
    internal class Startup : Application
    {
        private readonly ClientBootstrapper _clientBootstrapper;

        /// <summary>
        /// Constructor for <see cref="Startup"/>.
        /// </summary>
        public Startup()
        {
            _clientBootstrapper = new ClientBootstrapper();
        }

        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <remarks>Required for application build.</remarks>
        [STAThread]
        public static void Main()
        {
            new Startup().Run();
        }

        /// <inheritdoc/>
        [STAThread]
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            _clientBootstrapper.Run();
        }
    }
}