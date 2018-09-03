using System.Collections.Generic;
using System.Linq;
using System.Windows;

using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.GameField;

namespace NC.Shared.PieceMasters
{
    /// <summary>
    /// Queen piece master.
    /// </summary>
    internal class QueenMaster : PieceMasterBase
    {
        /// <summary>
        /// Constructor for <see cref="QueenMaster"/>.
        /// </summary>
        public QueenMaster(VirtualField field, int x, int y)
            : base(field, x, y, ChessPiece.BlackQueen, ChessPiece.WhiteQueen)
        {
        }

        /// <inheritdoc/>
        protected override IEnumerable<ChessPoint> GetAvailableMovements()
        {
            /*******************
             * (-1,-1)(0,-1)(1,-1)      
             * (-1,0) (Queen) (1,0)
             * (-1,1) (0,1) (1,1)
             *******************/

            var vectors = new[]
            {
                new ChessVector(-1, -1),
                new ChessVector(0, -1),
                new ChessVector(1, -1),
                new ChessVector(-1, 0),
                new ChessVector(1, 0),
                new ChessVector(-1, 1),
                new ChessVector(0, 1),
                new ChessVector(1, 1),
            };

            return vectors.SelectMany(GetVectorPathPoints);
        }
    }
}