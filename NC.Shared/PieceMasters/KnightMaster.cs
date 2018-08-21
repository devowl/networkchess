using System.Collections.Generic;
using System.Linq;
using System.Windows;

using NC.Shared.Contracts;
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
        protected override IEnumerable<Point> GetAvailableMovements()
        {
            var movements = new[]
            {
                new Vector(-2, -1),
                new Vector(-1, -2),
                new Vector(1, -2),
                new Vector(2, -1),
                new Vector(2, 1),
                new Vector(1, 2),
                new Vector(-1, 2),
                new Vector(-2, 1),
            };

            return movements.Select(vector => Point.Add(Position, vector)).Where(CanMove);
        }
    }
}