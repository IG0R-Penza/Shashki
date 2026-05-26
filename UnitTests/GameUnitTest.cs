using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shashki;

namespace UnitTests
{
    [TestClass]
    public class GameUnitTest
    {
        /// <summary>
        /// нельзя походить шашкой не первой линии в начале
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Game game = new Game(Color.White);
            Assert.IsFalse(game.isAbleToMove(1, 6));
        }

        /// <summary>
        /// нельзя походить чужой шашкой или пустой клеткой
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            Game game = new Game(Color.White);
            Assert.IsFalse(game.isAbleToMove(1, 2) || game.isAbleToMove(1, 3));
        }

        /// <summary>
        /// можно походить вперёд, но не назад
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            Game game = new Game(Color.White);
            Assert.IsTrue(game.Move(0, 5, 1, 4) && !game.Move(1, 4, 0, 5));
        }

        /// <summary>
        /// Нельзя походить дальше, чем на одну клетку
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            Game game = new Game(Color.White);
            game.Board[3, 2].Type = Shashki.Type.None;
            game.Board[3, 2].Color = Shashki.Color.None;
            game.updateStatus();
            Assert.IsFalse(game.Move(0, 5, 2, 3));
        }

        /// <summary>
        /// Дамка может походить дальше, чем на одну клетку
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            Game game = new Game(Color.White);
            game.Board[5, 0].Type = Shashki.Type.King;
            game.Board[3, 2].Type = Shashki.Type.None;
            game.Board[3, 2].Color = Shashki.Color.None;
            game.updateStatus();
            Assert.IsTrue(game.Move(0, 5, 2, 3));
        }

        /// <summary>
        /// Нельзя взять свою шашку
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            Game game = new Game(Color.White);
            game.Board[4, 1].Type = Shashki.Type.Man;
            game.Board[4, 1].Color = Shashki.Color.White;
            game.updateStatus();
            Assert.IsFalse(game.Take(0, 5, 2, 3));
        }

        /// <summary>
        /// Можно взять чужую шашку
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            Game game = new Game(Color.White);
            game.Board[4, 1].Type = Shashki.Type.Man;
            game.Board[4, 1].Color = Shashki.Color.Black;
            game.updateStatus();
            Assert.IsTrue(game.Take(0, 5, 2, 3));
        }

        /// <summary>
        /// Нельзя ходить, если можно взять чужую шашку
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            Game game = new Game(Color.White);
            game.Board[4, 1].Type = Shashki.Type.Man;
            game.Board[4, 1].Color = Shashki.Color.Black;
            game.updateStatus();
            Assert.IsFalse(game.Move(2, 5, 3, 4));
        }

        /// <summary>
        /// Можно взять чужую шашку сзади
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            Game game = new Game(Color.White);
            game.Board[4, 1].Type = Shashki.Type.Man;
            game.Board[4, 1].Color = Shashki.Color.Black;
            game.Board[3, 2].Type = Shashki.Type.Man;
            game.Board[3, 2].Color = Shashki.Color.White;
            game.Board[5, 0].Type = Shashki.Type.None;
            game.Board[5, 0].Color = Shashki.Color.None;
            game.updateStatus();
            Assert.IsTrue(game.Take(2, 3, 0, 5));
        }

        /// <summary>
        /// Нельзя ходить в свою клетку
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            Game game = new Game(Color.White);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    game.Board[i, j].Color = Color.None;
                    game.Board[i, j].Type = Type.None;
                }
            }
            game.Board[0, 5].Color = Color.White;
            game.Board[0, 5].Type = Type.King;
            game.updateStatus();
            Assert.IsFalse(game.isAbleToMoveTo(5, 0, 5, 0));
        }
    }
}
