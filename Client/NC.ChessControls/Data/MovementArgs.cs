using System;
using System.Windows;

using NC.Shared.Data;

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
        public MovementArgs(ChessPoint @from, ChessPoint to)
        {
            From = @from;
            To = to;
        }

        /// <summary>
        /// Movement from.
        /// </summary>
        public ChessPoint From { get; private set; }

        /// <summary>
        /// Movement to.
        /// </summary>
        public ChessPoint To { get; private set; }
    }
}