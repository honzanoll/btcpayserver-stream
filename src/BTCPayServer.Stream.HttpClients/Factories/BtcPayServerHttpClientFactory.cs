using BTCPayServer.Stream.HttpClients.BtcPayServerClient;
using BTCPayServer.Stream.HttpClients.Factories.Abstractions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BTCPayServer.Stream.HttpClients.Factories
{
    public class BtcPayServerHttpClientFactory : IHttpClientFactory<BtcPayServerHttpClient>
    {
        public HttpClient CreateClient(string baseAddress, string accessToken)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"token {accessToken}");

            return httpClient;
        }
    }
}
