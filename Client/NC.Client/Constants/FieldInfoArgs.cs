using System;
using System.Windows;

using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Client.Constants
{
    /// <summary>
    /// Field information arguments.
    /// </summary>
    public class FieldInfoArgs : EventArgs
    {
        /// <summary>
        /// Virtual game field.
        /// </summary>
        public ChessPiece[][] VirtualField { get; }

        /// <summary>
        /// Player turn color.
        /// </summary>
        public PlayerColor TurnColor { get; }

        /// <summary>
        /// Last one movement from.
        /// </summary>
        public Point FromPoint { get; }

        /// <summary>
        /// Last one movement to.
        /// </summary>
        public Point ToPoint { get; }

        /// <summary>
        /// Constructor for <see cref="FieldInfoArgs"/>.
        /// </summary>
        public FieldInfoArgs(ChessPiece[][] virtualField, PlayerColor turnColor, Point fromPoint, Point toPoint)
        {
            VirtualField = virtualField;
            TurnColor = turnColor;
            FromPoint = fromPoint;
            ToPoint = toPoint;
        }
    }
}