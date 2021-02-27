using BTCPayServer.Stream.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.Settings
{
    public class ProfileFormViewModel
    {
        #region Properties

        [Display(Name = nameof(CommonResource.Label_Email), ResourceType = typeof(CommonResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_DonatePageIdentifier), ResourceType = typeof(CommonResource))]
        [RegularExpression("^[a-z0-9_]*$", ErrorMessageResourceName = nameof(CommonResource.Validation_IdentifierRegularExpressionMatch), ErrorMessageResourceType = typeof(CommonResource))]
        public string DonatePageIdentifier { get; set; }

        #endregion
    }
}
