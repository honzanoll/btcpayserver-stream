using BTCPayServer.Stream.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.Account
{
    public class LoginFormViewModel
    {
        #region Properties

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_Email), ResourceType = typeof(CommonResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_Password), ResourceType = typeof(CommonResource))]
        public string Password { get; set; }

        #endregion
    }
}
