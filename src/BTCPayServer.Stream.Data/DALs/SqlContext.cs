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
        }
    }
}
