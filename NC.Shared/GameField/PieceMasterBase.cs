using System;
using System.Collections.Generic;
using System.Linq;

using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Shared.GameField
{
    /// <summary>
    /// Chess piece base class controller.
    /// </summary>
    public abstract class PieceMasterBase
    {
        private PlayerColor? _playerColor; 

        private IEnumerable<ChessPoint> _movements;

        /// <summary>
        /// Constructor for <see cref="PieceMasterBase"/>.
        /// </summary>
        protected PieceMasterBase(VirtualField field, ChessPoint point, params ChessPiece[] pieces)
        {
            Field = field;
            Pieces = pieces;
            Piece = Field[point];

            if (!pieces.Any())
            {
                throw new InvalidOperationException("Need more then one piece"); 
            }

            if (!pieces.Contains(Piece))
            {
                throw new InvalidOperationException($"x={point.X} y={point.Y} [{Piece}] isn't {string.Join(", ", pieces)}");
            }

            Position = point;
        }

        /// <summary>
        /// Virtual field.
        /// </summary>
        public VirtualField Field { get; }

        /// <summary>
        /// Piece types.
        /// </summary>
        public ChessPiece[] Pieces { get; set; }

        /// <summary>
        /// Current piece.
        /// </summary>
        public ChessPiece Piece { get; }

        /// <summary>
        /// Piece position.
        /// </summary>
        public ChessPoint Position { get; }

        /// <summary>
        /// Prefix name.
        /// </summary>
        protected PlayerColor SideName
            =>
                (PlayerColor)
                    (_playerColor ??
                     (_playerColor = CheckPrefix(Piece, PlayerColor.Black) ? PlayerColor.Black : PlayerColor.White));

        /// <summary>
        /// Get available movements for piece.
        /// </summary>
        /// <returns>Available points.</returns>
        public IEnumerable<ChessPoint> GetMovements()
        {
            return _movements ?? (_movements = GetAvailableMovements() ?? Enumerable.Empty<ChessPoint>());
        }

        protected static bool CheckPrefix(ChessPiece piece, PlayerColor playerColor)
        {
            return piece.ToString().StartsWith(playerColor.ToString());
        }

        /// <summary>
        /// Get available movements for piece.
        /// </summary>
        /// <returns>Available points.</returns>
        protected abstract IEnumerable<ChessPoint> GetAvailableMovements();

        /// <summary>
        /// Get vector path points.
        /// </summary>
        /// <param name="vector">Vector value.</param>
        /// <returns>Points on the path.</returns>
        protected IEnumerable<ChessPoint> GetVectorPathPoints(ChessVector vector)
        {
            var currentPos = Position;
            bool opponent = false;
            while (CanMove(currentPos = ChessPoint.Add(currentPos, vector)) && !opponent)
            {
                yield return currentPos;

                var targetPlace = Field[currentPos.X, currentPos.Y];
                var targetSideName = targetPlace.GetPlayerColor();
                opponent = targetSideName.HasValue && SideName != targetSideName;
            }
        }

        /// <summary>
        /// Get move status.
        /// </summary>
        /// <param name="position">Check position.</param>
        /// <returns>Is not outside board.</returns> 
        protected bool CanMove(ChessPoint position)
        {
            int x = position.X;
            int y = position.Y;

            if (0 <= x && x < Field.Width && 0 <= y && y < Field.Height)
            {
                var targetPlace = Field[x, y];
                if (targetPlace == ChessPiece.Empty)
                {
                    return true;
                }

                var sideName = targetPlace.GetPlayerColor();
                return SideName != sideName;
            }

            return false;
        }
    }
}