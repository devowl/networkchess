using System.ServiceModel;

namespace NC.ChessServer.Services
{
    /// <summary>
    /// Service base.
    /// </summary>
    [ServiceBehavior(
#if DEBUG
        IncludeExceptionDetailInFaults = true,
#endif
        InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class BaseService
    {
    }
}