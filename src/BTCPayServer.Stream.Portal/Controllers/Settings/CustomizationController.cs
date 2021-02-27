using BTCPayServer.Stream.Business.Consts.Enums;
using BTCPayServer.Stream.Data.Models.Users;
using BTCPayServer.Stream.Portal.ViewModels.Settings;
using BTCPayServer.Stream.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using honzanoll.MVC.NetCore.ViewModels;
using honzanoll.Storage.NetCore.Abstractions;
using honzanoll.Storage.Models.Settings;
using System.Threading.Tasks;
using honzanoll.Storage.Models;
using BTCPayServer.Stream.Data.Enums;
using System.Linq;
using BTCPayServer.Stream.Common.Resources;

namespace BTCPayServer.Stream.Portal.Controllers.Settings
{

    public class CustomizationController : SettingsControllerBase
    {
        #region Fields

        private readonly IUserRepository userRepository;

        private readonly IStorageProvider<LocalStorage, StorageType> localStorageProvider;

        private readonly LinkGenerator linkGenerator;

        #endregion

        #region Constructors

        public CustomizationController(
            IUserRepository userRepository,
            IStorageProvider<LocalStorage, StorageType> localStorageProvider,
            LinkGenerator linkGenerator)
        {
            this.userRepository = userRepository;

            this.localStorageProvider = localStorageProvider;

            this.linkGenerator = linkGenerator;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await userRepository.GetAsync(CurrentUserId);

            return View(new CustomizationFormViewModel
            {
                StylesheetFileName = currentUser.StylesheetFileObject?.FileName,
                DefaultCurrency = currentUser.DefaultCurrency ?? InvoiceCurrency.USD,
                DefaultCulture = currentUser.DefaultCulture ?? Culture.EN,
                LogoFileName = currentUser.LogoFileObject?.FileName,
                PageTitle = currentUser.PageTitle,
                GtagId = currentUser.GtagId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Process(CustomizationFormViewModel values)
        {
            if (ModelState.IsValid)
            {
                // Check the stylesheet file type
                if (values.Stylesheet != null && values.Stylesheet.ContentType != "text/css")
                    ModelState.AddModelError(nameof(values.Stylesheet), CommonResource.Validation_InvalidFileType);

                // Check the logo file type
                string[] allowedContentTypes = new string[] { "image/png", "image/jpeg" };
                if (values.Logo != null && !allowedContentTypes.Contains(values.Logo.ContentType))
                    ModelState.AddModelError(nameof(values.Logo), CommonResource.Validation_InvalidFileType);

                if (ModelState.IsValid)
                {
                    ApplicationUser currentUser = await userRepository.GetAsync(CurrentUserId);

                    File stylesheetFile = null;

                    // Delete current stylesheet from storage
                    if (currentUser.StylesheetFileObject != null && (values.Stylesheet != null || string.IsNullOrEmpty(values.StylesheetFileName)))
                        await localStorageProvider.DeleteFile(currentUser.StylesheetFileObject);
                    else
                        stylesheetFile = currentUser.StylesheetFileObject;

                    if (values.Stylesheet != null)
                        stylesheetFile = await localStorageProvider.StoreFileAsync(StorageType.Stylesheet, values.Stylesheet);

                    File logoFile = null;

                    // Delete current logo from storage
                    if (currentUser.LogoFileObject != null && (values.Logo != null || string.IsNullOrEmpty(values.StylesheetFileName)))
                        await localStorageProvider.DeleteFile(currentUser.LogoFileObject);
                    else
                        logoFile = currentUser.LogoFileObject;

                    if (values.Logo != null)
                        logoFile = await localStorageProvider.StoreFileAsync(StorageType.Logo, values.Logo);

                    currentUser.StylesheetFileObject = stylesheetFile;
                    currentUser.DefaultCurrency = values.DefaultCurrency;
                    currentUser.DefaultCulture = values.DefaultCulture;
                    currentUser.LogoFileObject = logoFile;
                    currentUser.PageTitle = values.PageTitle;
                    currentUser.GtagId = values.GtagId;

                    await userRepository.UpdateAsync(currentUser);

                    return Json(new FormValidationsViewModel(linkGenerator, nameof(Index), "Settings"));
                }
            }

            return Json(new FormValidationsViewModel(ModelState));
        }
    }
}
