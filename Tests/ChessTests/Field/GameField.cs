using Microsoft.VisualStudio.TestTools.UnitTesting;

using NC.Shared.Contracts;
using NC.Shared.Data;
using NC.Shared.Exceptions;

namespace ChessTests.Field
{
    [TestClass]
    public class GameField
    {
        private const int FieldSize = 8;

        [TestMethod]
        public void CheckSize()
        {
            var field = new VirtualField();
            Assert.AreEqual(field.Width, FieldSize);
            Assert.AreEqual(field.Height, FieldSize);
        }

        [TestMethod]
        public void CheckRooks()
        {
            var field = new VirtualField();

            Assert.AreEqual(field[0, 0], ChessPiece.BlackRook);
            Assert.AreEqual(field[0, 7], ChessPiece.BlackRook);

            Assert.AreEqual(field[7, 7], ChessPiece.WhiteRook);
            Assert.AreEqual(field[7, 0], ChessPiece.WhiteRook);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMovementException))]
        public void GetIncorrectMovement()
        {
            var field = new VirtualField();
            var test = field[100, 100];
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMovementException))]
        public void SetIncorrectMovement()
        {
            var field = new VirtualField();
            field[-1, 8] = ChessPiece.BlackRook;
        }
    }
}
