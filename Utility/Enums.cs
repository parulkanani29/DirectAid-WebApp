using System.ComponentModel.DataAnnotations;

namespace Utility
{
    public class Enums
    {
        public enum ApplicationCategory
        {
            [Display(Name = "Small Business")]
            SmallBusiness = 1,

            [Display(Name = "Medium Business")]
            MediumBusiness = 2,

            [Display(Name = "Farmer")]
            Farmer = 3,

            [Display(Name = "Senior Citizen")]
            SeniorCitizen = 4,

            [Display(Name = "Health Worker")]
            HealthWorker = 5,

            [Display(Name = "DailyWage Worker")]
            DailyWageWorker = 6
        }

        public enum UserRole
        {
            Member = 1,
            Admin = 2
        }

        public enum ApplicationStatus
        {
            [Display(Name = "Pending")]
            Pending,

            [Display(Name = "Approved")]
            Approved,

            [Display(Name = "Rejected")]
            Rejected
        }
    }
}
