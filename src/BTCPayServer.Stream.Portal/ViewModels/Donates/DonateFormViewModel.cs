using BTCPayServer.Stream.Common.Resources;
using BTCPayServer.Stream.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.Donates
{
    public class DonateFormViewModel
    {
        #region Properties

        public Guid TargetUserId { get; set; }
        public string TargetUserIdentifier { get; set; }

        [Display(Name = nameof(CommonResource.Label_Amount), ResourceType = typeof(CommonResource))]
        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        public decimal? Amount { get; set; }

        [Display(Name = nameof(CommonResource.Label_YourName), ResourceType = typeof(CommonResource))]
        [MinLength(2, ErrorMessageResourceName = nameof(CommonResource.Validation_MinLength_2), ErrorMessageResourceType = typeof(CommonResource))]
        [MaxLength(25, ErrorMessageResourceName = nameof(CommonResource.Validation_MaxLength_25), ErrorMessageResourceType = typeof(CommonResource))]
        [RegularExpression("^[a-zA-Z0-9_ ]*$", ErrorMessageResourceName = nameof(CommonResource.Validation_DonatorRegularExpressionMatch), ErrorMessageResourceType = typeof(CommonResource))]
        public string Donator { get; set; }

        [Display(Name = nameof(CommonResource.Label_YourMessage), ResourceType = typeof(CommonResource))]
        [MaxLength(230, ErrorMessageResourceName = nameof(CommonResource.Validation_MaxLength_230), ErrorMessageResourceType = typeof(CommonResource))]
        public string Message { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        public InvoiceCurrency? Currency { get; set; }

        public string Problem { get; set; }

        #endregion
    }
}
