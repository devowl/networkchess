using System;
using System.Diagnostics;

namespace NC.Shared.Data
{
    /// <summary>
    /// Chess point.
    /// </summary>
    [DebuggerDisplay("[{X},{Y}]")]
    public class ChessPoint
    {
        /// <summary>
        /// Constructor for <see cref="ChessPoint"/>.
        /// </summary>
        public ChessPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Adds vector to point
        /// </summary>
        /// <param name="point">Point coordinates.</param>
        /// <param name="vector">Sum vector value.</param>
        /// <returns></returns>
        public static ChessPoint Add(ChessPoint point, ChessVector vector)
        {
            return new ChessPoint(point.X + vector.X, point.Y + vector.Y);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var point = obj as ChessPoint;
            if (point == null)
            {
                return false;
            }

            return point.X.Equals(X) && point.Y.Equals(Y);
        }

        /// <summary>
        /// Equals operator.
        /// </summary>
        /// <param name="point1">Point one.</param>
        /// <param name="point2">Point two.</param>
        /// <returns>Operation result.</returns>
        public static bool operator ==(ChessPoint point1, ChessPoint point2)
        {
            if (point1?.X == point2?.X)
                return point1?.Y == point2?.Y;
            return false;
        }

        /// <summary>
        /// None equals operator.
        /// </summary>
        /// <param name="point1">Point one.</param>
        /// <param name="point2">Point two.</param>
        /// <returns>Operation result.</returns>
        public static bool operator !=(ChessPoint point1, ChessPoint point2)
        {
            return !(point1 == point2);
        }

        /// <summary>
        /// Empty point.
        /// </summary>
        public static ChessPoint Empty = new ChessPoint(int.MinValue, int.MaxValue);

        /// <inheritdoc/>
        public override string ToString()
        {
            int x0 = Convert.ToInt32('a');
            int y0 = Convert.ToInt32('8');
            var x = (char)(x0 + X);
            var y = (char)(y0 - Y);

            return $"{x}{y}";
        }
    }
}