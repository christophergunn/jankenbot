using System.Collections.Specialized;
using System.Net;

namespace Game.WebApp.Client.GameServerRequests
{
    public class RegistrationRequest : GameServerRequest
    {
        private readonly string _id;
        private readonly string _name;

        public RegistrationRequest(string serverUrl, string id, string name)
            : base(serverUrl)
        {
            _id = id;
            _name = name;
        }

        public void Execute()
        {
            using (var client = new WebClient())
            {
                //client.UploadValues(GetServerUrl(string.Format("register/{0}/{1}", id, name)), new NameValueCollection());
                client.UploadValues(GetServerUrl("register"), new NameValueCollection { { "id", _id }, { "name", _name } });
            }
        }
    }
}
