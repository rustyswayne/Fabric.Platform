using LibOwin;
using Serilog;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Logging
{
    public class RequestLoggingMiddleware
    {
        public static AppFunc Inject(AppFunc next, ILogger logger)
        {
            return async env =>
            {
                var contextSpecificLogger = logger.ForContext<RequestLoggingMiddleware>();
                var owinContext = new OwinContext(env);

                contextSpecificLogger.Information("Incoming request: {@Method}, {@Path}, {@Headers}",
                        owinContext.Request.Method,
                        owinContext.Request.Path,
                        owinContext.Request.Headers);

                await next(env);

                contextSpecificLogger.Information("Outgoing response: {@StatusCode}, {@Headers}",
                    owinContext.Response.StatusCode,
                    owinContext.Response.Headers);
            };
        }
    }
}
