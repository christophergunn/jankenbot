using System;
using System.Collections.Specialized;
using System.Net;
using Game.WebApp.Client.Configuration;

namespace Game.WebApp.Client
{
    public class ClientHttpInterface : Nancy.NancyModule
    {
        private readonly string _serverUrl;
        private readonly string _id;
        private readonly string _name;

        public ClientHttpInterface(ClientConfig config)
        {
            string serverUrl = config.ServerUrl;
            if (!serverUrl.EndsWith("/"))
            {
                serverUrl += "/";
            }
            _id = config.Id;
            _name = config.Name;
            _serverUrl = serverUrl;

            Get["/hello"] = _ => string.Format("Hi there, my name is {0} and my ID is {1}.", _id, _name);

            Post["/start"] = o => ProcessStart(o.opponentId, o.opponentName, o.pointsToWin, o.numberOfTurns, o.dynamiteLimit);

            Get["/move"] = _ => GetNextMove();

            Post["/move"] = o => ProcessLastOpponentMove(o.move);
        }

        private void ProcessStart(string opponentId, string opponentName, string pointsToWin, string maxRounds, string dynamiteAllowance)
        {
            throw new NotImplementedException();
        }

        private string GetNextMove()
        {
            throw new NotImplementedException();
        }

        private void ProcessLastOpponentMove(string move)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered { get; private set; }

        public void Register()
        {
            using (var client = new WebClient())
            {
                //client.UploadValues(GetServerUrl(string.Format("register/{0}/{1}", id, name)), new NameValueCollection());
                client.UploadValues(GetServerUrl("register"), new NameValueCollection { { "id", _id }, { "name", _name } });
                IsRegistered = true;
            }
        }

        private string GetServerUrl(string relativePath)
        {
            return _serverUrl + relativePath;
        }
    }
}
