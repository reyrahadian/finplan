using System.ComponentModel.DataAnnotations;

namespace FinPlan.Web.ViewModels
{
    public class AuthenticationFormViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]        
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}