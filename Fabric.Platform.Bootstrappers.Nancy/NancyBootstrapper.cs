using Fabric.Platform.Http;
using Fabric.Platform.Shared;
using Fabric.Platform.Shared.Configuration;
using Nancy;
using Nancy.Owin;
using Nancy.TinyIoc;

namespace Fabric.Platform.Bootstrappers.Nancy
{
    public static class NancyBootstrapper
    {
        public static TinyIoCContainer UseHttpClientFactory(this TinyIoCContainer self, NancyContext context, IdentityServerConfidentialClientSettings settings)
        {
            var correlationToken = context.GetOwinEnvironment()?[Constants.FabricLogContextProperties.CorrelationTokenContextName] as string;
            self.Register<IHttpClientFactory>(new HttpClientFactory(settings.Authority, settings.ClientId, settings.ClientSecret,
                correlationToken ?? "", ""));
            return self;
        }
    }
}
