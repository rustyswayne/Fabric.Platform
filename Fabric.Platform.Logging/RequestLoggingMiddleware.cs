using LibOwin;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Logging
{
    public class RequestLoggingMiddleware
    {
        public static AppFunc Inject(AppFunc next, ILogger logger)
        {
            return async env =>
            {
                var owinContext = new OwinContext(env);

                logger.Information<RequestLoggingMiddleware>(
                    "Incoming request: {@Method}, {@Path}, {@Headers}",
                    () => new object[]
                    {
                        owinContext.Request.Method,
                        owinContext.Request.Path,
                        owinContext.Request.Headers
                    });


                await next(env);

                logger.Information<RequestLoggingMiddleware>(
                    "Outgoing response: {@StatusCode}, {@Headers}",
                    () => new object[]
                    {
                        owinContext.Response.StatusCode,
                        owinContext.Response.Headers
                    });
            };
        }
    }
}
