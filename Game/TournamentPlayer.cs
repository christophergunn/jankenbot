namespace Game
{
    public class TournamentPlayer
    {
        private IPlayerCommunicationChannel _comms;

        public TournamentPlayer(string id, IPlayerCommunicationChannel comms)
        {
            _comms = comms;
            Id = id;
        }

        public string Id { get; private set; }

        public string Name { get; set; }
    }
}