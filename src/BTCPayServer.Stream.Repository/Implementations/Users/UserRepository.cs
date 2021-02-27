using BTCPayServer.Stream.Data.DALs;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Repository.Implementations.Users
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly SqlContext sqlContext;

        #endregion

        #region Constructors

        public UserRepository(SqlContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        #endregion

        #region Public methods

        public async Task StoreBtcPayServerAsync(Guid userId, string btcPayServerUri, string storeId)
        {
            ApplicationUser user = await sqlContext.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException($"Unknown user (userId: {userId})");

            user.BtcPayServerUri = btcPayServerUri;
            user.BtcPayServerStoreId = storeId;

            await sqlContext.SaveChangesAsync();
        }

        public async Task<(string, string)> GetBtcPayServerDataAsync(Guid userId)
        {
            ApplicationUser user = await GetAsync(userId);

            return (user?.BtcPayServerUri, user?.BtcPayServerStoreId);
        }

        public async Task<ApplicationUser> GetAsync(string identifier)
        {
            return await sqlContext.Users
                .Include(u => u.BtcPayServerAuthTokens)
                .FirstOrDefaultAsync(u => u.DonatePageIdentifier == identifier);
        }

        public async Task<ApplicationUser> GetAsync(Guid userId)
        {
            return await sqlContext.Users.FindAsync(userId);
        }

        public bool ExistsIdentifier(string identifier, Guid? userId = null)
        {
            return sqlContext.Users
                .Any(u => u.DonatePageIdentifier == identifier &&
                (!userId.HasValue || u.Id != userId.Value));
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            sqlContext.Entry(user).State = EntityState.Modified;

            await sqlContext.SaveChangesAsync();
        }

        public bool AnyUser()
        {
            return sqlContext.Users.Any();
        }

        #endregion
    }
}
