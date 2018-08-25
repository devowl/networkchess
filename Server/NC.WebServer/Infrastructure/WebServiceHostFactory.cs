using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

using Autofac.Integration.Wcf;

namespace NC.WebServer.Infrastructure
{
    /// <summary>
    /// Autofac <see cref="ServiceHost"/> factory.
    /// </summary>
    public class WebServerHostFactory : AutofacHostFactory
    {
        /// <inheritdoc/>
        protected override ServiceHost CreateSingletonServiceHost(object singletonInstance, Uri[] baseAddresses)
        {
            throw new NotSupportedException("Singleton isn't implemented");
        }
    }
}