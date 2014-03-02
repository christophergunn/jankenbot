using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.MatchMakers;
using Game.Utilities;
using NUnit.Framework;

namespace Game.Tests.Domain.MatchMakers
{
    [TestFixture]
    public class SimplePartitioningMatchMakerTests
    {
        [Test]
        public void Invoke_ShouldPartitionPlayersToCreateMatches()
        {
            var matchMaker = new SimplePartitioningMatchMaker();
            matchMaker.Invoke(new[] { 
                new TournamentPlayer("1", null, null), new TournamentPlayer("2", null, null), 
                new TournamentPlayer("3", null, null), new TournamentPlayer("4", null, null) 
            });

            Assert.That(matchMaker.Matches.First().Item1.Id, Is.EqualTo("1"));
            Assert.That(matchMaker.Matches.First().Item2.Id, Is.EqualTo("2"));
            Assert.That(matchMaker.Matches.Second().Item1.Id, Is.EqualTo("3"));
            Assert.That(matchMaker.Matches.Second().Item2.Id, Is.EqualTo("4"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_WithOddNumberOFArgumentsShouldThrowException()
        {
            var matchMaker = new SimplePartitioningMatchMaker();
            matchMaker.Invoke(new[] { 
                new TournamentPlayer("1", null, null), new TournamentPlayer("2", null, null), 
                new TournamentPlayer("3", null, null)
            });
        }
    }
}
