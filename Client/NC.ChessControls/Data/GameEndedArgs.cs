using System;

using NC.Shared.Contracts;

namespace NC.ChessControls.Data
{
    /// <summary>
    /// Game ended arguments.
    /// </summary>
    public class GameEndedArgs : EventArgs
    {
        /// <summary>
        /// Constructor for <see cref="GameEndedArgs"/>.
        /// </summary>
        public GameEndedArgs(PlayerColor? winnerColor)
        {
            WinnerColor = winnerColor;
        }

        /// <summary>
        /// Winner player color.
        /// </summary>
        public PlayerColor? WinnerColor { get; } 
    }
}