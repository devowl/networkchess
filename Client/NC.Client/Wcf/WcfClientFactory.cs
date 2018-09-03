using NC.Client.Interfaces;

namespace NC.Client.Wcf
{
    /// <summary>
    /// <see cref="IWcfClient{TContract}"/> instances factory.
    /// </summary>
    /// <typeparam name="TContract">Contract type.</typeparam>
    internal class WcfClientFactory<TContract> : IWcfClientFactory<TContract>
    {
        private readonly WcfClient<TContract>.Factory _clientsFactory;

        /// <summary>
        /// Constructor for <see cref="WcfClientFactory{Contract}"/>.
        /// </summary>
        public WcfClientFactory(WcfClient<TContract>.Factory clientsFactory)
        {
            _clientsFactory = clientsFactory;
        }

        /// <summary>
        /// End point information.
        /// </summary>
        public IEndpointInfo EndpointInfo { get; set; }

        /// <inheritdoc/>
        public IWcfClient<TContract> Create()
        {
            return _clientsFactory();
        }

        /// <inheritdoc/>
        public IWcfClient<TContract> Create<TCallback>(TCallback callback) where TCallback : class
        {
            return new WcfDuplexClient<TContract, TCallback>(callback, EndpointInfo);
        }
    }
}