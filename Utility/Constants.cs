namespace Utility
{
    public static class Constants
    {
        public const string UserIdType = "AidWeb:UserId";
        public const string WalletAddressType = "AidWeb:WalletAdress";
        public const string FirstNameType = "AidWeb:FirstName";
        public const string LastNameType = "AidWeb:LastName";
        public const string WalletNameType = "AidWeb:WalletName";
        public const string WalletPasswordType = "AidWeb:WalletPassword";

        public const int SatoshiDecimal = 100_000_000;

        //Messages
        public const string Error = "Error occurred!";
        public const string ErrorDuringDatabaseOperation = "Error occurred while performing the request!";

        public const string ApplicationCreationSuccess = "You have successfully applied!";
        public const string ClaimSuccess = "You have successfully claimed! Your balance will update soon";
        
        public const string CategoryAmountSuccess = "You have successfully set an amount for a category";
        public const string ContractFundSuccess = "Contract balance has been updated!";

        public const string ApprovedSuccess = "Application has been Approved!";
        public const string RejectedSuccess = "Application has been Rejected!";

    }
}
