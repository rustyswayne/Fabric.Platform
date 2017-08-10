using System;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Logging
{
    public class GlobalErrorLoggingMiddleware
    {
        public static AppFunc Inject(AppFunc next, ILogger logger)
        {
            return async env =>
            {
                try
                {
                    await next(env);
                }
                catch (Exception ex)
                {
                    // TODO review
                    logger.Error<GlobalErrorLoggingMiddleware>("Unhandled exception", ex);

                }
            };
        }
    }
}
