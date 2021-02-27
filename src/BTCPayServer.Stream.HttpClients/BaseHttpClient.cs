using BTCPayServer.Stream.Common.Extensions;
using BTCPayServer.Stream.HttpClients.Abstractions;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.HttpClients
{
    public abstract class BaseHttpClient<THttpClient> where THttpClient : IHttpClient
    {
        #region Fields

        private readonly ILogger<THttpClient> logger;

        #endregion

        #region Constructors

        public BaseHttpClient(ILogger<THttpClient> logger)
        {
            this.logger = logger;
        }

        #endregion

        #region Protected methods

        protected StringContent GetRequest<TType>(TType data)
        {
            return new StringContent(data.ToJson(), Encoding.UTF8, "application/json");
        }

        protected async Task<TType> GetResponse<TType>(HttpContent responseContent) where TType : new()
        {
            string content = await responseContent.ReadAsStringAsync();

            logger.LogDebug($"Received response: {content}");

            return content.FromJson<TType>();
        }

        protected async Task EnsureValidResponseAsync(HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                logger.LogError($"Error while getting a response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");

                throw;
            }
        }

        #endregion
    }
}
