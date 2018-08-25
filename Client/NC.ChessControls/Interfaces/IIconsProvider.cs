using System.Windows.Controls;

using NC.Shared.Contracts;

namespace NC.ChessControls.Interfaces
{
    /// <summary>
    /// Icons provider.
    /// </summary>
    public interface IIconsProvider
    {
        /// <summary>
        /// Get source icon.
        /// </summary>
        /// <param name="chessPiece"><see cref="ChessPiece"/> piece name.</param>
        /// <returns>Source icon.</returns>
        Image GetIcon(ChessPiece chessPiece);
    }
}