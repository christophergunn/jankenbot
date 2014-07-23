using Nancy;
using log4net;

namespace Game.WebApp.Api
{
    public class GamePlay : NancyModule
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(GamePlay));

        public GamePlay()
        {
            Get["/"] = _ => "Nice one.";
            Get["/register/{id}/{name}"] = o =>
                {
                    _logger.DebugFormat("Registered via GET with id: " + o.id + ", name: " + o.name + ", from IP: " + Request.UserHostAddress + ".");
                    return "Registered " + o.id + o.name;
                }; 
            Post["/register/{id}/{name}"] = o =>
                {
                    _logger.DebugFormat("Registered via POST with id: " + o.id + ", name: " + o.name + ", from IP: " + Request.UserHostAddress + ".");
                    return HttpStatusCode.OK;
                };
            
        }
    }
}