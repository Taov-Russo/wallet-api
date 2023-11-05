using FluentValidation;
using Wallet.Api.Models;
using Wallet.Api.Models.Common;

namespace Wallet.Api.Domain.Validation;

public class WalletCreateValidator : AbstractValidator<WalletCreateRequest>
{
    public WalletCreateValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(ErrorMessages.INVALID_VALUE);
    }
}