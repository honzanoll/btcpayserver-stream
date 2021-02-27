using BTCPayServer.Stream.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.Settings
{
    public class ChangePasswordFormViewModel
    {
        #region Properties

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_CurrentPassword), ResourceType = typeof(CommonResource))]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_NewPassword), ResourceType = typeof(CommonResource))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = nameof(CommonResource.Validation_Required), ErrorMessageResourceType = typeof(CommonResource))]
        [Display(Name = nameof(CommonResource.Label_ConfirmNewPassword), ResourceType = typeof(CommonResource))]
        public string ConfirmPassword { get; set; }

        #endregion
    }
}
