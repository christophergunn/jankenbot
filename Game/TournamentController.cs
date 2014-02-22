using System;
using System.Collections.Generic;

namespace Game
{
    public class TournamentController
    {
        private readonly IMatchMaker _matchMaker;
        private readonly Dictionary<string, TournamentPlayer> _playersMap = new Dictionary<string, TournamentPlayer>();
        private TournamentConfiguration _tournamentConfiguration;

        public TournamentController(IMatchMaker matchMaker)
        {
            _matchMaker = matchMaker;
        }

        public IEnumerable<TournamentPlayer> Players { get { return _playersMap.Values; } }

        public void RegisterPlayer(TournamentPlayer player)
        {
            if (_playersMap.ContainsKey(player.Id))
            {
                _playersMap[player.Id].Name = player.Name;
            }
            else
            {
                _playersMap.Add(player.Id, player);
            }
        }

        public void BeginRound()
        {
            _matchMaker.Invoke(Players);
        }

        public void Setup(TournamentConfiguration tournamentConfiguration)
        {
            _tournamentConfiguration = tournamentConfiguration;
        }
    }
}
