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

        private object _syncObject = new object();

        /// <summary>
        /// Constructor for <see cref="PieceMasterBase"/>.
        /// </summary>
        protected PieceMasterBase(VirtualField field, ChessPoint point, IPieceMasterFactory master, params ChessPiece[] pieces)
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
            Master = master;
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
        /// Master factory instance.
        /// </summary>
        protected IPieceMasterFactory Master { get; set; }

        /// <summary>
        /// Prefix name.
        /// </summary>
        protected PlayerColor SideName
            =>
                (PlayerColor)
                    (_playerColor ??
                     (_playerColor = CheckPrefix(Piece, PlayerColor.Black) ? PlayerColor.Black : PlayerColor.White));

        /// <summary>
        /// Ignore enemy king piece.
        /// </summary>
        protected bool IgnoreEnemyKingFlag { get; set; }

        /// <summary>
        /// Get available movements for piece.
        /// </summary>
        /// <returns>Available points.</returns>
        public IEnumerable<ChessPoint> GetMovements()
        {
            lock (_syncObject)
            {
                return _movements ?? (_movements = GetAvailableMovements() ?? Enumerable.Empty<ChessPoint>());
            }
        }

        /// <summary>
        /// Get only real movements, ignore under-attack points.
        /// </summary>
        /// <returns>Available points.</returns>
        internal IEnumerable<ChessPoint> GetRealMovements()
        {
            lock (_syncObject)
            {
                IgnoreEnemyKingFlag = true;
                var movements = GetAvailableMovements(true).ToArray();
                IgnoreEnemyKingFlag = false;
                return movements;
            }
        }

        protected static bool CheckPrefix(ChessPiece piece, PlayerColor playerColor)
        {
            return piece.ToString().StartsWith(playerColor.ToString());
        }

        /// <summary>
        /// Get available movements for piece.
        /// </summary>
        /// <param name="onlySteps">Только доступные перемещения фигур.</param>
        /// <returns>Available points.</returns>
        protected abstract IEnumerable<ChessPoint> GetAvailableMovements(bool onlySteps = false);

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
                opponent = targetSideName.HasValue && SideName != targetSideName && !IsIgnoreEnemyKing(targetPlace);
            }
        }

        /// <summary>
        /// Get move status.
        /// </summary>
        /// <param name="newPosition">Check position.</param>
        /// <returns>Is not outside board.</returns> 
        protected bool CanMove(ChessPoint newPosition)
        {
            int x = newPosition.X;
            int y = newPosition.Y;

            if (0 <= x && x < Field.Width && 0 <= y && y < Field.Height)
            {
                var targetPlace = Field[x, y];
                var sideName = targetPlace.GetPlayerColor();

                var result = false;
                if (targetPlace == ChessPiece.Empty)
                {
                    result = true;
                }
                else
                {
                    result = SideName != sideName;
                }

                bool isYourKingIsUnderAttack = Master.CheckedPlayer.HasValue &&
                                               Master.CheckedPlayer == Piece.GetPlayerColor();

                if (result && isYourKingIsUnderAttack)
                {
                    // If king is under attack, you next step must prevent a check state
                    var fieldCopy = new VirtualField(Field.CloneMatrix());
                    fieldCopy[newPosition] = fieldCopy[Position];
                    fieldCopy[Position] = ChessPiece.Empty;

                    var pieceColor = Piece.GetPlayerColor().Value.Invert();
                    return !CheckMateLogic.IsCheck(pieceColor, fieldCopy, Master);
                }

                return result;
            }

            return false;
        }

        private bool IsIgnoreEnemyKing(ChessPiece targetPlace)
        {
            if (!IgnoreEnemyKingFlag)
            {
                return false;
            }

            var targetColor = targetPlace.GetPlayerColor();
            if (targetColor == null)
            {
                return false;
            }
            var targetColorValue = targetColor.Value;

            var isEnemyPiece = Piece.GetPlayerColor()?.Invert() == targetPlace.GetPlayerColor();
            var isKingPiece = CheckMateLogic.MapKing(targetColorValue) == targetPlace;

            return isEnemyPiece && isKingPiece;
        }
    }
}