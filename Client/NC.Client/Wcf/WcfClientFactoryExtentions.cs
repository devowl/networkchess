using System;

using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.ServiceLocation;

using NC.Client.Interfaces;

namespace NC.Client.Wcf
{
    /// <summary>
    /// <see cref="WcfClientFactory{TContract}"/> extensions.
    /// </summary>
    public static class WcfClientFactoryExtentions
    {
        private static ILoggerFacade _logger;

        private static ILoggerFacade Logger
            => _logger ?? (_logger = ServiceLocator.Current.GetInstance<ILoggerFacade>());

        /// <summary>
        /// Call <see cref="TContract"/> method.
        /// </summary>
        /// <typeparam name="TContract">Contract type.</typeparam>
        /// <param name="wcfClientFactory"></param>
        /// <param name="action"></param>
        public static void Use<TContract>(this IWcfClientFactory<TContract> wcfClientFactory, Action<TContract> action)
        {
            using (var wcfClient = wcfClientFactory.Create())
            {
                try
                {
                    action(wcfClient.Service);
                }
                catch (Exception exception)
                {
                    LogException(exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Call <see cref="TContract"/> method.
        /// </summary>
        /// <typeparam name="TContract">Contract type.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="wcfClientFactory"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T Use<TContract, T>(this IWcfClientFactory<TContract> wcfClientFactory, Func<TContract, T> action)
        {
            using (var wcfClient = wcfClientFactory.Create())
            {
                try
                {
                    return action(wcfClient.Service);
                }
                catch (Exception exception)
                {
                    LogException(exception);
                    throw;
                }
            }
        }

        private static void LogException(Exception exception)
        {
            Logger.Log(exception.ToString(), Category.Exception, Priority.High);
        }
    }
}