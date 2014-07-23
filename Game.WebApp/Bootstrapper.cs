using System.Diagnostics;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Game.WebApp
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}