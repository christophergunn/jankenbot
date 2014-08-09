namespace Game.WebApp.Controller
{
    public interface IOutgoingPlayerChannelFactory
    {
        IPlayerCommunicationChannel CreateFromIp(string ip);
    }
}