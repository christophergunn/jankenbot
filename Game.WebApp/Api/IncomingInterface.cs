using Game.WebApp.Controller;
using Nancy;
using log4net;

namespace Game.WebApp.Api
{
    public class IncomingInterface : NancyModule
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(IncomingInterface));
        private readonly EventCoOrdinator _controller;

        public IncomingInterface(EventCoOrdinator controller)
        {
            _controller = controller;

            Get["/"] = _ => "Nice one. You've navigated to this JankenBot server.";

            Get["/start"] = _ =>
                {
                    if (_controller.IsRunning)
                    {
                        _logger.DebugFormat("Start game called, however it was already running - returning NotAcceptable (406).");
                        return HttpStatusCode.NotAcceptable;
                    }
                    _logger.DebugFormat("Start game called.");
                    _controller.Start();
                    return HttpStatusCode.OK;
                };

            Get["/register/{id}/{name}"] = o =>
                {
                    _logger.DebugFormat("Registered via GET with URL data. ID: " + o.id + ", name: " + o.name + ", from IP: " + Request.UserHostAddress + ".");
                    _controller.Register(Request.Form.id, Request.Form.name, Request.UserHostAddress);
                    return HttpStatusCode.Created;
                }; 
            Post["/register/{id}/{name}"] = o =>
                {
                    _logger.DebugFormat("Registered via POST with URL data. ID: " + o.id + ", name: " + o.name + ", from IP: " + Request.UserHostAddress + ".");
                    _controller.Register(Request.Form.id, Request.Form.name, Request.UserHostAddress);
                    return HttpStatusCode.Created;
                };
            Post["/register"] = o =>
                {
                    _logger.DebugFormat("Registered via POST with form data. ID: " + Request.Form.id + ", name: " + Request.Form.name + ", from IP: " + Request.UserHostAddress + ".");
                    _controller.Register(Request.Form.id, Request.Form.name, Request.UserHostAddress);
                    return HttpStatusCode.Created;
                };

        }
    }
}