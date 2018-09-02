using NC.ChessServer.GamePack;
using NC.Shared.Contracts;

namespace NC.ChessServer.Interfaces
{
    /// <summary>
    /// Players manager.
    /// </summary>
    public interface IPlayerManager
    {
        /// <summary>
        /// Add player to system queue.
        /// </summary>
        /// <param name="player">New <see cref="Player"/> instance.</param>
        void AddToQueue(Player player);

        /// <summary>
        /// Remove player from queue.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        void RemoveFromQueue(string sessionId);

        /// <summary>
        /// Player ready to play.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        /// <param name="callback">Client callback.</param>
        void Ready(string sessionId, IChessServiceCallback callback);

        /// <summary>
        /// Check session exists.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        /// <returns>Session exists.</returns>
        bool HasSession(string sessionId);
    }
}