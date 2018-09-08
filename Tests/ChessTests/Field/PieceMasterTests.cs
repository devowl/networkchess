using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NC.Shared.Data;
using NC.Shared.GameField;

namespace ChessTests.Field
{
    [TestClass]
    public class PieceMasterTests
    {
        [TestMethod]
        public void BlackBishopMovementTest()
        {
            var field = CreateBishopRookField();

            var factory = new PieceMasterFactory();
            PieceMasterBase bishopMaster;

            bool hasBishopMaster = factory.TryGetMaster(field, new ChessPoint(0, 0), out bishopMaster);

            Assert.AreEqual(hasBishopMaster, true);

            var bishopMovements = bishopMaster.GetMovements();

            Assert.AreEqual(bishopMovements.Count(), 3);

            bishopMovements = OrderPoints(bishopMovements);
            var realBishopMovements = OrderPoints(
                new[]
                {
                    new ChessPoint(1, 1),
                    new ChessPoint(2, 2),
                    new ChessPoint(3, 3),
                });

            Assert.IsTrue(bishopMovements.SequenceEqual(realBishopMovements));
        }

        [TestMethod]
        public void WhiteRookMovementTest()
        {
            var field = CreateBishopRookField();

            var factory = new PieceMasterFactory();
            PieceMasterBase rookMaster;

            bool hasRookMaster = factory.TryGetMaster(field, new ChessPoint(3, 3), out rookMaster);

            Assert.AreEqual(hasRookMaster, true);

            var rookMovements = rookMaster.GetMovements();

            Assert.AreEqual(rookMovements.Count(), 14);

            rookMovements = OrderPoints(rookMovements);
            var realRookMovements = OrderPoints(
                new[]
                {
                    new ChessPoint(3, 0),
                    new ChessPoint(3, 1),
                    new ChessPoint(3, 2),
                    new ChessPoint(3, 4),
                    new ChessPoint(3, 5),
                    new ChessPoint(3, 6),
                    new ChessPoint(3, 7),
                    new ChessPoint(0, 3),
                    new ChessPoint(1, 3),
                    new ChessPoint(2, 3),
                    new ChessPoint(4, 3),
                    new ChessPoint(5, 3),
                    new ChessPoint(6, 3),
                    new ChessPoint(7, 3),
                });

            Assert.IsTrue(realRookMovements.SequenceEqual(rookMovements));
        }

        [TestMethod]
        public void KnightMovementTest()
        {
            var field = CreateKnightPawnField();

            var factory = new PieceMasterFactory();
            PieceMasterBase knightMaster;

            Assert.AreEqual(factory.TryGetMaster(field, new ChessPoint(0, 1), out knightMaster), true);

            var knightMovements = knightMaster.GetMovements();

            Assert.AreEqual(knightMovements.Count(), 3);

            knightMovements = OrderPoints(knightMovements);
            var realknightMovements = OrderPoints(
                new[]
                {
                    new ChessPoint(2, 0),
                    new ChessPoint(2, 2),
                    new ChessPoint(1, 3),
                });

            Assert.IsTrue(realknightMovements.SequenceEqual(knightMovements));
        }

        [TestMethod]
        public void PawnMovementTest()
        {
            var field = CreateKnightPawnField();

            var factory = new PieceMasterFactory();
            PieceMasterBase pawnMaster;

            Assert.AreEqual(factory.TryGetMaster(field, new ChessPoint(2, 2), out pawnMaster), true);

            var pawnMovements = pawnMaster.GetMovements();

            Assert.AreEqual(pawnMovements.Count(), 1);

            pawnMovements = OrderPoints(pawnMovements);
            var realPawnMovements = OrderPoints(
                new[]
                {
                    new ChessPoint(2, 1),
                });

            Assert.IsTrue(realPawnMovements.SequenceEqual(pawnMovements));
        }

        [TestMethod]
        public void QueenMovementTest()
        {
            var field = CreateQueenKingField();

            var factory = new PieceMasterFactory();
            PieceMasterBase queenMaster;

            Assert.AreEqual(factory.TryGetMaster(field, new ChessPoint(1, 1), out queenMaster), true);

            var queenMovements = queenMaster.GetMovements();

            Assert.AreEqual(queenMovements.Count(), 10);
        }

        [TestMethod]
        public void KingMovementTest()
        {
            var field = CreateQueenKingField();

            var factory = new PieceMasterFactory();
            PieceMasterBase kingMaster;

            Assert.AreEqual(factory.TryGetMaster(field, new ChessPoint(4, 2), out kingMaster), true);

            var kingMovements = kingMaster.GetMovements();

            Assert.AreEqual(kingMovements.Count(), 6);
        }

        [TestMethod]
        public void KingMovementTest2()
        {
            // King cant move on attacked point
            var defaultField = VirtualFieldUtils.CreateEmptyField();
            var field = new VirtualField(defaultField);

            field[1, 7] = ChessPiece.BlackRook;
            field[0,0] = ChessPiece.WhiteKing;

            var factory = new PieceMasterFactory();
            PieceMasterBase kingMaster;

            Assert.AreEqual(factory.TryGetMaster(field, new ChessPoint(0, 0), out kingMaster), true);

            var kingMovements = kingMaster.GetMovements();

            Assert.AreEqual(kingMovements.Count(), 1);
        }

        [TestMethod]
        public void KnigtMovementTest2()
        {
            // King cant move on attacked point
            var defaultField = VirtualFieldUtils.CreateEmptyField();
            var field = new VirtualField(defaultField);

            field[4,4] = ChessPiece.BlackKnight;

            var factory = new PieceMasterFactory();
            PieceMasterBase knigtMaster;

            Assert.AreEqual(factory.TryGetMaster(field, new ChessPoint(4, 4), out knigtMaster), true);

            var knigtMovements = knigtMaster.GetMovements();

            Assert.AreEqual(knigtMovements.Count(), 8);
        }

        [TestMethod]
        public void KingMovementTest3()
        {
            // King cant move on attacked point
            var defaultField = VirtualFieldUtils.CreateEmptyField();
            var field = new VirtualField(defaultField);

            field[4, 1] = ChessPiece.BlackKing;
            field[4, 3] = ChessPiece.WhiteKing;

            var factory = new PieceMasterFactory();
            PieceMasterBase master;

            factory.TryGetMaster(field, new ChessPoint(4, 1), out master);
            Assert.AreEqual(master.GetMovements().Count(), 5);

            factory.TryGetMaster(field, new ChessPoint(4, 3), out master);
            Assert.AreEqual(master.GetMovements().Count(), 5);
        }

        private VirtualField CreateQueenKingField()
        {
            var defaultField = VirtualFieldUtils.CreateEmptyField();
            var field = new VirtualField(defaultField);

            /***************
             * _ _ _ _ P
             * _ Q _ _ _ P
             * _ _ _ _ K
             * P P _ P
             * 
            */
            field[4, 2] = ChessPiece.WhiteKing;
            field[1, 1] = ChessPiece.WhiteQueen;

            field[0, 3] = ChessPiece.WhitePawn;
            field[1, 3] = ChessPiece.WhitePawn;
            field[3, 3] = ChessPiece.WhitePawn;
            field[4, 0] = ChessPiece.WhitePawn;
            field[5, 1] = ChessPiece.WhitePawn;
            return field;
        }

        private VirtualField CreateKnightPawnField()
        {
            var defaultField = VirtualFieldUtils.CreateEmptyField();
            var field = new VirtualField(defaultField);

            /***************
             * _ _ _ _
             * ? _ _ _
             * _ _ P _
             * _ _ _ _
             * 
            */
            field[0, 1] = ChessPiece.BlackKnight;
            field[2, 2] = ChessPiece.WhitePawn;

            return field;
        }
        
        private VirtualField CreateBishopRookField()
        {
            var defaultField = VirtualFieldUtils.CreateEmptyField();
            var field = new VirtualField(defaultField);

            /***************
             * B _ _ _
             * _ * _ _
             * _ _ * _
             * _ _ _ R
             * 
            */
            field[0, 0] = ChessPiece.BlackBishop;
            field[3, 3] = ChessPiece.WhiteRook;

            return field;
        }

        private IEnumerable<ChessPoint> OrderPoints(IEnumerable<ChessPoint> points)
        {
            return points.OrderBy(p => p.Y).ThenBy(p => p.X);
        }
    }
}