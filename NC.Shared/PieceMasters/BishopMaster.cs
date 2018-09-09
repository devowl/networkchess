using System.Collections.Generic;
using System.Linq;

using NC.Shared.Data;
using NC.Shared.GameField;

namespace NC.Shared.PieceMasters
{
    /// <summary>
    /// Bishop piece master.
    /// </summary>
    internal class BishopMaster : PieceMasterBase
    {
        /// <summary>
        /// Constructor for <see cref="BishopMaster"/>.
        /// </summary>
        public BishopMaster(VirtualField field, ChessPoint point, IPieceMasterFactory master)
            : base(field, point, master, ChessPiece.BlackBishop, ChessPiece.WhiteBishop)
        {
        }
        
        /// <inheritdoc/>
        protected override IEnumerable<ChessPoint> GetAvailableMovements(bool onlySteps = false)
        {
            /*******************
             * (-1,-1)       (1,-1)      
             *       (Bishop)
             * (-1,1)        (1,1)
             *******************/
            
            var vectors = new[]
            {
                new ChessVector(-1, -1),
                new ChessVector(1, -1),
                new ChessVector(-1, 1),
                new ChessVector(1, 1),
            };

            return vectors.SelectMany(GetVectorPathPoints);
        }
    }
}