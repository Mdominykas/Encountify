using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc.Controllers;

namespace EncountifyAPI.Middleware
{
    public class StatisticsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        
        public StatisticsMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var controllerActionDecriptor =
                context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDecriptor.ControllerName;
            var actionName = controllerActionDecriptor.ActionName;

            await _next(context);

            sw.Stop();

            _logger.Information($"It took {sw.ElapsedMilliseconds} ms to perform " + 
                $"this action {actionName} in this controller {controllerName}");
        }
    }
}