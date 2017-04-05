
using System.IdentityModel.Tokens.Jwt;
using LibOwin;
using Microsoft.IdentityModel.Tokens;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Auth
{
    public class IdTokenMiddleware
    {
        private const string IdTokenHeader = "fabric-end-user";
        public static AppFunc Inject(AppFunc next)
        {
            return env =>
            {
                var ctx = new OwinContext(env);
                if (ctx.Request.Headers.ContainsKey(IdTokenHeader))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken token;
                    var userPrincipal = tokenHandler.ValidateToken(ctx.Request.Headers[IdTokenHeader],
                        new TokenValidationParameters(), out token);
                    ctx.Set(IdTokenHeader, userPrincipal);
                }
                return next(env);
            };
        }
    }
}
