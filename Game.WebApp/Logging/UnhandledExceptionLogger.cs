using System;
using Nancy;
using log4net;

namespace Game.WebApp.Logging
{
    public class UnhandledExceptionLogger : Nancy.ErrorHandling.IStatusCodeHandler
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UnhandledExceptionLogger));

        public bool HandlesStatusCode(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return statusCode == HttpStatusCode.InternalServerError;
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            object errorObject;
            context.Items.TryGetValue(NancyEngine.ERROR_EXCEPTION, out errorObject);
            var error = errorObject as Exception;

            if (error != null)
            {
                _logger.Error("Unhandled error", error);
                throw error;
            }
        }
    }
}