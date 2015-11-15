namespace Game.WebApp.Client.GameServerRequests
{
    public abstract class GameServerRequest
    {
        protected readonly string _serverUrl;

        protected GameServerRequest(string serverUrl)
        {
            if (!serverUrl.EndsWith("/"))
            {
                serverUrl += "/";
            }
            _serverUrl = serverUrl;
        }

        protected string GetServerUrl(string relativePath)
        {
            return _serverUrl + relativePath;
        }
    }
}