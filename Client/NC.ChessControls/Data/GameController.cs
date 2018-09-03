using System;

using NC.Shared.Data;

namespace NC.ChessControls.Data
{
    /// <summary>
    /// Chess game controller
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// Piece movement.
        /// </summary>
        public event EventHandler<MovementArgs> Movement;

        /// <summary>
        /// Raise movement event. 
        /// </summary>
        /// <param name="from">Point from.</param>
        /// <param name="to">Point to.</param>
        internal void RaiseMovementEvent(ChessPoint from, ChessPoint to)
        {
            Movement?.Invoke(this, new MovementArgs(from, to));
        }
    }
}