namespace NC.Client.Interfaces
{
    /// <summary>
    /// <see cref="IWcfClient{TContract}"/> instances factory.
    /// </summary>
    /// <typeparam name="TContract">Contract type.</typeparam>
    public interface IWcfClientFactory<out TContract>
    {
        /// <summary>
        /// Create client instance.
        /// </summary>
        /// <returns>Client instance.</returns>
        IWcfClient<TContract> Create();
         
        /// <summary>
        /// Create duplex instance.
        /// </summary>
        /// <typeparam name="TCallback">Callback type.</typeparam>
        /// <param name="callback">Callback instance.</param>
        /// <returns>Client instance.</returns>
        IWcfClient<TContract> Create<TCallback>(TCallback callback) where TCallback : class;
    }
}