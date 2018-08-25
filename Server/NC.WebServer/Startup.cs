using System.Threading;

using Autofac;
using Autofac.Integration.Wcf;

using Microsoft.Owin.BuilderProperties;

using Owin;

namespace NC.WebServer
{
    /// <summary>
    /// Owin start up module.
    /// </summary>
    /// <remarks>More info https://autofaccn.readthedocs.io/en/latest/integration/owin.html </remarks>
    public class Startup
    {
        /// <summary>
        /// Static container reference.
        /// </summary>
        internal IContainer Container { get; set; }
        
        /// <summary>
        /// Owin configuration entry point.
        /// </summary>
        /// <param name="app"><see cref="IAppBuilder"/> implementation.</param>
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WebServerModule>();
            AutofacHostFactory.Container = builder.Build();

            var appProperties = new AppProperties(app.Properties);
            if (appProperties.OnAppDisposing != CancellationToken.None)
            {
                appProperties.OnAppDisposing.Register(
                    () =>
                    {
                        // On server shutdown (dispose)
                        Container.Dispose();
                    });
            }
        }
    }
}