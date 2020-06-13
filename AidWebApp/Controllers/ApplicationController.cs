using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AidWebApp.Models;
using ApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using SharedModels;
using Utility;
using Enums = Utility.Enums;

namespace AidWebApp.Controllers
{
    public class ApplicationController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IBlockchainApi _blockChainApi;
        private readonly IOptions<AppConfiguration> _appConfiguration;
        public ApplicationController(IUserService userService,
            IBlockchainApi blockChainApi,
            IOptions<AppConfiguration> appConfiguration)
        {
            _userService = userService;
            _blockChainApi = blockChainApi;
            _appConfiguration = appConfiguration;
        }

        [HttpGet]
        public IActionResult SubmitApplication()
        {
            var model = new ApplicationViewModel();
            TempData["ActionLoader"] = false;

            return PartialView("_ApplyModalPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication(ApplicationViewModel model)
        {
            if (!ModelState.IsValid)
                return View("_ApplyModalPartial", model);

            var applicationModel = new Application
            {
                UserId = User.GetUserId(),
                AverageIncome = model.AverageIncome,
                Category = (int)model.Category,
                Status = (int)Enums.ApplicationStatus.Pending,
                IdentificationNumber = model.IdentificationNumber
            };

            applicationModel.Id = await _userService.CreateApplication(applicationModel);

            if (applicationModel.Id.IsValidGuid())
            {
                var contract = new Contract
                {
                    WalletName = User.GetWalletName(),
                    WalletPassword = model.WalletPassword,
                    WalletAddress = User.GetWalletAddress()
                };

                var transaction = await _blockChainApi.SubmitApplication(contract, applicationModel, DateTime.UtcNow.ToUnixTimestamp());
                if (!transaction.Success)
                {
                    return JsonError(Constants.Error);
                }

                await Task.Delay(_appConfiguration.Value.AverageBlockTime);

                var receipt = await _blockChainApi.GetReceipt(transaction.TransactionId);
                if (receipt.Success)
                {
                    return JsonSuccess(Constants.ApplicationCreationSuccess);
                }
                return JsonError(receipt.Error);
            }

            return JsonError(Constants.ErrorDuringDatabaseOperation);
        }

        [HttpGet]
        public async Task<IActionResult> GetApprovedLogs()
        {
            var transactionLogs = await _blockChainApi.GetTransactionLogs();
            if (transactionLogs == null) throw new ArgumentNullException(nameof(transactionLogs));
            var modelList = new List<TransactionViewModel>();

            foreach (var transactionDetail in transactionLogs)
            {
                modelList.AddRange(transactionDetail.Logs
                    .Where(logDetail => logDetail.LogDetail.From == _appConfiguration.Value.ContractAddress)
                    .Select(logDetail => new TransactionViewModel
                    {
                        From = logDetail.LogDetail.From,
                        To = logDetail.LogDetail.To,
                        Amount = logDetail.LogDetail.Amount / Constants.SatoshiDecimal,
                        TransactionHash = transactionDetail.TransactionHash
                    }));
            }
            return View("/Views/Application/ApprovedLogs.cshtml", modelList);
        }
    }
}