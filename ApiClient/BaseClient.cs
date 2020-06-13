using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using SharedModels;
using Utility;

namespace ApiClient
{
    public class BaseClient
    {
        private readonly IOptions<AppConfiguration> _appConfiguration;

        public BaseClient(IOptions<AppConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public HttpClient WalletCall()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{_appConfiguration.Value.BaseUrl}/Wallet/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public HttpClient SmartContractWalletCall()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{_appConfiguration.Value.BaseUrl}/SmartContractWallet/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public HttpClient ContractCall(Contract contract)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{_appConfiguration.Value.BaseUrl}/contract/{_appConfiguration.Value.ContractAddress}/method/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("GasPrice", _appConfiguration.Value.GasPrice);
            client.DefaultRequestHeaders.Add("GasLimit", _appConfiguration.Value.GasLimit);
            client.DefaultRequestHeaders.Add("Amount", string.IsNullOrEmpty(contract.Amount) ? _appConfiguration.Value.Amount : contract.Amount);
            client.DefaultRequestHeaders.Add("FeeAmount", _appConfiguration.Value.FeeAmount);
            client.DefaultRequestHeaders.Add("WalletName", contract.WalletName);
            client.DefaultRequestHeaders.Add("WalletPassword", contract.WalletPassword);
            client.DefaultRequestHeaders.Add("Sender", contract.WalletAddress);

            return client;
        }

        public HttpClient LocalCall()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{_appConfiguration.Value.BaseUrl}/SmartContracts/local-call");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public HttpClient ReceiptCall()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{_appConfiguration.Value.BaseUrl}/SmartContracts/receipt/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
