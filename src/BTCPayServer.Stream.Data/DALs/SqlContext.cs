using BTCPayServer.Stream.Data.Models.Invoices;
using BTCPayServer.Stream.Data.Models.OAuth;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Data.Models.Webhook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BTCPayServer.Stream.Data.DALs
{
    public class SqlContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        #region Properties

        public DbSet<StreamlabsAuthToken> StreamlabsAuthToken { get; }
        public DbSet<BtcPayServerAuthToken> BtcPayServerAuthToken { get; }

        public DbSet<BtcPayServerWebhook> BtcPayServerWebhook { get; }

        public DbSet<Invoice> Invoices { get; }

        #endregion

        #region Constructors

        public SqlContext() { }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options) { }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Sqlite name tables without schema
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                builder.Entity<StreamlabsAuthToken>()
                    .ToTable("oAuth_Streamlabs");
                builder.Entity<BtcPayServerAuthToken>()
                    .ToTable("oAuth_BtcPayServer");

                builder.Entity<BtcPayServerWebhook>()
                    .ToTable("webhook_BtcPayServer");
            }
            else
            {
                builder.Entity<StreamlabsAuthToken>()
                    .ToTable("Streamlabs", "oAuth");
                builder.Entity<BtcPayServerAuthToken>()
                    .ToTable("BtcPayServer", "oAuth");

                builder.Entity<BtcPayServerWebhook>()
                    .ToTable("BtcPayServer", "webhook");
            }
        }
    }
}
