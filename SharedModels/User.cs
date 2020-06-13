using System;

namespace SharedModels
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string WalletName { get; set; }
        public string WalletAddress { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
