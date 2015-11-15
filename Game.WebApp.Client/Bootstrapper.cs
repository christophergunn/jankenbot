using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.WebApp.Client.Configuration;
using Nancy;

namespace Game.WebApp.Client
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var config = AppConfigReader.RetrieveClientConfig();
            var client = new Domain.Client(config.Id, config.Name);

            container.Register(config);
            container.Register(client);
        }  
    }
}
