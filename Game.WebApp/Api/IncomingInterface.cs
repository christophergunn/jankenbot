using Game.WebApp.Controller;
using Nancy;
using log4net;

namespace Game.WebApp.Api
{
    public class IncomingInterface : NancyModule
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(IncomingInterface));
        private readonly GameController _controller;

        public IncomingInterface(GameController controller)
        {
            _controller = controller;

            Get["/"] = _ => "Nice one.";

            Get["/start"] = _ =>
                {
                    _logger.DebugFormat("Start game called.");
                    _controller.Start();
                };

            Get["/register/{id}/{name}"] = o =>
                {
                    _logger.DebugFormat("Registered via GET with URL data. ID: " + o.id + ", name: " + o.name + ", from IP: " + Request.UserHostAddress + ".");
                    return "Registered " + o.id + o.name;
                }; 
            Post["/register/{id}/{name}"] = o =>
                {
                    _logger.DebugFormat("Registered via POST with URL data. ID: " + o.id + ", name: " + o.name + ", from IP: " + Request.UserHostAddress + ".");
                    return HttpStatusCode.OK;
                };
            Post["/register"] = o =>
                {
                    _logger.DebugFormat("Registered via POST with form data. ID: " + Request.Form.id + ", name: " + Request.Form.name + ", from IP: " + Request.UserHostAddress + ".");
                    _controller.Register(Request.Form.id, Request.Form.name, Request.UserHostAddress);
                    return HttpStatusCode.OK;
                };

        }
    }
}