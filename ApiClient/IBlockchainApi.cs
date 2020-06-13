using System.Collections.Generic;
using System.Threading.Tasks;
using SharedModels;

namespace ApiClient
{
    public interface IBlockchainApi
    {
        Task<BlockchainResponse> LoadWallet(string walletName, string walletPassword);

        Task<BlockchainResponse> WalletAddresses(string walletName);

        Task<double> AddressBalance(string walletAddress);

        Task<TransactionResponse> SubmitApplication(Contract contract, Application application, double currentTime);

        Task<TransactionResponse> SetAmountForCategory(Contract contract, int category, ulong amount);

        Task<TransactionResponse> AddFundToContract(Contract contract);

        Task<TransactionResponse> ApproveApplication(Contract contract, string applicationId);

        Task<TransactionResponse> RejectApplication(Contract contract, string applicationId);

        Task<TransactionResponse> ClaimAid(Contract contract, string applicationId, string currentTime);

        Task<TransactionReceipt> GetReceipt(string txHash);

        Task<double> ContractBalance();

        Task<List<ReceiptSearch>> GetTransactionLogs();

    }
}
