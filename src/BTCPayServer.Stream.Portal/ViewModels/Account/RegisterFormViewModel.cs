using BTCPayServer.Stream.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.Account
{
    public class RegisterFormViewModel
    {
        #region Public methods

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessageResourceName = nameof(CommonResource.Validation_Invalid_Format), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_Email), ResourceType = typeof(CommonResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_Password), ResourceType = typeof(CommonResource))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_ConfirmPassword), ResourceType = typeof(CommonResource))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_DonatePageIdentifier), ResourceType = typeof(CommonResource))]
        [RegularExpression("^[a-z0-9_]*$", ErrorMessageResourceName = nameof(CommonResource.Validation_IdentifierRegularExpressionMatch), ErrorMessageResourceType = typeof(CommonResource))]
        public string DonatePageIdentifier { get; set; }

        #endregion
    }
}
