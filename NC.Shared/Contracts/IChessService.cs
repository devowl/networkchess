using System;
using System.ServiceModel;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// Chess service contract.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChessServiceCallback))]
    public interface IChessService
    {
        [OperationContract(IsInitiating = true)]
        void Ready(string sessionId);

        [OperationContract]
        void Move(string sessionId, int x1, int y1, int x2, int y2);
    }
}