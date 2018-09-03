using System;

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
        /// Constructor for <see cref="FieldInfoArgs"/>.
        /// </summary>
        public FieldInfoArgs(
            ChessPiece[][] virtualField,
            PlayerColor turnColor,
            ChessPoint fromPoint,
            ChessPoint toPoint,
            PlayerColor playerColor)
        {
            VirtualField = virtualField;
            TurnColor = turnColor;
            FromPoint = fromPoint;
            ToPoint = toPoint;
            PlayerColor = playerColor;
        }

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
        public ChessPoint FromPoint { get; }

        /// <summary>
        /// Last one movement to.
        /// </summary>
        public ChessPoint ToPoint { get; }

        /// <summary>
        /// Player color.
        /// </summary>
        public PlayerColor PlayerColor { get; }
    }
}