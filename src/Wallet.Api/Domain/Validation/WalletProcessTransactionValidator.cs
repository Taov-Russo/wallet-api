using System;
using FluentValidation;
using Wallet.Api.Models;
using Wallet.Api.Models.Common;
using Wallet.Api.Models.Enums;

namespace Wallet.Api.Domain.Validation;

public class WalletProcessTransactionValidator : AbstractValidator<TransactionRequest>
{
    public WalletProcessTransactionValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .WithMessage(ErrorMessages.INVALID_VALUE);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(ErrorMessages.WRONG_AMOUNT);

        RuleFor(x => x.OperationType)
            .Must(operationType => operationType is OperationType.Deposit or OperationType.Withdrawal)
            .WithMessage(ErrorMessages.INVALID_VALUE);
    }
}