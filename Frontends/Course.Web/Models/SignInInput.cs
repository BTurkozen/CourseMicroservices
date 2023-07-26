using System.ComponentModel.DataAnnotations;

namespace Course.Web.Models
{
    public sealed class SignInInput
    {
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember")]
        public bool IsRemember { get; set; }
    }
}
