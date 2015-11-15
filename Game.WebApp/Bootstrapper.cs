using System;
using Game.WebApp.Api;
using Game.WebApp.Configuration;
using Game.WebApp.Controller;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace Game.WebApp
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private Lazy<TournamentPersistence> _persistence = new Lazy<TournamentPersistence>();
        private Lazy<OutgoingPlayerChannelFactory> _outgoingChannelFactory = new Lazy<OutgoingPlayerChannelFactory>();

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            // call 'base' first - calling it later would override any of our customisations
            base.ConfigureApplicationContainer(container);

            container.Register<ITournamentPersistence, TournamentPersistence>().AsSingleton();
            container.Register<IOutgoingPlayerChannelFactory, OutgoingPlayerChannelFactory>().AsSingleton();
            container.Register<IApplicationConfiguration, ApplicationConfiguration>().AsSingleton();
            container.Register<IActionScheduler, TimerBasedActionScheduler>();

            container.Register<EventCoOrdinator, EventCoOrdinator>().AsSingleton();
        }
    }
}