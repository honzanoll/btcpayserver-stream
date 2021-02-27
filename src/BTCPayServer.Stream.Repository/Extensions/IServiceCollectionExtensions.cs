using BTCPayServer.Stream.Data.DALs;
using BTCPayServer.Stream.Data.Models.Invoices;
using BTCPayServer.Stream.Data.Models.OAuth;
using BTCPayServer.Stream.Data.Models.Webhook;
using BTCPayServer.Stream.Repository.Abstractions;
using BTCPayServer.Stream.Repository.Implementations.Users;
using Microsoft.Extensions.DependencyInjection;
using honzanoll.Repository.Abstractions;
using honzanoll.Repository.Implementations;

namespace BTCPayServer.Stream.Repository.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Public methods

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IRepository<StreamlabsAuthToken>, CrudRepository<StreamlabsAuthToken, SqlContext>>();
            services.AddScoped<IRepository<BtcPayServerAuthToken>, CrudRepository<BtcPayServerAuthToken, SqlContext>>();

            services.AddScoped<IRepository<BtcPayServerWebhook>, CrudRepository<BtcPayServerWebhook, SqlContext>>();

            services.AddScoped<IRepository<Invoice>, CrudRepository<Invoice, SqlContext>>();
        }

        #endregion
    }
}
