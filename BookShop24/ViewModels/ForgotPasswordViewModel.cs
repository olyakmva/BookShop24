using System.ComponentModel.DataAnnotations;

namespace BookShop24.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
