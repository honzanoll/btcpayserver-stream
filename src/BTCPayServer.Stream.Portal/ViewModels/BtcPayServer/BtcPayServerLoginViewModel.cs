using BTCPayServer.Stream.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.BtcPayServer
{
    public class BtcPayServerLoginViewModel
    {
        #region Properties

        [Display(Name = nameof(CommonResource.Label_BTCPayServerUrl), ResourceType = typeof(CommonResource))]
        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [DataType(DataType.Url, ErrorMessageResourceName = nameof(CommonResource.Validation_InvalidUrl), ErrorMessageResourceType = typeof(CommonResource))]
        public string ServerUrl { get; set; }

        [Display(Name = nameof(CommonResource.Label_BTCPayServerStoreId), ResourceType = typeof(CommonResource))]
        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [RegularExpression("^[a-zA-Z0-9]{44}$", ErrorMessageResourceName = nameof(CommonResource.Validation_Invalid_Format), ErrorMessageResourceType = typeof(CommonResource))]
        public string StoreId { get; set; }

        #endregion
    }
}
