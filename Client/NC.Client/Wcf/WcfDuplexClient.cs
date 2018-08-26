using System.ServiceModel;

using NC.Client.Interfaces;

namespace NC.Client.Wcf
{
    /// <summary>
    /// Wcf duplex client wrapper.
    /// </summary>
    /// <typeparam name="TContract">Contract type.</typeparam>
    /// <typeparam name="TCallback">Callback type.</typeparam>
    internal class WcfDuplexClient<TContract, TCallback> : WcfClient<TContract>
    {
        private readonly TCallback _callaback;

        /// <summary>
        /// Constructor for <see cref="WcfDuplexClient{TContract, TCallback}"/>.
        /// </summary>
        public WcfDuplexClient(TCallback callaback, IEndpointInfo endpointInfo)
        {
            _callaback = callaback;
            EndpointInfo = endpointInfo;
        }

        /// <summary>
        /// <see cref="IWcfClient{TContract}"/> factory.
        /// </summary>
        /// <returns><see cref="IWcfClient{TContract}"/> instance.</returns>
        public delegate IWcfClient<TContract> DuplexFactory(TCallback callaback);

        /// <inheritdoc/>
        protected override TContract CreateService()
        {
            var binding = GetBinding();
            var endpoint = GetEndpoint(binding, EndpointInfo.ServerAddress);
            return DuplexChannelFactory<TContract>.CreateChannel(_callaback, binding, endpoint);
        }
    }
}