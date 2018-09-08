using System.Linq;
using System.Text;

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
        public VirtualField(ChessPiece[,] sourceArray, PlayerColor? playerColor = null)
        {
            PlayerColor = playerColor;
            _sourceArray = sourceArray;
        }

        /// <summary>
        /// Player color.
        /// </summary>
        public PlayerColor? PlayerColor { get; private set; }

        /// <summary>
        /// Grid width.
        /// </summary>
        public int Width => _sourceArray.GetLength(0);

        /// <summary>
        /// Grid height.
        /// </summary>
        public int Height => _sourceArray.GetLength(1);

        /// <summary>
        /// Get piece.
        /// </summary>
        /// <param name="point">Field point.</param>
        /// <returns>Is black point.</returns>
        public ChessPiece this[ChessPoint point]
        {
            get
            {
                return this[point.X, point.Y];
            }

            set
            {
                this[point.X, point.Y] = value;
            }
        }

        /// <summary>
        /// Get piece.
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

            throw new InvalidMovementException(x, y, "Out of bounds");
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var buffer = new StringBuilder();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    var piece = this[x, y];
                    if (piece == ChessPiece.Empty)
                    {
                        buffer.Append("__ ");
                    }
                    else
                    {
                        var playerColor = piece.GetPlayerColor().ToString();
                        var pieceShortName = piece.ToString().Substring(playerColor.Length).First();
                        var playerShortName = playerColor.First();
                        buffer.Append($"{playerShortName}{pieceShortName} ");
                    }
                }

                buffer.AppendLine();
            }

            return buffer.ToString();
        }
    }
}