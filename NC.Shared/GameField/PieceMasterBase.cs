using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Shared.GameField
{
    /// <summary>
    /// Chess piece base class controller.
    /// </summary>
    public abstract class PieceMasterBase
    {
        protected const string BlackName = "Black";

        protected const string WhiteName = "White";

        /// <summary>
        /// White chess pieces.
        /// </summary>
        protected readonly ChessPiece[] White = {
            ChessPiece.WhiteQueen,
            ChessPiece.WhiteBishop,
            ChessPiece.WhiteKing,
            ChessPiece.WhitePawn,
            ChessPiece.WhiteKnight,
            ChessPiece.WhiteRook,
        };

        /// <summary>
        /// Black chess pieces.
        /// </summary>
        protected readonly ChessPiece[] Black = {
            ChessPiece.BlackQueen,
            ChessPiece.BlackBishop,
            ChessPiece.BlackKing,
            ChessPiece.BlackPawn,
            ChessPiece.BlackKnight,
            ChessPiece.BlackRook,
        };

        private string _sideName;

        /// <summary>
        /// Constructor for <see cref="PieceMasterBase"/>.
        /// </summary>
        protected PieceMasterBase(VirtualField field, int x, int y, params ChessPiece[] pieces)
        {
            Field = field;
            Piece = Field[x, y];

            if (!pieces.Any())
            {
                throw new InvalidOperationException("Need more then one piece");
            }

            if (!pieces.Contains(Piece))
            {
                throw new InvalidOperationException($"x={x} y={y} [{Piece}] isn't {string.Join(", ", pieces)}");
            }

            Position = new Point(x, y);
        }

        /// <summary>
        /// Virtual field.
        /// </summary>
        public VirtualField Field { get; }

        /// <summary>
        /// Current piece.
        /// </summary>
        public ChessPiece Piece { get; }

        /// <summary>
        /// Piece position.
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// Prefix name.
        /// </summary>
        protected string SideName => _sideName ?? (_sideName = CheckPrefix(Piece, BlackName) ? BlackName : WhiteName);

        private IEnumerable<Point> _movements;

        /// <summary>
        /// Get available movements for piece.
        /// </summary>
        /// <returns>Available points.</returns>
        public IEnumerable<Point> GetMovements()
        {
            return _movements ?? (_movements = GetAvailableMovements() ?? Enumerable.Empty<Point>());
        }

        /// <summary>
        /// Get available movements for piece.
        /// </summary>
        /// <returns>Available points.</returns>
        protected abstract IEnumerable<Point> GetAvailableMovements();

        /// <summary>
        /// Get vector path points.
        /// </summary>
        /// <param name="vector">Vector value.</param>
        /// <returns>Points on the path.</returns>
        protected IEnumerable<Point> GetVectorPathPoints(Vector vector)
        {
            var currentPos = Position;
            bool enemy = false;
            while (CanMove(currentPos = Point.Add(currentPos, vector)) && !enemy)
            {
                yield return currentPos;

                var targetPlace = Field[(int)currentPos.X, (int)currentPos.Y];
                var targetName = GetSideName(targetPlace);
                enemy = !string.IsNullOrEmpty(targetName) && SideName != GetSideName(targetPlace);
            }
        }

        /// <summary>
        /// Get move status.
        /// </summary>
        /// <param name="position">Check position.</param>
        /// <returns>Is not outside board.</returns> 
        protected bool CanMove(Point position)
        {
            int x = (int)position.X;
            int y = (int)position.Y;

            if (0 <= x && x < Field.Width && 0 <= y && y < Field.Height)
            {
                var targetPlace = Field[x, y];
                if (targetPlace == ChessPiece.Empty)
                {
                    return true;
                }

                var sideName = GetSideName(targetPlace);
                return SideName != sideName;
            }

            return false;
        }

        protected static bool CheckPrefix(ChessPiece piece, string prefix)
        {
            return piece.ToString().StartsWith(prefix);
        }

        private string GetSideName(ChessPiece piece)
        {
            if (White.Contains(piece))
            {
                return WhiteName;
            }

            if (Black.Contains(piece))
            {
                return BlackName;
            }

            return string.Empty;
        }
    }
}