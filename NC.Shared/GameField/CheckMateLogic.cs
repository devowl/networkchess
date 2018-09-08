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
        /// <param name="opponentColor">Opponent color.</param>
        /// <param name="field">Game field.</param>
        /// <param name="masterFactory">Master factory implementation.</param>
        /// <returns></returns>
        public static bool IsCheckMate(
            PlayerColor initiatorColor,
            PlayerColor opponentColor,
            VirtualField field,
            IPieceMasterFactory masterFactory)
        {
            // Check for check now
            if (IsCheck(initiatorColor, opponentColor, field, masterFactory))
            {
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

                            if (!IsCheck(initiatorColor, opponentColor, fieldCopy, masterFactory))
                            {
                                return false;
                            }
                        }

                        // Passed all steps and no possibilities to prevent a check
                        return true;
                    }
                }
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

        private static bool IsCheck(
            PlayerColor initiatorColor,
            PlayerColor opponentColor,
            VirtualField field,
            IPieceMasterFactory masterFactory)
        {
            var kingMapping = new Dictionary<PlayerColor, ChessPiece>()
            {
                { PlayerColor.Black, ChessPiece.BlackKing },
                { PlayerColor.White, ChessPiece.WhiteKing },
            };

            ChessPoint opponentKingPoint = FindPieces(p => p == kingMapping[opponentColor], field).FirstOrDefault();

            if (opponentKingPoint == null)
            {
                return false;
            }

            var initiatorAttacked = UnderAttackPoints(initiatorColor, field, masterFactory);
            var @return = initiatorAttacked.Any(point => point == opponentKingPoint);

            return @return;
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