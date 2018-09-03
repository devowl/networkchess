using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Shared.GameField
{
    /// <summary>
    /// Chess piece base class controller.
    /// </summary>
    public abstract class PieceMasterBase
    {
        private PlayerColor? _sideName;

        /// <summary>
        /// Constructor for <see cref="PieceMasterBase"/>.
        /// </summary>
        protected PieceMasterBase(VirtualField field, int x, int y, params ChessPiece[] pieces)
        {
            Field = field;
            Piece = Field[x, y];

            if (!pieces.Any())
            {
                throw new InvalidOperationException("Need more then one piece");
            }

            if (!pieces.Contains(Piece))
            {
                throw new InvalidOperationException($"x={x} y={y} [{Piece}] isn't {string.Join(", ", pieces)}");
            }

            Position = new ChessPoint(x, y);
        }

        /// <summary>
        /// Virtual field.
        /// </summary>
        public VirtualField Field { get; }

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
            => (PlayerColor)(_sideName ?? (_sideName = CheckPrefix(Piece, PlayerColor.Black) ? PlayerColor.Black : PlayerColor.White));

        private IEnumerable<ChessPoint> _movements;

        /// <summary>
        /// Get available movements for piece.
        /// </summary>
        /// <returns>Available points.</returns>
        public IEnumerable<ChessPoint> GetMovements()
        {
            return _movements ?? (_movements = GetAvailableMovements() ?? Enumerable.Empty<ChessPoint>());
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
                var targetSideName = VirtualFieldUtils.GetSideName(targetPlace);
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

                var sideName = VirtualFieldUtils.GetSideName(targetPlace);
                return SideName != sideName;
            }

            return false;
        }

        protected static bool CheckPrefix(ChessPiece piece, PlayerColor playerColor)
        {
            return piece.ToString().StartsWith(playerColor.ToString());
        }

        
    }
}