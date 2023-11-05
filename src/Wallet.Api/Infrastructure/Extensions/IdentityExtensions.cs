using System;
using System.Security.Claims;

namespace Wallet.Api.Infrastructure.Extensions;

public static class IdentityExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        if (Guid.TryParse(user.Identity.Name, out var guid))
        {
            return guid;
        }
        return null;
    }
}