using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AidWebApp.Extensions;
using AidWebApp.Models;
using ApiClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Services;
using SharedModels;
using Utility;
using static Utility.Enums;

namespace AidWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IBlockchainApi _blockChainApi;
        private readonly IUserService _userService;
        private readonly IToastNotification _toastNotification;

        public UserController(IBlockchainApi blockChainApi,
            IUserService userService,
            IToastNotification toastNotification)
        {
            _blockChainApi = blockChainApi;
            _userService = userService;
            _toastNotification = toastNotification;
        }

        [NonAction]
        private async Task SignInUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, string.Empty),
                new Claim(Constants.UserIdType, Convert.ToString(user.UserId)),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(Constants.FirstNameType, user.FirstName),
                new Claim(Constants.LastNameType,user.LastName),
                new Claim(Constants.WalletAddressType,user.WalletAddress),
                new Claim(Constants.WalletNameType,user.WalletName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IssuedUtc = DateTime.UtcNow,
                IsPersistent = false,
                AllowRefresh = false
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated) return View();

            if (User.IsInRole(role: Convert.ToString((int)UserRole.Admin)))
            {
                return RedirectToAction("AdminDashBoard", "Admin", routeValues: new { area = $"admin" });
            }

            return RedirectToAction("Index", "Dashboard");

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var loginResponse = await _blockChainApi.LoadWallet(model.WalletName, model.Password);
            if (loginResponse.Success)
            {
                var user = await _userService.GetUserByWallet(model.WalletName);
                if (user == null)
                {
                    return RedirectToAction("Register", new { walletName = model.WalletName });
                }

                await SignInUserAsync(user);

                if (user.Role == (int)UserRole.Admin)
                {
                    return RedirectToAction("AdminDashBoard", "Admin", routeValues: new { area = $"admin" });
                }

                return RedirectToAction("Index", "Dashboard");
            }

            _toastNotification.AddErrorToastMessage(loginResponse.Message);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register(string walletName)
        {
            var response = await _blockChainApi.WalletAddresses(walletName);

            if (response.Success)
            {
                var addressList = ((IEnumerable)response.Data).Cast<object>().ToList();
                var selectListItems = addressList.Select(address => new SelectListItem { Text = address.ToString(), Value = address.ToString() }).ToList();
                ViewBag.WalletAddresses = selectListItems;
            }
            var user = new User
            {
                WalletName = walletName,
                BirthDate = DateTime.Now.AddYears(-16)
            };

            return View(user.ToModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            user.Role = (int)UserRole.Member;

            var userId = await _userService.Register(user);
            if (userId.IsValidGuid())
                return RedirectToAction("Login");

            return View(user.ToModel());
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("login");
        }
    }
}