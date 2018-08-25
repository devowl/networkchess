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
        /// <summary>
        /// Login in system.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <returns>User session guid.</returns>
        [OperationContract(IsInitiating = true)]
        bool Login(string login);

        /// <summary>
        /// Log out from system.
        /// </summary>
        [OperationContract(IsTerminating = true)]
        void Logout();
    }
}