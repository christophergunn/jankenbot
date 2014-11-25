using System;
using Game.HouseBots.Api;

namespace Game.HouseBots
{
    public class Randomer : IBotAi
    {
        private Random _rnd = new Random(5474);

        public string Name { get { return "A Randomer"; } }

        public Move GetMove()
        {
            return (Move)_rnd.Next(Enum.GetValues(typeof(Move)).Length);
        }

        public void InformOfGameAgainst(TournamentPlayer opponent, int numberOfTurns, int dynamiteLimit)
        {
        }
    }
}
