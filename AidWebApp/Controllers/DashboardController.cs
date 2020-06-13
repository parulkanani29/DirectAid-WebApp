using System.Linq;
using System.Threading.Tasks;
using AidWebApp.Extensions;
using AidWebApp.Models;
using ApiClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Utility;

namespace AidWebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IBlockchainApi _blockChainApi;
        private readonly IUserService _userService;

        public DashboardController(IBlockchainApi blockChainApi, IUserService userService)
        {
            _blockChainApi = blockChainApi;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var applications = await _userService.GetApplicationsByUser(User.GetUserId().ToString());
            var model = new DashboardViewModel
            {
                Application = applications.ToList().ToListModel()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressBalance()
        {
            var walletAddress = User.GetWalletAddress();

            var balance = await _blockChainApi.AddressBalance(walletAddress);

            if (balance > 0)
                balance /= Constants.SatoshiDecimal;

            return Json(balance);
        }
    }
}