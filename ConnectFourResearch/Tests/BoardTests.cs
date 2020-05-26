using System.Linq;
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
        public void GetCell_ReturnsAllEmpty_WhenEmptyBoard()
        {
            for (var x = 0; x < Board.Width; x++)
            {
                for (var y = 0; y < Board.Height; y++)
                    board.GetCell(x, y).Should().Be(Cell.Empty);
            }
        }

        [Test]
        public void GetCell_ReturnsRightPlayer_WhenHasDisks(
            [Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            game = game.Move(0, player);

            game.GetCell(0, 0).Should().Be(player);
        }

        [Test]
        public void GetCell_ReturnsAllPlayersDisks_WhenHasMoreThanOneDisk(
            [Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            game = game.Move(0, player);
            game = game.Move(3, player);
            game = game.Move(3, player);
            game = game.Move(5, player);
            game = game.Move(1, player);

            game.GetCell(0, 0).Should().Be(player);
            game.GetCell(3, 0).Should().Be(player);
            game.GetCell(3, 1).Should().Be(player);
            game.GetCell(5, 0).Should().Be(player);
            game.GetCell(1, 0).Should().Be(player);
        }

        [Test]
        public void Move_PlacesDisk_InRightColumn(
            [Values(Cell.Yellow, Cell.Red)] Cell player,
            [Range(0, Board.Width - 1)] int column)
        {
            var game = board;

            game = game.Move(column, player);

            game.GetCell(column, 0).Should().Be(player);
        }

        [Test]
        public void Move_PlacesDisk_InFirstEmptyRow(
            [Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            game = game.Move(0, player);
            game = game.Move(3, player.GetOpponent());
            game = game.Move(3, player);

            game.GetCell(0, 0).Should().Be(player);
            game.GetCell(3, 1).Should().Be(player);
        }

        [Test]
        public void IsFinished_ReturnsFalse_AtStart()
        {
            board.IsFinished().Should().BeFalse();
        }

        [Test]
        public void GetLinesCount_ReturnsColumns(
            [Values(Cell.Yellow, Cell.Red)] Cell player,
            [Range(2, 4)] int length)
        {
            var game = board;

            game = BuildColumn(player, game, length);

            game.GetLinesCountOfLength(length, player).Should().Be(1);
        }

        [Test]
        public void GetLinesCount_ReturnsRows(
            [Values(Cell.Yellow, Cell.Red)] Cell player,
            [Range(2, 4)] int length)
        {
            var game = board;

            game = BuildRow(player, game, length);

            game.GetLinesCountOfLength(length, player).Should().Be(1);
        }

        [Test]
        public void GetLinesCount_ReturnsMainDiagonal(
            [Values(Cell.Yellow, Cell.Red)] Cell player,
            [Range(2, 4)] int length)
        {
            var game = board;

            game = BuildMainDiagonal(player, game, length);

            game.GetLinesCountOfLength(length, player).Should().Be(1);
        }

        [Test]
        public void GetLinesCount_ReturnsAntiDiagonal(
            [Values(Cell.Yellow, Cell.Red)] Cell player,
            [Range(2, 4)] int length)
        {
            var game = board;

            game = BuildAntiDiagonal(player, game, length);

            game.GetLinesCountOfLength(length, player).Should().Be(1);
        }

        [Test]
        public void GetLinesCount_ReturnsAllLines_WhenNoIntersection(
            [Values(Cell.Yellow, Cell.Red)] Cell player,
            [Range(2, 4)] int length)
        {
            var game = board;

            for (var i = 0; i < length; i++)
            {
                game = game.Move(0, player);
                game = game.Move(3, player);
            }

            game.GetLinesCountOfLength(length, player).Should().Be(2);
        }

        [Test]
        public void GetLinesCount_ReturnsAllLines_WhenIntersect(
            [Values(Cell.Yellow, Cell.Red)] Cell player,
            [Range(2, 4)] int length)
        {
            var game = board;

            game = game.Move(0, player);

            for (var i = 0; i < length - 1; i++)
            {
                game = game.Move(0, player);
                game = game.Move(i + 1, player);
            }

            game.GetLinesCountOfLength(length, player).Should().Be(length == 2 ? 3 : 2);
        }

        [Test]
        public void CanWinWithVertical(
            [Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            game = BuildColumn(player, game, 4);

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanWinWithHorizontal(
            [Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            game = BuildRow(player, game, 4);

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanWinWithMainDiagonal(
            [Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            game = BuildMainDiagonal(player, game, 4);

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanWinWithAntiDiagonal(
            [Values(Cell.Yellow, Cell.Red)] Cell player)
        {
            var game = board;

            game = BuildAntiDiagonal(player, game, 4);

            AssertPlayerWon(player, game);
        }

        [Test]
        public void CanFinishWithDraw()
        {
            var game = board;

            game = FillBoard(game);

            game.IsFinished().Should().BeTrue();
            game.GetLinesCountOfLength(4, Cell.Yellow).Should().Be(0);
            game.GetLinesCountOfLength(4, Cell.Red).Should().Be(0);
        }

        [Test]
        public void GetPossibleMoves_ReturnsAllPossibleColumns_WhenEmpty()
        {
            var expected = Enumerable.Range(0, Board.Width);
            board.GetPossibleMoves().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetPossibleMoves_ReturnsAllPossibleColumns_WhenHasDisks()
        {
            var game = board;

            game = game.Move(0, Cell.Yellow);
            game = game.Move(4, Cell.Red);

            var expected = Enumerable.Range(0, Board.Width);
            game.GetPossibleMoves().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetPossibleMoves_ReturnsOnlyPossibleColumns_WhenHasOneFullColumn(
            [Range(0, Board.Width - 1)] int column)
        {
            var game = board;

            game = FillColumn(column, game);

            var expected = Enumerable
                .Range(0, Board.Width)
                .Where(c => c != column);
            game.GetPossibleMoves().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetPossibleMoves_ReturnsOnlyPossibleColumns_WhenHasMoreThanOneFullColumn(
            [Range(0, Board.Width - 1)] int column)
        {
            var game = board;
            var secondColumn = (column + 2) % Board.Width;

            game = FillColumn(column, game);
            game = FillColumn(secondColumn, game);

            var expected = Enumerable
                .Range(0, Board.Width)
                .Where(c => c != column && c != secondColumn);
            game.GetPossibleMoves().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetPossibleMoves_ReturnsNothing_WhenBoardIsFull()
        {
            var game = board;

            game = FillBoard(game);

            var expected = Enumerable.Empty<int>();
            game.GetPossibleMoves().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void EqualityMembers_WorkRight_ForDifferentPaths()
        {
            var game = board;

            var game1 = game
                .Move(0, Cell.Yellow)
                .Move(0, Cell.Yellow)
                .Move(3, Cell.Red)
                .Move(3, Cell.Red);

            var game2 = game
                .Move(0, Cell.Yellow)
                .Move(3, Cell.Red)
                .Move(0, Cell.Yellow)
                .Move(3, Cell.Red);

            game1.GetHashCode().Should().Be(game2.GetHashCode());
            game1.Equals(game2).Should().BeTrue();
        }

        private static Board BuildColumn(Cell player, Board game, int length)
        {
            for (var i = 0; i < length; i++)
                game = game.Move(0, player);
            return game;
        }

        private static Board BuildRow(Cell player, Board game, int length)
        {
            for (var i = 0; i < length; i++)
                game = game.Move(i, player);
            return game;
        }

        private static Board BuildMainDiagonal(Cell player, Board game, int length)
        {
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < i; j++)
                    game = game.Move(i, player.GetOpponent());
                game = game.Move(i, player);
            }

            return game;
        }

        private static Board BuildAntiDiagonal(Cell player, Board game, int length)
        {
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < i; j++)
                    game = game.Move(length - i, player.GetOpponent());
                game = game.Move(length - i, player);
            }

            return game;
        }

        private static Board FillBoard(Board game)
        {
            for (var i = 0; i < 10; i++)
            {
                var yColumn = i * 2 % Board.Width;
                var rColumn = (i * 2 + 1) % Board.Width;
                for (var j = 0; j < 2; j++)
                {
                    game = game.Move(yColumn, Cell.Yellow);
                    game = game.Move(rColumn, Cell.Red);
                }
            }

            game = game.Move(6, Cell.Yellow);
            game = game.Move(6, Cell.Red);
            return game;
        }

        private static Board FillColumn(int column, Board game)
        {
            for (var i = 0; i < 3; i++)
            {
                game = game.Move(column, Cell.Yellow);
                game = game.Move(column, Cell.Red);
            }

            return game;
        }

        private static void AssertPlayerWon(Cell player, Board game)
        {
            game.IsFinished().Should().BeTrue();
            game.GetLinesCountOfLength(4, player).Should().BeGreaterThan(0);
        }
    }
}