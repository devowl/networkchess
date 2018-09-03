using System.ServiceModel;

using NC.Shared.Exceptions;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// Chess service contract.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChessServiceCallback))]
    public interface IChessService
    {
        /// <summary>
        /// Set your status to ready to play.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        [OperationContract(IsInitiating = true)]
        void Ready(string sessionId);

        /// <summary>
        /// Move your piece.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        /// <param name="from">Point from.</param>
        /// <param name="to">Point to.</param>
        [OperationContract]
        [FaultContract(typeof(CheaterException))]
        [FaultContract(typeof(InvalidMovementException))]
        void Move(string sessionId, WcfChessPoint from, WcfChessPoint to);
    }
}