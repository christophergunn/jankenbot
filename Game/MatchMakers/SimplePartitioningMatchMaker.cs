using System;
using System.Collections.Generic;
using System.Linq;
using Game.Utilities;

namespace Game.MatchMakers
{
    public class SimplePartitioningMatchMaker :IMatchMaker
    {
        public void Invoke(IEnumerable<TournamentPlayer> players)
        {
            if (players == null || players.Count() < 2)
            {
                Matches = null;
                return;
            }
            if (players.Count() % 2 != 0)
                throw new ArgumentException("Numbers of players must be a multiple of two.", "players");

            var partition = players.Partition(2);
            Matches = (from p in partition select new Tuple<TournamentPlayer, TournamentPlayer>(p[0], p[1])).ToList();
        }

        public IList<Tuple<TournamentPlayer, TournamentPlayer>> Matches { get; private set; }
    }
}
