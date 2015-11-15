using System.Net;

namespace Game.WebApp.Client.GameServerRequests
{
    public class StartGameRequest : GameServerRequest
    {
        public StartGameRequest(string serverUrl)
            : base(serverUrl)
        {
        }

        public void Execute()
        {
            using (var client = new WebClient())
            {
                client.DownloadString(GetServerUrl("start"));
            }
        }        
    }
}