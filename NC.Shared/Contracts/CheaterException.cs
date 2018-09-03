using System;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// Cheater player exception.
    /// </summary>
    [Serializable]
    public class CheaterException : Exception
    {
        /// <summary>
        /// Constructor for <see cref="CheaterException"/>.
        /// </summary>
        public CheaterException(string message) : base(message)
        {
        }
    }
}