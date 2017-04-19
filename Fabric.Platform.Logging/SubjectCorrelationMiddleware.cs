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
                var subject = owinContext.Request.Headers[Constants.FabricHeaders.SubjectNameHeader];
                if (string.IsNullOrEmpty(subject))
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
