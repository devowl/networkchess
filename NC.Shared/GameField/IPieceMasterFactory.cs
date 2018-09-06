using NC.Shared.Data;

namespace NC.Shared.GameField
{
    /// <summary>
    /// Piece master factory.
    /// </summary>
    public interface IPieceMasterFactory
    {
        /// <summary>
        /// Try get master for cell.
        /// </summary>
        /// <param name="field">Game field.</param>
        /// <param name="point">Cell coordinate.</param>
        /// <param name="master">Master reference.</param>
        /// <returns>Has master for cell.</returns>
        bool TryGetMaster(VirtualField field, ChessPoint point, out PieceMasterBase master);
    }
}