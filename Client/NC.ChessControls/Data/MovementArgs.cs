using System;
using System.Windows;

namespace NC.ChessControls.Data
{
    /// <summary>
    /// Movement arguments.
    /// </summary>
    public class MovementArgs : EventArgs
    {
        /// <summary>
        /// Constructor for <see cref="MovementArgs"/>.
        /// </summary>
        public MovementArgs(Point @from, Point to)
        {
            From = @from;
            To = to;
        }

        /// <summary>
        /// Movement from.
        /// </summary>
        public Point From { get; private set; }

        /// <summary>
        /// Movement to.
        /// </summary>
        public Point To { get; private set; }
    }
}