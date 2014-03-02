namespace Game
{
    public class TournamentPlayer
    {
        private readonly TournamentPlayerScore _score;

        public TournamentPlayer(string id, string name, IPlayerCommunicationChannel comms)
        {
            Comms = comms;
            Id = id;
            Name = name;
            _score = new TournamentPlayerScore();
            Comms.Start(this);
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public IPlayerCommunicationChannel Comms { get; private set; }

        public TournamentPlayerScore Score { get { return _score; } }
    }
}