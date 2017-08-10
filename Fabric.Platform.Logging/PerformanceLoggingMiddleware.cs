using System.Diagnostics;
using LibOwin;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Logging
{
    public class PerformanceLoggingMiddleware
    {
        public static AppFunc Inject(AppFunc next, ILogger logger)
        {
            return async env =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await next(env);
                stopWatch.Stop();
                var owinContext = new OwinContext(env);

                logger.Information<PerformanceLoggingMiddleware>("Request: {@Method} {@Path} executed in {@RequestTime:000} ms",
                    () => new object[]
                    {
                        owinContext.Request.Method, owinContext.Request.Path, stopWatch.ElapsedMilliseconds
                    });
            };
        }
    }
}
