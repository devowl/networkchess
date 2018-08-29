﻿using System.Runtime.Serialization;

namespace NC.Shared.Data
{
    /// <summary>
    /// Chess piece types.
    /// </summary>
    [DataContract]
    public enum ChessPiece
    {
        Empty,

        BlackKing,

        BlackQueen,

        BlackRook,

        BlackBishop,

        BlackKnight,

        BlackPawn,

        WhiteKing,

        WhiteQueen,

        WhiteRook,

        WhiteBishop,

        WhiteKnight,

        WhitePawn
    }
}