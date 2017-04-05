using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using BuildFunc = System.Action<System.Func<
                    System.Func<
                        System.Collections.Generic.IDictionary<string, object>,
                        System.Threading.Tasks.Task>,
                    System.Func<
                        System.Collections.Generic.IDictionary<string, object>,
                        System.Threading.Tasks.Task>
                   >>;

namespace Fabric.Platform.Logging
{
    public static class BuildFuncExtensions
    {
        public static BuildFunc UseFabricLoggingAndMonitoring(this BuildFunc buildFunc, ILogger logger,
            Func<Task<bool>> healthCheck, LoggingLevelSwitch levelSwitch)
        {
            buildFunc(next => GlobalErrorLoggingMiddleware.Inject(next, logger));
            buildFunc(next => CorrelationTokenMiddleware.Inject(next));
            buildFunc(next => RequestLoggingMiddleware.Inject(next, logger));
            buildFunc(next => PerformanceLoggingMiddleware.Inject(next, logger));
            buildFunc(next => new DiagnosticsMiddleware(next, levelSwitch).Inject);
            buildFunc(next => new MonitoringMiddleware(next, healthCheck).Inject);
            return buildFunc;
        }
    }
}
