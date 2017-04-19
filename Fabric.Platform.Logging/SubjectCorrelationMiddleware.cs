using Fabric.Platform.Shared;
using LibOwin;
using Serilog.Context;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Logging
{
    public class SubjectCorrelationMiddleware
    {
        public const string SubClaim = "sub";
        public static AppFunc Inject(AppFunc next)
        {
            return async env =>
            {
                var owinContext = new OwinContext(env);
                //initialize to unknown
                var subject = "unknown";
                //if the subject id comes in the header set it from there
                if (owinContext.Request.Headers.ContainsKey(Constants.FabricHeaders.SubjectNameHeader))
                {
                    subject = owinContext.Request.Headers[Constants.FabricHeaders.SubjectNameHeader];
                }
                //otherwise try to get it from the current user
                else if (owinContext.Request.User != null)
                {
                    subject = owinContext.Request.User.FindFirst(SubClaim).Value;
                }
                owinContext.Set(Constants.FabricHeaders.SubjectNameHeader, subject);
                using (LogContext.PushProperty(Constants.FabricHeaders.SubjectNameHeader, subject))
                {
                    await next(env);
                }
            };
        }
    }
}
