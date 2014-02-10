using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Game.Tests.Domain
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void CanCreateGame()
        {
            new Game();
        }

        [Test]
        [TestCase(Move.Rock, Move.Rock, RoundResult.Neither)]
        [TestCase(Move.Paper, Move.Paper, RoundResult.Neither)]
        [TestCase(Move.Scissors, Move.Scissors, RoundResult.Neither)]
        public void GivenSameMoves_NeitherPlayerWins(Move playerOneMove, Move playerTwoMove, RoundResult expectedResult)
        {
            var game = new Game();

            game.PlayMoves(playerOneMove, playerTwoMove);

            Assert.That(game.LastResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(Move.Rock, Move.Paper, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Rock, Move.Scissors, RoundResult.PlayerOneWins)]
        [TestCase(Move.Paper, Move.Scissors, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Paper, Move.Rock, RoundResult.PlayerOneWins)]
        [TestCase(Move.Scissors, Move.Rock, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Scissors, Move.Paper, RoundResult.PlayerOneWins)]
        public void GivenStandardMoves_CanDetermineWinner(Move playerOneMove, Move playerTwoMove, RoundResult expectedResult)
        {
            var game = new Game();

            game.PlayMoves(playerOneMove, playerTwoMove);

            Assert.That(game.LastResult, Is.EqualTo(expectedResult));
        }
    }
}
