using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

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
        public PawnMaster(VirtualField field, int x, int y)
            : base(field, x, y, ChessPiece.BlackPawn, ChessPiece.WhitePawn)
        {
        }

        /// <inheritdoc/>
        protected override IEnumerable<Point> GetAvailableMovements()
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

            Vector[] movementsVectors;
            Vector[] attackMovementsVectors;
            string enemy;

            // 1. Ходит тока в перёд
            // 2. В бок может тока есть
            // 3. Если на исходной позиции тогда может прыгнуть на 2
            if (SideName == WhiteName)
            {
                enemy = BlackName;
                attackMovementsVectors = new[]
                {
                    new Vector(-1, -1),
                    new Vector(1, -1)
                };

                movementsVectors = new[]
                {
                    new Vector(0, -1)
                };

                // If we do first one move as white
                if (Position.Y == 6)
                {
                    movementsVectors = movementsVectors.Union(
                        new[]
                        {
                            new Vector(0, -2)
                        }).ToArray();
                }
            }
            else
            {
                enemy = WhiteName;
                attackMovementsVectors = new[]
                {
                    new Vector(-1, 1),
                    new Vector(1, 1)
                };

                movementsVectors = new[]
                {
                    new Vector(0, 1),
                };

                // If we do first one move as black
                if (Position.Y == 1)
                {
                    movementsVectors = movementsVectors.Union(
                        new[]
                        {
                            new Vector(0, 2)
                        }).ToArray();
                }
            }

            return
                movementsVectors.Select(vector => Point.Add(Position, vector))
                    .Where(CanMove)
                    .Union(
                        attackMovementsVectors.Select(vector => Point.Add(Position, vector))
                            .Where(CanMove)
                            .Where(point => CheckPrefix(Field[(int)point.Y, (int)point.Y], enemy)));
        }
    }
}