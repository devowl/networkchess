using System;
using System.ServiceModel;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// User service contract.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IUserService
    {
        /// <summary>
        /// Login in system.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <param name="sessionId">Session Id.</param>
        /// <returns>User session guid.</returns>
        [OperationContract(IsInitiating = true)]
        bool Login(string login, out string sessionId);

        /// <summary>
        /// Log out from system.
        /// </summary>
        [OperationContract(IsTerminating = true)]
        void Logout(string sessionId);
    }
}