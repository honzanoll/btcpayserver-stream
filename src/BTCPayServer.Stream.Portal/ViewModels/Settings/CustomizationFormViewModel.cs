using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Data.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.Settings
{
    public class CustomizationFormViewModel
    {
        #region Properties

        [Display(Name = nameof(CommonResource.Label_DonatePageStylesheet), ResourceType = typeof(CommonResource))]
        public IFormFile Stylesheet { get; set; }
        public string StylesheetFileName { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_DefaultCurrency), ResourceType = typeof(CommonResource))]
        public InvoiceCurrency? DefaultCurrency { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_DefaultCulture), ResourceType = typeof(CommonResource))]
        public Culture? DefaultCulture { get; set; }

        [Display(Name = nameof(CommonResource.Label_PageLogo), ResourceType = typeof(CommonResource))]
        public IFormFile Logo { get; set; }
        public string LogoFileName { get; set; }

        [Display(Name = nameof(CommonResource.Label_PageTitle), ResourceType = typeof(CommonResource))]
        public string PageTitle { get; set; }

        [Display(Name = nameof(CommonResource.Label_GtagId), ResourceType = typeof(CommonResource))]
        public string GtagId { get; set; }

        #endregion
    }
}
