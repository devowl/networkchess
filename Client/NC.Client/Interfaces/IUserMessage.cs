namespace NC.Client.Interfaces
{
    /// <summary>
    /// User message.
    /// </summary>
    public interface IUserMessage
    {
        /// <summary>
        /// Push information message.
        /// </summary>
        /// <param name="message">Message text.</param>
        void PushInfo(string message);
    }
}