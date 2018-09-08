using System.Collections.Generic;
using System.Linq;

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
        public RookMaster(VirtualField field, ChessPoint point)
            : base(field, point, ChessPiece.BlackRook, ChessPiece.WhiteRook)
        {
        }

        protected override IEnumerable<ChessPoint> GetAvailableMovements(bool onlySteps = false)
        {
            /*******************
             *        (0,-1)    
             * (-1,0) (Rook) (1,0)
             *         (0,1)
             *******************/

            var vectors = new[]
            {
                new ChessVector(0, -1),
                new ChessVector(-1, 0),
                new ChessVector(1, 0),
                new ChessVector(0, 1)
            };

            return vectors.SelectMany(GetVectorPathPoints);
        }
    }
}