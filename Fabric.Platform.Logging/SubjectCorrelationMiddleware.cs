//Copyright 2017 HealthCatalyst

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

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
