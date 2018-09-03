namespace NC.Shared.Data
{
    /// <summary>
    /// Chess point.
    /// </summary>
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
    }
}