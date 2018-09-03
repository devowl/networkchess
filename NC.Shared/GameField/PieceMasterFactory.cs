using System;
using System.Collections.Generic;

using NC.Shared.Data;
using NC.Shared.PieceMasters;

namespace NC.Shared.GameField
{
    /// <summary>
    /// Piece master factory.
    /// </summary>
    public class PieceMasterFactory
    {
        private readonly VirtualField _field;

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

        /// <summary>
        /// Constructor for <see cref="PieceMasterFactory"/>.
        /// </summary>
        public PieceMasterFactory(VirtualField field)
        {
            _field = field;
        }

        /// <summary>
        /// Try get master for cell.
        /// </summary>
        /// <param name="x">Cell x coordinate.</param>
        /// <param name="y">Cell x coordinate.</param>
        /// <param name="master">Master reference.</param>
        /// <returns>Has master for cell.</returns>
        public bool TryGetMaster(int x, int y, out PieceMasterBase master)
        {
            master = null;
            if (0 <= x && x < _field.Width && 0 <= y && y < _field.Height)
            {
                var targetPlace = _field[x, y];
                if (targetPlace == ChessPiece.Empty)
                {
                    return false;
                }

                master = (PieceMasterBase)Activator.CreateInstance(_objectsProvider[targetPlace], _field, x, y);
                return true;
            }

            return false;
        }
    }
}