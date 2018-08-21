using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.GameField;

namespace NC.Shared.PieceMasters
{
    /// <summary>
    /// Rook piece master.
    /// </summary>
    internal class RookMaster : PieceMasterBase
    {
        /// <summary>
        /// Constructor for <see cref="RookMaster"/>.
        /// </summary>
        public RookMaster(VirtualField field, int x, int y)
            : base(field, x, y, ChessPiece.BlackRook, ChessPiece.WhiteRook)
        {
        }

        protected override IEnumerable<Point> GetAvailableMovements()
        {
            /*******************
             *        (0,-1)    
             * (-1,0) (Rook) (1,0)
             *         (0,1)
             *******************/

            var vectors = new[]
            {
                new Vector(0, -1),
                new Vector(-1, 0),
                new Vector(1, 0),
                new Vector(0, 1)
            };

            return vectors.SelectMany(GetVectorPathPoints);
        }
    }
}
