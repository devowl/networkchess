using System;

using NC.Client.Interfaces;

namespace NC.Client.Wcf
{
    /// <summary>
    /// <see cref="WcfClientFactory{TContract}"/> extentions.
    /// </summary>
    public static class WcfClientFactoryExtentions
    {
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
                    // handle
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
                    // handle
                    throw;
                }
            }
        }
    }
}