using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedModels;
using Utility;

namespace ApiClient
{

    public class BlockchainApi : IBlockchainApi
    {
        private readonly IOptions<AppConfiguration> _appConfiguration;

        public BlockchainApi(IOptions<AppConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public async Task<BlockchainResponse> LoadWallet(string walletName, string walletPassword)
        {
            var requestBody = new
            {
                name = walletName,
                password = walletPassword
            };

            var queryParams = new Dictionary<string, string>
            {
                {"IsCancellationRequested", "true"},
                {"CanBeCanceled", "true"},
                {"WaitHandle.Handle", "true"},
                {"WaitHandle.SafeWaitHandle.IsInvalid", "true"},
                {"WaitHandle.SafeWaitHandle.IsClosed", "true"}
            };

            using (var client = new BaseClient(_appConfiguration).WalletCall())
            {
                var url = QueryHelpers.AddQueryString("load", queryParams);

                var response = await client.PostAsync(url, CreateJsonContent(requestBody));
                if (response.IsSuccessStatusCode)
                {
                    return new BlockchainResponse { Success = true };
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<BlockchainError>(responseString);
                return new BlockchainResponse { Success = false, Message = errors.errors.FirstOrDefault()?.message };
            }
        }

        public async Task<BlockchainResponse> WalletAddresses(string walletName)
        {
            var queryParams = new Dictionary<string, string> { { "walletName", walletName } };

            using (var client = new BaseClient(_appConfiguration).SmartContractWalletCall())
            {
                var url = QueryHelpers.AddQueryString("account-addresses", queryParams);

                var response = await client.GetAsync(url);

                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var addresses = JsonConvert.DeserializeObject<List<string>>(responseContent);
                    return new BlockchainResponse { Success = true, Data = addresses };
                }

                var errors = JsonConvert.DeserializeObject<BlockchainError>(responseContent);
                return new BlockchainResponse { Success = false, Message = errors.errors.FirstOrDefault()?.message };
            }
        }

        public async Task<double> AddressBalance(string walletAddress)
        {
            var queryParams = new Dictionary<string, string> { { "address", walletAddress } };

            using (var client = new BaseClient(_appConfiguration).SmartContractWalletCall())
            {
                var url = QueryHelpers.AddQueryString("address-balance", queryParams);

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<double>(content);

                    return responseData;
                }

                return 0;
            }
        }

        public async Task<TransactionResponse> AddFundToContract(Contract contract)
        {
            using (var client = new BaseClient(_appConfiguration).ContractCall(contract))
            {
                var response = await client.PostAsync("AddFundToContract", CreateJsonContent(new object()));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TransactionResponse>(content);

                }
                var errorMessage = response.RequestMessage.ToString();
                return new TransactionResponse { Success = false, Message = errorMessage };
            }
        }

        public async Task<TransactionResponse> SetAmountForCategory(Contract contract, int category, ulong amount)
        {
            var requestBody = new
            {
                categoryType = category,
                amount = amount
            };

            using (var client = new BaseClient(_appConfiguration).ContractCall(contract))
            {
                var response = await client.PostAsync("SetAmountForCategory", CreateJsonContent(requestBody));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TransactionResponse>(content);
                }
                var errorMessage = response.RequestMessage.ToString();
                return new TransactionResponse { Success = false, Message = errorMessage };
            }
        }

        public async Task<TransactionResponse> ApproveApplication(Contract contract, string applicationId)
        {
            var requestBody = new
            {
                applicationId = applicationId
            };

            using (var client = new BaseClient(_appConfiguration).ContractCall(contract))
            {
                var response = await client.PostAsync("Approve", CreateJsonContent(requestBody));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TransactionResponse>(content);
                }
                var errorMessage = response.RequestMessage.ToString();
                return new TransactionResponse { Success = false, Message = errorMessage };
            }
        }

        public async Task<TransactionResponse> RejectApplication(Contract contract, string applicationId)
        {
            var requestBody = new
            {
                applicationId = applicationId
            };

            using (var client = new BaseClient(_appConfiguration).ContractCall(contract))
            {
                var response = await client.PostAsync("Reject", CreateJsonContent(requestBody));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TransactionResponse>(content);
                }
                var errorMessage = response.RequestMessage.ToString();
                return new TransactionResponse { Success = false, Message = errorMessage };
            }
        }

        public async Task<double> ContractBalance()
        {
            using (var client = new HttpClient())
            {
                var queryParams = new Dictionary<string, string> { { "address", _appConfiguration.Value.ContractAddress } };

                var url = QueryHelpers.AddQueryString($"{_appConfiguration.Value.BaseUrl}/SmartContracts/balance", queryParams);

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<double>(content);
                }

                return 0;
            }
        }

        public async Task<TransactionResponse> SubmitApplication(Contract contract, Application application, double currentTime)
        {
            try
            {
                var requestBody = new
                {
                    applicationId = application.Id,
                    category = application.Category,
                    currentTime = currentTime.ToString()
                };

                using (var client = new BaseClient(_appConfiguration).ContractCall(contract))
                {
                    var response = await client.PostAsync("SubmitApplication", CreateJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<TransactionResponse>(content);
                        return result;
                    }
                    var errorMessage = response.RequestMessage.ToString();
                    return new TransactionResponse { Success = false, Message = errorMessage };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<TransactionResponse> ClaimAid(Contract contract, string applicationId, string currentTime)
        {
            var requestBody = new
            {
                applicationId = applicationId,
                currentTime = currentTime
            };

            using (var client = new BaseClient(_appConfiguration).ContractCall(contract))
            {
                var response = await client.PostAsync("ClaimAid", CreateJsonContent(requestBody));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TransactionResponse>(content);
                }

                var errorMessage = response.RequestMessage.ToString();
                return new TransactionResponse { Success = false, Message = errorMessage };
            }
        }

        public async Task<TransactionReceipt> GetReceipt(string txHash)
        {
            try
            {
                using (var client = new BaseClient(_appConfiguration).ReceiptCall())
                {
                    var response = await client.GetAsync($"{client.BaseAddress}?txHash={txHash}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<TransactionReceipt>(content);
                        return result;
                    }
                    return new TransactionReceipt { Success = false, Error = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionReceipt { Success = false, Error = ex.Message };
            }
        }

        private static StringContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        public async Task<List<ReceiptSearch>> GetTransactionLogs()
        {
            using (var client = new HttpClient())
            {
                var queryParams = new Dictionary<string, string>
                {
                    {
                        "contractAddress", _appConfiguration.Value.ContractAddress
                    },
                    {
                        "eventName", "TransactionLog"
                    }
                };

                var url = QueryHelpers.AddQueryString($"{_appConfiguration.Value.BaseUrl}/SmartContracts/receipt-search", queryParams);

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ReceiptSearch>>(content);
                }

                return new List<ReceiptSearch>();
            }
        }
    }
}
