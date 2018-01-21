using System.ComponentModel.DataAnnotations;

namespace TaskManager.Web.Models {

    public class LoginViewModel {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterViewModel {
        [StringLength(64)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 2)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
