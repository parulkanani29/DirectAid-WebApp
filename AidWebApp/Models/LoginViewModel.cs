using System.ComponentModel.DataAnnotations;

namespace AidWebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Wallet name is required")]
        [Display(Name = "Wallet Name")]
        public string WalletName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }        
    }
}
