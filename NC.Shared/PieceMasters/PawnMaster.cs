using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.GameField;

namespace NC.Shared.PieceMasters
{
    /// <summary>
    /// Pawn piece master.
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    internal class PawnMaster : PieceMasterBase
    {
        /// <summary>
        /// Constructor for <see cref="PawnMaster"/>.
        /// </summary>
        public PawnMaster(VirtualField field, ChessPoint point)
            : base(field, point, ChessPiece.BlackPawn, ChessPiece.WhitePawn)
        {
        }

        /// <inheritdoc/>
        protected override IEnumerable<ChessPoint> GetAvailableMovements()
        {
            // Black (top)
            /*******************
            *       (Pawn) 
            * (-1,1) (0,1)(1,1)
            *******************/

            // White (bottom)
            /*******************
            * (-1,-1)(0,-1) (1,-1)      
            *       (Pawn) 
            *******************/

            ChessVector[] movementsVectors;
            ChessVector[] attackMovementsVectors;
            PlayerColor opponent;

            // 1. Ходит тока в перёд
            // 2. В бок может тока есть
            // 3. Если на исходной позиции тогда может прыгнуть на 2
            if (SideName == PlayerColor.White)
            {
                opponent = PlayerColor.Black;
                attackMovementsVectors = new[]
                {
                    new ChessVector(-1, -1),
                    new ChessVector(1, -1)
                };

                movementsVectors = new[]
                {
                    new ChessVector(0, -1)
                };

                // If we do first one move as white
                if (Position.Y == 6)
                {
                    movementsVectors = movementsVectors.Union(
                        new[]
                        {
                            new ChessVector(0, -2)
                        }).ToArray();
                }
            }
            else
            {
                opponent = PlayerColor.White;
                attackMovementsVectors = new[]
                {
                    new ChessVector(-1, 1),
                    new ChessVector(1, 1)
                };

                movementsVectors = new[]
                {
                    new ChessVector(0, 1),
                };

                // If we do first one move as black
                if (Position.Y == 1)
                {
                    movementsVectors = movementsVectors.Union(
                        new[]
                        {
                            new ChessVector(0, 2)
                        }).ToArray();
                }
            }

            return
                movementsVectors.Select(vector => ChessPoint.Add(Position, vector)).TakeWhile(point => Field[point] == ChessPiece.Empty)
                    .Where(CanMove)
                    .Union(
                        attackMovementsVectors.Select(vector => ChessPoint.Add(Position, vector))
                            .Where(CanMove)
                            .Where(point => CheckPrefix(Field[point], opponent)));
        }
    }
}