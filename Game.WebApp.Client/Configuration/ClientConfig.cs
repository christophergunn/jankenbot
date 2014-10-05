namespace Game.WebApp.Client.Configuration
{
    public class ClientConfig
    {
        private readonly string _serverUrl;
        private readonly string _id;
        private readonly string _name;

        public string ServerUrl { get { return _serverUrl; } }
        public string Id { get { return _id; } }
        public string Name { get { return _name; } }

        public ClientConfig(string serverUrl, string id, string name)
        {
            _serverUrl = serverUrl;
            _id = id;
            _name = name;
        }
    }
}