using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class TournamentRound
    {
        private readonly int _sequenceNumber;
        private readonly IEnumerable<TournamentPlayer> _players;
        private IEnumerable<TournamentGame> _games; 

        public TournamentRound(int sequenceNumber, IEnumerable<TournamentPlayer> players)
        {
            _sequenceNumber = sequenceNumber;
            _players = players;
            _games = Enumerable.Empty<TournamentGame>();
        }

        public void SetGames(IEnumerable<TournamentGame> games)
        {
            _games = games.ToList();
        }

        public int SequenceNumber { get { return _sequenceNumber; } }
        public IEnumerable<TournamentPlayer> Players { get { return _players; } }
        public IEnumerable<TournamentGame> Games { get { return _games; } }
    }
}