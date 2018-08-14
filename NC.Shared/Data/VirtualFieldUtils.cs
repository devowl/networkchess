using NC.Shared.Contracts;

namespace NC.Shared.Data
{
    /// <summary>
    /// <see cref="VirtualField"/> utilities.
    /// </summary>
    public class VirtualFieldUtils
    {
        private const int FieldSize = 8;

        /// <summary>
        /// Create default game field.
        /// </summary>
        /// <returns></returns>
        public static ChessPiece[,] CreateDefaultField()
        {
            return new[,]
            {
                {
                    ChessPiece.BlackRook,
                    ChessPiece.BlackKnight,
                    ChessPiece.BlackBishop,
                    ChessPiece.BlackQueen,
                    ChessPiece.BlackKing,
                    ChessPiece.BlackBishop,
                    ChessPiece.BlackKnight,
                    ChessPiece.BlackRook
                },
                {
                    ChessPiece.BlackPawn,
                    ChessPiece.BlackPawn,
                    ChessPiece.BlackPawn,
                    ChessPiece.BlackPawn,
                    ChessPiece.BlackPawn,
                    ChessPiece.BlackPawn,
                    ChessPiece.BlackPawn,
                    ChessPiece.BlackPawn
                },
                {
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty
                },
                {
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty
                },
                {
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty
                },
                {
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty,
                    ChessPiece.Empty
                },
                {
                    ChessPiece.WhitePawn,
                    ChessPiece.WhitePawn,
                    ChessPiece.WhitePawn,
                    ChessPiece.WhitePawn,
                    ChessPiece.WhitePawn,
                    ChessPiece.WhitePawn,
                    ChessPiece.WhitePawn,
                    ChessPiece.WhitePawn
                },
                {
                    ChessPiece.WhiteRook,
                    ChessPiece.WhiteKnight,
                    ChessPiece.WhiteBishop,
                    ChessPiece.WhiteQueen,
                    ChessPiece.WhiteKing,
                    ChessPiece.WhiteBishop,
                    ChessPiece.WhiteKnight,
                    ChessPiece.WhiteRook
                },
            };
        }

        /// <summary>
        /// Create empty game field.
        /// </summary>
        /// <returns>Game field array.</returns>
        public static ChessPiece[,] CreateEmptyField()
        {
            return new ChessPiece[FieldSize, FieldSize];
        }
    }
}