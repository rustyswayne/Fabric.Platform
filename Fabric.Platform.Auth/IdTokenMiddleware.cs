
using System.IdentityModel.Tokens.Jwt;
using LibOwin;
using Microsoft.IdentityModel.Tokens;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Fabric.Platform.Shared;

namespace Fabric.Platform.Auth
{
    public class IdTokenMiddleware
    {

        public static AppFunc Inject(AppFunc next)
        {
            return env =>
            {
                var ctx = new OwinContext(env);
                if (ctx.Request.Headers.ContainsKey(Constants.FabricHeaders.IdTokenHeader))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken token;
                    var userPrincipal = tokenHandler.ValidateToken(ctx.Request.Headers[Constants.FabricHeaders.IdTokenHeader],
                        new TokenValidationParameters(), out token);
                    ctx.Set(Constants.FabricHeaders.IdTokenHeader, userPrincipal);
                }
                return next(env);
            };
        }
    }
}
