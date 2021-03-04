using BTCPayServer.Stream.Data.DALs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BTCPayServer.Stream.Data.Extensions
{
    public static class IServiceCollectionExtensions
    {
        #region Public methods

        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            string provider = configuration.GetValue("DatabaseProvider", "Sqlite");

            string npgsqlConnectionString = configuration.GetConnectionString("NpgsqlConnection");
            string sqliteConnectionString = configuration.GetConnectionString("SqliteConnection");

            services.AddDbContext<SqlContext>(
                options => _ = provider switch
                {
                    "Npgsql" => options.UseNpgsql(
                        npgsqlConnectionString,
                        x => x.MigrationsAssembly("BTCPayServer.Stream.Data.Npgsql")),

                    "Sqlite" => options.UseSqlite(
                        sqliteConnectionString,
                        x => x.MigrationsAssembly("BTCPayServer.Stream.Data.Sqlite")),

                    _ => throw new Exception($"Unsupported provider: {provider}")
                });

        }

        #endregion
    }
}
