using System;

using NC.Shared.Contracts;
using NC.Shared.Exceptions;

namespace NC.Shared.Data
{
    /// <summary>
    /// Mono life grid model.
    /// </summary>
    public class VirtualField
    {
        private readonly ChessPiece[,] _sourceArray;

        /// <summary>
        /// Constructor for <see cref="VirtualField"/>.
        /// </summary>
        public VirtualField()
        {
            _sourceArray = VirtualFieldUtils.CreateEmptyField();
        }
         
        /// <summary>
        /// Constructor for <see cref="VirtualField"/>.
        /// </summary>
        public VirtualField(ChessPiece[,] sourceArray)
        {
            _sourceArray = sourceArray;
        }

        /// <summary>
        /// Grid width.
        /// </summary>
        public int Width => _sourceArray.GetLength(0);

        /// <summary>
        /// Grid height.
        /// </summary>
        public int Height => _sourceArray.GetLength(1);

        /// <summary>
        /// Get point black state.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Is black point.</returns>
        public ChessPiece this[int x, int y]
        {
            get
            {
                /* ********** X ***********
                 * [0, 0] | .... | [8, 0]
                 * ************************
                 Y          ....
                 * ************************
                 * [0, 8] | .... | [8, 8]
                 * ************************/
                CheckBounds(x, y);
                return _sourceArray[x, y];
            }

            set
            {
                CheckBounds(x, y);
                _sourceArray[x, y] = value;
            }
        }

        /// <summary>
        /// Clone matrix field.
        /// </summary>
        /// <returns>Matrix field.</returns>
        public ChessPiece[,] CloneMatrix()
        {
            return (ChessPiece[,])_sourceArray.Clone();
        }

        private void CheckBounds(int x, int y)
        {
            if (0 <= x && x < Width && 0 <= y && y < Height)
            {
                return;
            }

            throw new InvalidMovementException(x, y);
        }
    }
}