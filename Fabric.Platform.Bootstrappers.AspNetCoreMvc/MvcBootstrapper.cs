using System.Linq;
using Fabric.Platform.Shared.Configuration;
using Fabric.Platform.Http;
using Microsoft.AspNetCore.Owin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabric.Platform.Bootstrappers.AspNetCoreMvc
{
    public static class MvcBootstrapper
    {
        public static void AddHttpClientFactory(this IServiceCollection services,
            IdentityServerConfidentialClientSettings settings)
        {
            services.AddScoped<IHttpClientFactory>(ctx =>
            {
                var contextAccessor = ctx.GetRequiredService<IHttpContextAccessor>();
                var context = contextAccessor.HttpContext;
                // ReSharper disable once CollectionNeverUpdated.Local
                var owinEnvironment = new OwinEnvironment(context);
                var correlationToken =
                    owinEnvironment.FirstOrDefault(
                        x => x.Key == Shared.Constants.FabricLogContextProperties.CorrelationTokenContextName).Value as string;
                return new HttpClientFactory(settings.Authority, settings.ClientId,
                    settings.ClientSecret, correlationToken ?? "", "");
            });
        }
    }
}
