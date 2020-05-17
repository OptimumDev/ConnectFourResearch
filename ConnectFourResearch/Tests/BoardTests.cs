using ConnectFourResearch.ConnectFour;
using FluentAssertions;
using NUnit.Framework;

namespace ConnectFourResearch.Tests
{
    [TestFixture]
    public class BoardTests
    {
        private readonly Board board = new Board();

        [Test]
        public void NotFinishedAtStart()
        {
            board.IsFinished().Should().BeFalse();
        }

        [Test]
        public void CanWinWithVertical([Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            for (var i = 0; i < 4; i++)
                game = game.Move(0, player);

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanWinWithHorizontal([Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            for (var i = 0; i < 4; i++)
                game = game.Move(i, player);

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanWinWithMainDiagonal([Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < i; j++)
                    game = game.Move(i, player.GetOpponent());
                game = game.Move(i, player);
            }

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanWinWithAntiDiagonal([Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < i; j++)
                    game = game.Move(4 - i, player.GetOpponent());
                game = game.Move(4 - i, player);
            }

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanFinishWithDraw()
        {
            var game = board;

            for (var i = 0; i < 10; i++)
            {
                var yColumn = i * 2 % 7;
                var rColumn = (i * 2 + 1) % 7;
                for (var j = 0; j < 2; j++)
                {
                    game = game.Move(yColumn, Cell.Yellow);
                    game = game.Move(rColumn, Cell.Red);
                }
            }
            game = game.Move(6, Cell.Yellow);
            game = game.Move(6, Cell.Red);

            game.IsFinished().Should().BeTrue();
            game.GetLinesCountOfLength(4, Cell.Yellow).Should().Be(0);
            game.GetLinesCountOfLength(4, Cell.Red).Should().Be(0);
        }

        private static void AssertPlayerWon(Cell player, Board game)
        {
            game.IsFinished().Should().BeTrue();
            game.GetLinesCountOfLength(4, player).Should().BeGreaterThan(0);
        }
    }
}