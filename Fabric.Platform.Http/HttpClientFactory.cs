using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Fabric.Platform.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private const string CorrelationTokenHeaderName = "Correlation-Token";
        private const string IdTokenHeader = "fabric-end-user";
        private readonly string _correlationToken;
        private readonly string _idToken;
        private readonly TokenClient _tokenClient;

        public HttpClientFactory(string tokenUrl, string clientId, string secret, string correlationToken, string idToken)
        {
            _correlationToken = correlationToken;
            _idToken = idToken;
            _tokenClient = new TokenClient(tokenUrl, clientId, secret);
        }
        public async Task<HttpClient> Create(Uri uri, string requestScope)
        {
            var response = await _tokenClient.RequestClientCredentialsAsync(requestScope).ConfigureAwait(false);
            return CreateWithAccessToken(uri, response.AccessToken);
        }

        public HttpClient CreateWithAccessToken(Uri uri, string accessToken)
        {
            var client = new HttpClient{ BaseAddress = uri};
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add(CorrelationTokenHeaderName, _correlationToken);
            if (!string.IsNullOrEmpty(_idToken))
            {
                client.DefaultRequestHeaders.Add(IdTokenHeader, _idToken);
            }
            return client;
        }
    }
}
