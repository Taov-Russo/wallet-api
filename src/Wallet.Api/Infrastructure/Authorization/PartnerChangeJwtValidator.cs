using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Wallet.Api.Infrastructure.Authorization;

public class PartnerChangeJwtValidator : ISecurityTokenValidator
{
    private readonly JwtSecurityTokenHandler tokenHandler;
    public bool CanValidateToken => true;
    public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

    public PartnerChangeJwtValidator()
    {
        tokenHandler = new JwtSecurityTokenHandler();
    }

    public bool CanReadToken(string securityToken)
    {
        return tokenHandler.CanReadToken(securityToken);
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        ClaimsPrincipal claims;
        try
        {
            claims = tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
        }
        catch (SecurityTokenExpiredException)
        {
            // Login with expired token
            validatedToken = new JwtSecurityToken();
            return new ClaimsPrincipal();
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "ValidateToken error");
            throw;
        }

        return claims;
    }
}