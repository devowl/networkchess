using System;
using System.Collections.Generic;

using NC.Shared.Data;
using NC.Shared.PieceMasters;

namespace NC.Shared.GameField
{
    /// <summary>
    /// Piece master factory.
    /// </summary>
    internal class PieceMasterFactory : IPieceMasterFactory
    {
        private readonly IDictionary<ChessPiece, Type> _objectsProvider = new Dictionary<ChessPiece, Type>
        {
            { ChessPiece.BlackBishop, typeof(BishopMaster) },
            { ChessPiece.BlackKnight, typeof(KnightMaster) },
            { ChessPiece.BlackKing, typeof(KingMaster) },
            { ChessPiece.BlackPawn, typeof(PawnMaster) },
            { ChessPiece.BlackQueen, typeof(QueenMaster) },
            { ChessPiece.BlackRook, typeof(RookMaster) },
            { ChessPiece.WhiteBishop, typeof(BishopMaster) },
            { ChessPiece.WhiteKnight, typeof(KnightMaster) },
            { ChessPiece.WhiteKing, typeof(KingMaster) },
            { ChessPiece.WhitePawn, typeof(PawnMaster) },
            { ChessPiece.WhiteQueen, typeof(QueenMaster) },
            { ChessPiece.WhiteRook, typeof(RookMaster) },
        };

        /// <inheritdoc/>
        public bool TryGetMaster(VirtualField field, ChessPoint point, out PieceMasterBase master)
        {
            master = null;
            if (0 <= point.X && point.X < field.Width && 0 <= point.Y && point.Y < field.Height)
            {
                var targetPlace = field[point];
                if (targetPlace == ChessPiece.Empty)
                {
                    return false;
                }

                master = (PieceMasterBase)Activator.CreateInstance(_objectsProvider[targetPlace], field, point);
                return true;
            }

            return false;
        }
    }
}