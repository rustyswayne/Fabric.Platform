using System;
using LibOwin;
using Serilog.Context;
using Fabric.Platform.Shared;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Logging
{
    public class CorrelationTokenMiddleware
    {
        public static AppFunc Inject(AppFunc next)
        {
            return async env =>
            {
                var owinContext = new OwinContext(env);
                var existingCorrelationToken = owinContext.Request.Headers[Constants.FabricHeaders.CorrelationTokenHeaderName];
                if (!Guid.TryParse(existingCorrelationToken, out Guid correlationToken))
                {
                    correlationToken = Guid.NewGuid();
                }
                owinContext.Set(Constants.FabricLogContextProperties.CorrelationTokenContextName, correlationToken.ToString());
                using (LogContext.PushProperty(Constants.FabricLogContextProperties.CorrelationTokenContextName, correlationToken))
                {
                    await next(env);
                }
            };
        }
    }
}
