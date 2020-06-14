using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Utility.Enums;

namespace AidWebApp.Models
{
    public class ApplicationViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public ApplicationCategory Category { get; set; }

        [Required]
        public string WalletPassword { get; set; }

        [DisplayName("Applicant Address")]
        public string WalletAddress { get; set; }

        [Required]
        [DisplayName("Average Income")]        
        public long AverageIncome { get; set; }

        [DisplayName("Applicant Name")]
        public string UserFullName { get; set; }

        [Required]
        [DisplayName("Identification Number (EIN/SSN/PAN)")]
        public string IdentificationNumber { get; set; }

        [DisplayName("Status")]
        public ApplicationStatus Status { get; set; }
    }
}
