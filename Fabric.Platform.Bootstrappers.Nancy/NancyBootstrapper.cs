using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using Nancy.Owin;
using Nancy.TinyIoc;

namespace Fabric.Platform.Bootstrappers.Nancy
{
    public static class NancyBootstrapper
    {
        private static string _tokenUrl;
        private static string _clientId;
        private static string _clientSecret;
        public const string CorrelationTokenContextName = "CorrelationToken";

        public static void Configure(string tokenUrl, string clientId, string clientSecret)
        {
            _tokenUrl = tokenUrl;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public static TinyIoCContainer UseHttpClientFactory(this TinyIoCContainer self, NancyContext context)
        {
            var correlationToken = context.GetOwinEnvironment()?[CorrelationTokenContextName] as string;
            //self.Register<IHttpClientFactory>(new HttpClientFactory(_tokenUrl, _clientId, _clientSecret,
            //    correlationToken ?? "", ""));
            return self;
        }
    }
}
