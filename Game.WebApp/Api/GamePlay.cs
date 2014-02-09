using Nancy;

namespace Game.WebApp.Api
{
    public class GamePlay : NancyModule
    {
        public GamePlay()
        {
            Get["/"] = _ => "Nice one.";}
    }
}