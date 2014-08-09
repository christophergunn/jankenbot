namespace Game
{
    public class TournamentPlayer
    {
        private readonly TournamentPlayerScore _score = new TournamentPlayerScore();

        public TournamentPlayer(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public IPlayerCommunicationChannel Comms { get; set; }

        public TournamentPlayerScore Score { get { return _score; } }
    }
}