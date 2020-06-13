using System;
using System.Linq;
using System.Security.Claims;
using Utility;

namespace AidWebApp
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return Guid.Empty;

            ClaimsPrincipal currentUser = user;
            return Guid.Parse(currentUser.FindFirst(c => c.Type == Constants.UserIdType).Value);
        }

        public static string GetWalletAddress(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.Claims.First(c => c.Type == Constants.WalletAddressType).Value;
        }

        public static string GetWalletName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.Claims.First(c => c.Type == Constants.WalletNameType).Value;
        }

        public static string GetFirstName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.Claims.First(c => c.Type == Constants.FirstNameType).Value;
        }

        public static string GetLastName(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.Claims.First(c => c.Type == Constants.LastNameType).Value;
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            return $"{GetFirstName(user)} {GetLastName(user)}";
        }
    }
}
