using System.Collections.Generic;
using System.Linq;

using NC.Shared.Data;
using NC.Shared.GameField;

namespace NC.Shared.PieceMasters
{
    /// <summary>
    /// Knight piece master.
    /// </summary>
    internal class KnightMaster : PieceMasterBase
    {
        /// <summary>
        /// Constructor for <see cref="KnightMaster"/>.
        /// </summary>
        public KnightMaster(VirtualField field, int x, int y)
            : base(field, x, y, ChessPiece.BlackKnight, ChessPiece.WhiteKnight)
        {
        }

        /// <inheritdoc/>
        protected override IEnumerable<ChessPoint> GetAvailableMovements()
        {
            var movements = new[]
            {
                new ChessVector(-2, -1),
                new ChessVector(-1, -2),
                new ChessVector(1, -2),
                new ChessVector(2, -1),
                new ChessVector(2, 1),
                new ChessVector(1, 2),
                new ChessVector(-1, 2),
                new ChessVector(-2, 1),
            };

            return movements.Select(vector => ChessPoint.Add(Position, vector)).Where(CanMove);
        }
    }
}