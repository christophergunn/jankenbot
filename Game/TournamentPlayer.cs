namespace Game
{
    public class TournamentPlayer
    {
        private readonly TournamentPlayerScore _score = new TournamentPlayerScore();

        public TournamentPlayer(string id, string name, IPlayerCommunicationChannel comms)
        {
            Comms = comms;
            Id = id;
            Name = name;
            Comms.SetPlayer(this);
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public IPlayerCommunicationChannel Comms { get; private set; }

        public TournamentPlayerScore Score { get { return _score; } }
    }
}