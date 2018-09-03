using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NC.Shared.Data
{
    /// <summary>
    /// Chess vector size.
    /// </summary>
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
        public int X { get; private set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; private set; }

    }
}
