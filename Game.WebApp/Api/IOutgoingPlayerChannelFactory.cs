using Game.HouseBots;
using Game.HouseBots.Api;

namespace Game.WebApp.Api
{
    public interface IOutgoingPlayerChannelFactory
    {
        IPlayerCommunicationChannel CreateFromHttpEndpoint(string ip);
        IPlayerCommunicationChannel CreateForBot(IBotAi bot);
    }
}