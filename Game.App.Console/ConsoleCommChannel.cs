using System;

namespace Game.App.Console
{
    public class ConsoleCommChannel : IPlayerCommunicationChannel
    {
        private TournamentPlayer _player;

        public void SetPlayer(TournamentPlayer p1)
        {
            _player = p1;
        }

        public void InformOfGameAgainst(TournamentPlayer p1)
        {
            ConsoleUi.WriteText("PLAYER-" + _player.Name + "-CONSOLE: your opponent for this round is " + p1.Name + ".");
        }

        public event EventHandler MoveRecieved;
    }
}
