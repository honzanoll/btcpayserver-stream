using BTCPayServer.Stream.Business.Models.Streamlabs;
using System;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Business.Services.Abstractions
{
    public interface IStreamlabsService
    {
        #region Public methods

        Task ObtainAccessTokenAsync(string code, Guid userId);

        Task ClearTokenAsync(Guid userId);

        Task SendDonateAsync(Guid userId, Donate donate);

        string PrepareMessage(string message);

        #endregion
    }
}
