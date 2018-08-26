using System;

namespace NC.Client.Interfaces
{
    /// <summary>
    /// Wcf client wrapper.
    /// </summary>
    public interface IWcfClient<out TContract> : IDisposable
    {
        /// <summary>
        /// Service proxy.
        /// </summary>
        TContract Service { get; }
    }
}