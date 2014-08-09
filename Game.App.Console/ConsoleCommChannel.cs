using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.App.Console
{
    public class ConsoleCommChannel : IPlayerCommunicationChannel
    {
        private String _playerCallsign;

        public ConsoleCommChannel(TournamentPlayer player)
        {
            _playerCallsign = player.Name;
        }

        public void InformOfGameAgainst(TournamentPlayer opponent)
        {
            ConsoleUi.WriteTextLine(CreatePlayerPrefix() + "your opponent for this round is " + opponent.Name + ".");
        }

        private Dictionary<char, Move> _charToMoveMapping = new Dictionary<char, Move>
        {
            {'r', Move.Rock},
            {'p', Move.Paper},
            {'s', Move.Scissors},
            {'d', Move.Dynamite},
            {'w', Move.Waterbomb}
        };

        public Task<Move> RequestMove()
        {
            char moveChar = ConsoleUi.WriteTextThenReadKey(
CreatePlayerPrefix() + @"please select your move: 
    r - Rock
    p - Paper
    s - Scissors
    d - Dynamite
    w - Waterbomb
");
            var move = _charToMoveMapping[moveChar];
            ConsoleUi.WriteTextLine(string.Format(" - {0} recorded.", move));

            return Task.FromResult(move);
        }

        private string CreatePlayerPrefix()
        {
            return string.Format("PLAYER-{0}-CONSOLE: ", _playerCallsign);
        }
    }
}