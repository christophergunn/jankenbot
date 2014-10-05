using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.WebApp.Client.Configuration
{
    public static class AppConfigReader
    {

        public static ClientConfig RetrieveClientConfig()
        {
            return new ClientConfig(ConfigurationManager.AppSettings["serverUrl"], ConfigurationManager.AppSettings["clientId"],
                                    ConfigurationManager.AppSettings["clientName"]);
        }
    }
}
