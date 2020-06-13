using System;
using System.ComponentModel.DataAnnotations;

namespace AidWebApp.Models
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        public int Role { get; set; }

        [Required(ErrorMessage = "Wallet name is required")]
        [Display(Name = "Wallet Name")]
        public string WalletName { get; set; }

        [Required(ErrorMessage = "Wallet address is required")]
        [Display(Name = "Wallet Address")]
        public string WalletAddress { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
    }
}
