using System;
using System.Windows;

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
        internal void RaiseMovementEvent(Point frm, Point to)
        {
            Movement?.Invoke(this, new MovementArgs(frm, to));
        }
    }
}