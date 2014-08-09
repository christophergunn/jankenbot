using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Game.WebApp.Client.Configuration;

namespace Game.WebApp.Client
{
    public class Client : Nancy.NancyModule
    {
        private readonly string _serverUrl;
        private readonly string _id;
        private readonly string _name;

        public Client(ClientConfig config)
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
