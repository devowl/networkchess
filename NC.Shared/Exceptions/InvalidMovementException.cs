using System;

namespace NC.Shared.Exceptions
{
    /// <summary>
    /// Invalid unit movement.
    /// </summary>
    [Serializable]
    public class InvalidMovementException : Exception
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Constructor for <see cref="InvalidMovementException"/>.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public InvalidMovementException(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}