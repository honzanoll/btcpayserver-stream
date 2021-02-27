using BTCPayServer.Stream.HttpClients.Abstractions;
using System.Net.Http;

namespace BTCPayServer.Stream.HttpClients.Factories.Abstractions
{
    public interface IHttpClientFactory<THttpClient> where THttpClient : IHttpClient
    {
        #region Public methods

        HttpClient CreateClient(string baseAddress, string accessToken);

        #endregion
    }
}
