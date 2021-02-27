using BTCPayServer.Stream.Data.DALs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BTCPayServer.Stream.Data.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Public methods

        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlContext>(options => options.UseNpgsql(configuration.GetConnectionString("NpgsqlConnection")));
        }

        #endregion
    }
}
