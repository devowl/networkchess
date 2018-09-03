using System.Linq;

using NC.Shared.Contracts;

namespace NC.Shared.Data
{
    /// <summary>
    /// <see cref="VirtualField"/> chess utilities.
    /// </summary>
    public static class VirtualFieldUtils
    {
        private const int FieldSize = 8;

        /// <summary>
        /// Whites chess pieces.
        /// </summary>
        private static readonly ChessPiece[] Whites = {
            ChessPiece.WhiteQueen,
            ChessPiece.WhiteBishop,
            ChessPiece.WhiteKing,
            ChessPiece.WhitePawn,
            ChessPiece.WhiteKnight,
            ChessPiece.WhiteRook,
        };

        /// <summary>
        /// Blacks chess pieces.
        /// </summary>
        private static readonly ChessPiece[] Blacks = {
            ChessPiece.BlackQueen,
            ChessPiece.BlackBishop,
            ChessPiece.BlackKing,
            ChessPiece.BlackPawn,
            ChessPiece.BlackKnight,
            ChessPiece.BlackRook,
        };

        /// <summary>
        /// Create default game field.
        /// </summary>
        /// <returns></returns>
        public static ChessPiece[,] CreateDefaultField()
        {
            var field = new[,]
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

            // Rotate on 90 (because of beautiful input above ^ )
            var newField = new ChessPiece[field.GetLength(0), field.GetLength(1)];
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = x; y < field.GetLength(1); y++)
                {
                    newField[x, y] = field[y, x];
                    newField[y, x] = field[x, y];
                }
            }

            return newField;
        }

        /// <summary>
        /// Create empty game field.
        /// </summary>
        /// <returns>Game field array.</returns>
        public static ChessPiece[,] CreateEmptyField()
        {
            return new ChessPiece[FieldSize, FieldSize];
        }

        /// <summary>
        /// Get chess piece side.
        /// </summary>
        /// <param name="piece">Chess piece.</param>
        /// <returns>Chess color name.</returns>
        public static PlayerColor? GetPlayerColor(this ChessPiece piece)
        {
            if (Whites.Contains(piece))
            {
                return PlayerColor.White;
            }

            if (Blacks.Contains(piece))
            {
                return PlayerColor.Black;
            }

            return null;
        }
    }
}