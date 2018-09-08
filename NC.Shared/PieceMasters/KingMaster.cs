using System.Collections.Generic;
using System.Linq;

using NC.Shared.Data;
using NC.Shared.GameField;

namespace NC.Shared.PieceMasters
{
    /// <summary>
    /// King piece master.
    /// </summary>
    internal class KingMaster : PieceMasterBase
    {
        /// <summary>
        /// Constructor for <see cref="KingMaster"/>.
        /// </summary>
        public KingMaster(VirtualField field, ChessPoint point)
            : base(field, point, ChessPiece.BlackKing, ChessPiece.WhiteKing)
        {
        }

        /// <inheritdoc/>
        protected override IEnumerable<ChessPoint> GetAvailableMovements(bool onlySteps = false)
        {
            /*******************
             * (-1,-1)(0,-1)(1,-1)      
             * (-1,0) (King) (1,0)
             * (-1,1) (0,1) (1,1)
             *******************/

            // АЛГОРИТМ
            // Выясняем куда ВСЕ фигуры могу ходить, затем создаём поле где проставляем все движения всех фигур и выясняем куда можно куда нельзя ходить. НЕ ЗАБЫВАЕМ что
            // Стоит не забывать что фигура достающая до союзной - защищает НО НАМ ЭТО НЕ НАДО
            // Check cells around
            var movements = new[]
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

            var avaliableMovements =
                movements.Select(vector => ChessPoint.Add(Position, vector))
                    .Where(CanMove)  .ToArray();

            if (onlySteps)
            {
                return avaliableMovements;
            }

            var piecePlayerColor = Field[Position].GetPlayerColor().Value;
            var opponentColorInitiator = piecePlayerColor.Invert();
            var underAttackPoints = CheckMateLogic.UnderAttackPoints(opponentColorInitiator, Field, new PieceMasterFactory());
            return avaliableMovements.Where(point => !underAttackPoints.Contains(point)).ToArray();
        }
    }
}