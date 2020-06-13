using System.Linq;
using System.Threading.Tasks;
using AidWebApp.Extensions;
using ApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using Services;
using SharedModels;
using Utility;
using Enums = Utility.Enums;

namespace AidWebApp.Admin.Controllers
{
    [Area("admin")]
    [Route("[controller]")]
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IBlockchainApi _blockChainApi;
        private readonly IOptions<AppConfiguration> _appConfiguration;
        private readonly IToastNotification _toastNotification;

        public AdminController(IUserService userService, IBlockchainApi blockChainApi,
            IOptions<AppConfiguration> appConfiguration,
            IToastNotification toastNotification)
        {
            _userService = userService;
            _blockChainApi = blockChainApi;
            _appConfiguration = appConfiguration;
            _toastNotification = toastNotification;

        }

        [HttpGet]
        public async Task<IActionResult> AdminDashBoard()
        {
            var applications = await _userService.GetAllApplications();
            return View("/Admin/Views/Index.cshtml", applications.ToList().ToListModel());
        }


        [HttpGet("GetContractBalance")]
        public async Task<IActionResult> GetContractBalance()
        {
            return Json(await _blockChainApi.ContractBalance());
        }

        [HttpPost("SetCategoryAmount")]
        public async Task<IActionResult> SetCategoryAmount(CategoryAmount model)
        {
            var contract = new Contract
            {
                WalletName = User.GetWalletName(),
                WalletAddress = User.GetWalletAddress(),
                WalletPassword = model.WalletPassword
            };

            var transaction = await _blockChainApi.SetAmountForCategory(contract, model.Category, model.Amount);
            if (transaction.Success)
            {
                for (var i = 0; i < 2; i++)
                {
                    await Task.Delay(_appConfiguration.Value.AverageBlockTime);
                    var receipt = await _blockChainApi.GetReceipt(transaction.TransactionId);
                    if (receipt.Success)
                        break;

                    if (!string.IsNullOrEmpty(receipt.Error))
                        return JsonError(receipt.Error);
                }

                return JsonSuccess(Constants.CategoryAmountSuccess);
            }

            return JsonError(Constants.Error);
        }

        [Route("AddFundToContract")]
        public async Task<IActionResult> AddFundToContract(long fundAmount, string walletPassword)
        {
            var contract = new Contract
            {
                WalletName = User.GetWalletName(),
                WalletAddress = User.GetWalletAddress(),
                WalletPassword = walletPassword,
                Amount = fundAmount.ToString()
            };

            var transaction = await _blockChainApi.AddFundToContract(contract);
            if (transaction.Success)
            {
                for (var i = 0; i < 2; i++)
                {
                    await Task.Delay(_appConfiguration.Value.AverageBlockTime);
                    var receipt = await _blockChainApi.GetReceipt(transaction.TransactionId);
                    if (receipt.Success)
                        break;

                    if (!string.IsNullOrEmpty(receipt.Error))
                        return JsonError(receipt.Error);
                }

                return JsonSuccess(Constants.ContractFundSuccess);
            }

            return JsonError(Constants.Error);
        }

        [Route("Approve")]
        public async Task<IActionResult> Approve(string id)
        {
            var contract = new Contract
            {
                WalletName = User.GetWalletName(),
                WalletAddress = User.GetWalletAddress(),
                WalletPassword = _appConfiguration.Value.WalletPassword
            };

            var transaction = await _blockChainApi.ApproveApplication(contract, id);
            if (transaction.Success)
            {
                for (var i = 0; i < 2; i++)
                {
                    await Task.Delay(_appConfiguration.Value.AverageBlockTime);
                    var receipt = await _blockChainApi.GetReceipt(transaction.TransactionId);
                    if (receipt.Success)
                    {
                        await _userService.UpdateApplicationStatus(id, (int)Enums.ApplicationStatus.Approved);
                        break;
                    }

                    if (!string.IsNullOrEmpty(receipt.Error))
                    {
                        _toastNotification.AddErrorToastMessage(receipt.Error);
                        return View("/Admin/Views/Index.cshtml");
                    }
                }
                _toastNotification.AddSuccessToastMessage(Constants.ApprovedSuccess);
                return RedirectToAction("AdminDashBoard");
            }

            _toastNotification.AddErrorToastMessage(Constants.Error);
            return View("/Admin/Views/Index.cshtml");
        }

        [Route("Reject")]
        public async Task<IActionResult> Reject(string id)
        {
            var contract = new Contract
            {
                WalletName = User.GetWalletName(),
                WalletAddress = User.GetWalletAddress(),
                WalletPassword = _appConfiguration.Value.WalletPassword
            };

            var transaction = await _blockChainApi.RejectApplication(contract, id);
            if (transaction.Success)
            {
                for (var i = 0; i < 2; i++)
                {
                    await Task.Delay(_appConfiguration.Value.AverageBlockTime);
                    var receipt = await _blockChainApi.GetReceipt(transaction.TransactionId);
                    if (receipt.Success)
                    {
                        await _userService.UpdateApplicationStatus(id, (int)Enums.ApplicationStatus.Rejected);
                        break;
                    }

                    if (!string.IsNullOrEmpty(receipt.Error))
                    {
                        _toastNotification.AddErrorToastMessage(receipt.Error);
                        return View("/Admin/Views/Index.cshtml");
                    }
                }
                _toastNotification.AddSuccessToastMessage(Constants.RejectedSuccess);
                return RedirectToAction("AdminDashBoard");
            }

            _toastNotification.AddErrorToastMessage(Constants.Error);
            return View("/Admin/Views/Index.cshtml");
        }
    }
}