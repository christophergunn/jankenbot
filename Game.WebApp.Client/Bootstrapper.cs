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
            container.Register(typeof(ClientConfig), (ioc, npo) => new ClientConfig { Id = "1", Name = "Mr Bob", ServerUrl = "http://localhost/Game.WebApp"});
        }  
    }
}
