using NC.Shared.Data;

namespace NC.ChessServer.Interfaces
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Move piece.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        /// <param name="from">Point from.</param>
        /// <param name="to">Point to.</param>
        void Move(string sessionId, ChessPoint from, ChessPoint to);
    }
}