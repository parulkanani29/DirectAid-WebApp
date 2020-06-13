namespace AidWebApp.Models
{
    public class TransactionViewModel
    {
        public string From { get; set; }
        public string To { get; set; }

        public long Amount { get; set; }

        public string TransactionHash { get; set; }
    }
}
