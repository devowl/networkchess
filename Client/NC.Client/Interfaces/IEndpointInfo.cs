namespace NC.Client.Interfaces
{
    /// <summary>
    /// Endpoint information.
    /// </summary>
    public interface IEndpointInfo
    {
        /// <summary>
        /// Remote server address.
        /// </summary>
        string ServerAddress { get; }

        /// <summary>
        /// Player session Id.
        /// </summary>
        string SessionId { get; }
    }
}