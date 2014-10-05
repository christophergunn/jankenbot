using System;
using Game.HouseBots;

namespace Game.WebApp.Api
{
    public class OutgoingPlayerChannelFactory : IOutgoingPlayerChannelFactory
    {
        public IPlayerCommunicationChannel CreateFromHttpEndpoint(string ip)
        {
            return new OutgoingPlayerChannel(new Uri("http://" + ip));
        }

        public IPlayerCommunicationChannel CreateForBot(IBotAi bot)
        {
            return new BotCommunicationChannel(bot);
        }
    }
}