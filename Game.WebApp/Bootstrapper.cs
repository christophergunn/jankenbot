using System.Diagnostics;
using Game.WebApp.Controller;
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
            RegisterInstances(container, new[] { new InstanceRegistration(typeof(GameController), GameController.GetSingleton()) });
        }
    }
}