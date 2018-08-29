using NC.ChessServer.GamePack;

namespace NC.ChessServer.Interfaces
{
    /// <summary>
    /// Players manager.
    /// </summary>
    public interface IPlayerManager
    {
        void AddToQueue(Player player);

        void Ready(string sessionId);

        bool HasSession(string sessionId);
    }
}