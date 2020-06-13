using System;
using System.ComponentModel;

namespace SharedModels
{
    public class Application
    {
        public Guid Id { get; set; }

        public int Category { get; set; }

        public Guid UserId { get; set; }

        public string WalletAddress { get; set; }

        [DisplayName("User Name")]
        public string UserFullName { get; set; }

        public string IdentificationNumber { get; set; }

        public decimal AverageIncome { get; set; }

        public int Status { get; set; }
    }
}
