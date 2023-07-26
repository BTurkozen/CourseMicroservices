using System.ComponentModel.DataAnnotations;

namespace Course.Web.Models
{
    public sealed class SignInInput
    {
        [Display(Name = "Email"),Required]
        public string Email { get; set; }
        
        [Display(Name = "Password"), Required]
        public string Password { get; set; }

        [Display(Name = "Remember"), Required]
        public bool IsRemember { get; set; }
    }
}
