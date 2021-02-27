using System.ComponentModel.DataAnnotations;

namespace BTCPayServer.Stream.Portal.ViewModels.Account
{
    public class GoogleRegisterFormViewModel
    {
        #region Properties

        [Required(ErrorMessage = "Required field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string FullName => $"{Firstname} {Surname}";

        [Display(Name = "Donate page identifier")]
        [Required(ErrorMessage = "Required field")]
        [RegularExpression("^[a-z0-9_]*$", ErrorMessage = "Only alphanumeric characters and _ (underscore) allowed")]
        public string DonatePageIdentifier { get; set; }

        public string Error { get; set; }

        #endregion
    }
}
