using System.Diagnostics;

namespace NC.Shared.Data
{
    /// <summary>
    /// Chess vector size.
    /// </summary>
    [DebuggerDisplay("[{X},{Y}]")]
    public class ChessVector
    {
        /// <summary>
        /// Constructor for <see cref="ChessVector"/>.
        /// </summary>
        public ChessVector(int x, int y)
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
    }
}