using Microsoft.VisualStudio.TestTools.UnitTesting;

using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.GameField;

namespace ChessTests.Field
{
    [TestClass]
    public class CheckMateTests
    {
        [TestMethod]
        public void Checkmate1()
        {
            var field = new VirtualField(VirtualFieldUtils.CreateEmptyField());
            var factory = new PieceMasterFactory();

            field[0, 0] = ChessPiece.WhiteKing;
            field[0, 7] = ChessPiece.BlackKing;

            field[7, 1] = ChessPiece.BlackRook;
            field[6, 0] = ChessPiece.BlackRook;

            Assert.IsTrue(CheckMateLogic.IsCheckMate(PlayerColor.Black, PlayerColor.White, field, factory));
        }

        [TestMethod]
        public void NoCheckmate1()
        {
            var field = new VirtualField(VirtualFieldUtils.CreateEmptyField());
            var factory = new PieceMasterFactory();

            field[0, 0] = ChessPiece.WhiteKing;
            field[0, 7] = ChessPiece.BlackKing;

            field[7, 2] = ChessPiece.BlackRook;
            field[6, 0] = ChessPiece.BlackRook;

            Assert.IsFalse(CheckMateLogic.IsCheckMate(PlayerColor.Black, PlayerColor.White, field, factory));
        }

        [TestMethod]
        public void Checkmate2()
        {
            var field = new VirtualField(VirtualFieldUtils.CreateEmptyField());
            var factory = new PieceMasterFactory();

            field[0, 0] = ChessPiece.WhiteKing;
            field[0, 7] = ChessPiece.BlackKing;

            field[1, 2] = ChessPiece.BlackRook;
            field[2, 1] = ChessPiece.BlackRook;
            field[2, 2] = ChessPiece.BlackQueen;

            Assert.IsTrue(CheckMateLogic.IsCheckMate(PlayerColor.Black, PlayerColor.White, field, factory));
        }

        [TestMethod]
        public void Checkmate3()
        {
            var field = new VirtualField(VirtualFieldUtils.CreateEmptyField());
            var factory = new PieceMasterFactory();

            field[0, 0] = ChessPiece.WhiteKing;
            field[1, 2] = ChessPiece.BlackKing;

            field[7, 0] = ChessPiece.BlackQueen;

            Assert.IsTrue(CheckMateLogic.IsCheckMate(PlayerColor.Black, PlayerColor.White, field, factory));
        }

        [TestMethod]
        public void NoCheckmate2()
        {
            var field = new VirtualField(VirtualFieldUtils.CreateEmptyField());
            var factory = new PieceMasterFactory();

            field[1, 1] = ChessPiece.WhiteKing;
            field[5, 6] = ChessPiece.BlackKing;

            field[7, 0] = ChessPiece.BlackRook;
            field[0, 7] = ChessPiece.BlackRook;

            // Eat for no checkmate
            field[2, 2] = ChessPiece.BlackQueen;

            Assert.IsFalse(CheckMateLogic.IsCheckMate(PlayerColor.Black, PlayerColor.White, field, factory));
        }

        [TestMethod]
        public void NoCheckmate3()
        {
            var field = new VirtualField(VirtualFieldUtils.CreateEmptyField());
            var factory = new PieceMasterFactory();

            field[7, 0] = ChessPiece.WhiteRook;
            field[6, 4] = ChessPiece.WhiteRook;
            field[1, 1] = ChessPiece.BlackKing;
            field[6, 7] = ChessPiece.WhiteKing;
            
            Assert.IsFalse(CheckMateLogic.IsCheckMate(PlayerColor.White, PlayerColor.Black, field, factory));
        }
    }
}