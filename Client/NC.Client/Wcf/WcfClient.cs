using System;
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
        private readonly string _address;

        private TContract _service;

        private bool _closed;

        /// <summary>
        /// Constructor for <see cref="WcfClient{TContract}"/>.
        /// </summary>
        public WcfClient(string address)
        {
            _address = address;
        }

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
            // Pattern http://msdn.microsoft.com/en-us/library/aa355056.aspx

            if (_service == null || _closed)
            {
                return;
            }

            _closed = true;

            var client = Service as ICommunicationObject;
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
        
        private TContract CreateService()
        {
            Binding binding = GetBinding();
            EndpointAddress endpoint = GetEndpoint();
            return ChannelFactory<TContract>.CreateChannel(binding, endpoint);
        }

        private EndpointAddress GetEndpoint()
        {
            var serviceName = typeof(TContract).Name;
            var address = $"{_address}/WebServices/";
            return null;
        }

        private Binding GetBinding()
        {
            throw new System.NotImplementedException();
        }
    }
}