namespace Game.App.Console
{
    public class ConsoleCommChannel : IPlayerCommunicationChannel
    {
        private TournamentPlayer _player;

        public void Start(TournamentPlayer p1)
        {
            _player = p1;
        }
    }
}
