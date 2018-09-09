using System;
using System.Collections.Generic;
using System.Linq;

using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Shared.GameField
{
    /// <summary>
    /// Check mate logic.
    /// </summary>
    public static class CheckMateLogic
    {
        /// <summary>
        /// Check for check mate.
        /// </summary>
        /// <param name="initiatorColor">Initiator color.</param>
        /// <param name="field">Game field.</param>
        /// <param name="masterFactory">Master factory implementation.</param>
        /// <param name="isCheck">Is check on table.</param>
        /// <returns>Checkmate flag.</returns>
        public static bool IsCheckMate(
            PlayerColor initiatorColor,
            VirtualField field,
            IPieceMasterFactory masterFactory,
            out bool isCheck)
        {
            isCheck = false;

            // Check for check now
            if (IsCheck(initiatorColor, field, masterFactory))
            {
                isCheck = true;
                var opponentColor = initiatorColor.Invert();

                // Otherwise do all possible movements, in the case of no possibility to prevent being attacked = initiator wins
                foreach (var opponmentPiecePoint in FindPieces(p => p.GetPlayerColor() == opponentColor, field))
                {
                    PieceMasterBase master;
                    if (masterFactory.TryGetMaster(field, opponmentPiecePoint, out master))
                    {
                        foreach (var movement in master.GetMovements())
                        {
                            var fieldCopy = new VirtualField(field.CloneMatrix());

                            if (fieldCopy[movement].GetPlayerColor() == fieldCopy[opponmentPiecePoint].GetPlayerColor())
                            {
                                // Step on free space
                                var temp = fieldCopy[opponmentPiecePoint];
                                fieldCopy[opponmentPiecePoint] = fieldCopy[movement];
                                fieldCopy[movement] = temp;
                            }
                            else
                            {
                                // Eat opponent piece
                                fieldCopy[movement] = fieldCopy[opponmentPiecePoint];
                                fieldCopy[opponmentPiecePoint] = ChessPiece.Empty;
                            }

                            if (!IsCheck(initiatorColor, fieldCopy, masterFactory))
                            {
                                return false;
                            }
                        }
                    }
                }

                // Passed all steps and no possibilities to prevent a check
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get all points under attack by initiator.
        /// </summary>
        /// <param name="initiatorColor">Initiator color.</param>
        /// <param name="field">Game field.</param>
        /// <param name="masterFactory">Masters factory.</param>
        /// <returns></returns>
        public static IEnumerable<ChessPoint> UnderAttackPoints(
            PlayerColor initiatorColor,
            VirtualField field,
            IPieceMasterFactory masterFactory)
        {
            var movements = new HashSet<ChessPoint>();
            foreach (var point in FindPieces(piece => piece.GetPlayerColor() == initiatorColor, field))
            {
                PieceMasterBase master;
                if (masterFactory.TryGetMaster(field, point, out master))
                {
                    foreach (var movement in master.GetRealMovements())
                    {
                        movements.Add(movement);
                    }
                }
            }

            return movements;
        }

        /// <summary>
        /// Map player color on king piece.
        /// </summary>
        /// <param name="color">Player color.</param>
        /// <returns>Player piece name.</returns>
        public static ChessPiece? MapKing(PlayerColor color)
        {
            var kingMapping = new Dictionary<PlayerColor, ChessPiece>()
            {
                { PlayerColor.Black, ChessPiece.BlackKing },
                { PlayerColor.White, ChessPiece.WhiteKing },
            };

            if (kingMapping.ContainsKey(color))
            {
                return kingMapping[color];
            }

            return null;
        }

        /// <summary>
        /// Is king in check status.
        /// </summary>
        /// <param name="initiatorColor">Initiator color.</param>
        /// <param name="field">Game field.</param>
        /// <param name="masterFactory">Master factory implementation.</param>
        /// <returns>Check flag.</returns>
        public static bool IsCheck(
            PlayerColor initiatorColor,
            VirtualField field,
            IPieceMasterFactory masterFactory)
        {
            var opponentColor = initiatorColor.Invert();
            ChessPoint opponentKingPoint = FindPieces(p => p == MapKing(opponentColor), field).FirstOrDefault();

            if (opponentKingPoint == null)
            {
                return false;
            }

            var initiatorAttacked = UnderAttackPoints(initiatorColor, field, masterFactory);
            return initiatorAttacked.Any(point => point == opponentKingPoint);
        }

        private static IEnumerable<ChessPoint> FindPieces(Func<ChessPiece, bool> checker, VirtualField field)
        {
            for (int x = 0; x < field.Width; x++)
            {
                for (int y = 0; y < field.Height; y++)
                {
                    if (checker(field[x, y]))
                    {
                        yield return new ChessPoint(x, y);
                    }
                }
            }
        }
    }
}