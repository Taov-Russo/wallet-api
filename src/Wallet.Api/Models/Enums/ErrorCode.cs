namespace Wallet.Api.Models.Enums;

public enum ErrorCode
{
    WalletAlreadyExists = 1,
    WalletNotFound = 2,
    TransactionAlreadyExists = 3,
    InsufficientBalance = 4
}