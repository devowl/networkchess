using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

using NC.Client.Interfaces;

namespace NC.Client.Wcf
{
    /// <summary>
    /// Wcf client wrapper.
    /// </summary>
    /// <typeparam name="TContract">Contract type.</typeparam>
    public class WcfClient<TContract> : IWcfClient<TContract>
    {
        private const int DefaultPort = 7007;

        private readonly string _serviceName;

        private TContract _service;

        private bool _closed;

        /// <summary>
        /// Constructor for <see cref="WcfClient{TContract}"/>.
        /// </summary>
        public WcfClient()
        {
            _serviceName = typeof(TContract).Name;

            if (_serviceName.StartsWith("I"))
            {
                _serviceName = _serviceName.Substring(1, _serviceName.Length - 1);
            }
        }

        /// <summary>
        /// <see cref="IWcfClient{TContract}"/> factory.
        /// </summary>
        /// <returns><see cref="IWcfClient{TContract}"/> instance.</returns>
        public delegate IWcfClient<TContract> Factory();

        /// <summary>
        /// End point information.
        /// </summary>
        public IEndpointInfo EndpointInfo { get; set; }

        /// <inheritdoc/>
        public TContract Service
        {
            get
            {
                if (_service == null)
                {
                    _service = CreateService();
                }

                return _service;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // http://msdn.microsoft.com/en-us/library/aa355056.aspx
            if (_service == null || _closed)
            {
                return;
            }

            _closed = true;

            var client = _service as ICommunicationObject;
            try
            {
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
                throw;
            }
        }

        /// <summary>
        /// Create service instance.
        /// </summary>
        /// <returns>Service instance.</returns>
        protected virtual TContract CreateService()
        {
            var binding = GetBinding();
            if (binding == null)
            {
                throw new ArgumentNullException($"Binding not founded for service {_serviceName}");
            }

            var endpoint = GetEndpoint(binding, EndpointInfo.ServerAddress);

            Debug.WriteLine(endpoint.Uri);

            return ChannelFactory<TContract>.CreateChannel(binding, endpoint);
        }

        /// <summary>
        /// Get service endpoint.
        /// </summary>
        /// <param name="binding">Service binding.</param>
        /// <param name="serviceAddress">Service address.</param>
        /// <returns>Service endpoint.</returns>
        protected EndpointAddress GetEndpoint(Binding binding, string serviceAddress)
        {
            var scheme = binding.Scheme;
            var address = $"{scheme}://{serviceAddress}:{DefaultPort}/WebServices/{_serviceName}.svc";
            return new EndpointAddress(address);
        }
        
        /// <summary>
        /// Get service binding.
        /// </summary>
        /// <returns>Service binding.</returns>
        protected Binding GetBinding()
        {
            return WcfClientUtils.ResolveBinding(_serviceName);
        }
    }
}