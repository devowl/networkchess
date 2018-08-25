﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;

using NC.Shared.Contracts;
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
        public KingMaster(VirtualField field, int x, int y) 
            : base(field, x, y, ChessPiece.BlackKing, ChessPiece.WhiteKing)
        {
        }

        /// <inheritdoc/>
        protected override IEnumerable<Point> GetAvailableMovements()
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
                new Vector(-1, -1),
                new Vector(0, -1),
                new Vector(1, -1),
                new Vector(-1, 0),
                new Vector(1, 0),
                new Vector(-1, 1),
                new Vector(0, 1),
                new Vector(1, 1),
            };

            return movements.Select(vector => Point.Add(Position, vector)).Where(CanMove);
        }
    }
}