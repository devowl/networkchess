using System;
using System.Windows;

using NC.Shared.Data;

namespace NC.ChessControls.Data
{
    /// <summary>
    /// Chess game controller
    /// </summary>
    public class GameController
    {
        public event EventHandler<MovementArgs> Movement;

        /// <summary>
        /// Raise movement event. 
        /// </summary>
        internal void RaiseMovementEvent(ChessPoint frm, ChessPoint to)
        {
            Movement?.Invoke(this, new MovementArgs(frm, to));
        }
    }
}