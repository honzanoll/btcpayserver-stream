using BTCPayServer.Stream.Data.Models.Users;
using System;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Repository.Abstractions
{
    public interface IUserRepository
    {
        #region Public methods

        Task StoreBtcPayServerAsync(Guid userId, string btcPayServerUri, string storeId);

        Task<(string, string)> GetBtcPayServerDataAsync(Guid userId);

        Task<ApplicationUser> GetAsync(string identifier);

        Task<ApplicationUser> GetAsync(Guid userId);

        bool ExistsIdentifier(string identifier, Guid? userId = null);

        Task UpdateAsync(ApplicationUser user);

        #endregion
    }
}
