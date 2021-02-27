using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using honzanoll.Storage.NetCore.Abstractions;
using honzanoll.Storage.Abstractions.Models;
using honzanoll.Web.Middlewares.Exceptions;
using System;
using System.Threading.Tasks;

namespace BTCPayServer.Stream.Portal.Middlewares.Extensions
{
    public static class IStorageProviderExtensions
    {
        #region Public methods

        public static async Task<byte[]> GetStylesheetAsync<TProvider>(
            this IStorageProvider<TProvider, StorageType> storageProvider,
            string identifier,
            IServiceProvider serviceProvider) where TProvider : IStorageProviderSettings
        {
            IUserRepository userRepository = serviceProvider.GetRequiredService<IUserRepository>();

            ApplicationUser user = await userRepository.GetAsync(identifier);
            if (user == null)
                throw new UnknownDocumentException(nameof(user));

            if (user.StylesheetFileObject == null)
                throw new UnknownDocumentException(nameof(user));

            return await storageProvider.GetFileAsync(user.StylesheetFileObject);
        }

        public static async Task<byte[]> GetLogoAsync<TProvider>(
            this IStorageProvider<TProvider, StorageType> storageProvider,
            string identifier,
            IServiceProvider serviceProvider) where TProvider : IStorageProviderSettings
        {
            IUserRepository userRepository = serviceProvider.GetRequiredService<IUserRepository>();

            ApplicationUser user = await userRepository.GetAsync(identifier);
            if (user == null)
                throw new UnknownDocumentException(nameof(user));

            if (user.LogoFileObject == null)
                throw new UnknownDocumentException(nameof(user));

            return await storageProvider.GetFileAsync(user.LogoFileObject);
        }

        #endregion
    }
}
