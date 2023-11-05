using System;
using Wallet.Api.Infrastructure.Http;
using Wallet.Api.Models.Enums;

namespace Wallet.Api.Models;

public class TransactionModel : JsonModel
{
    public Guid TransactionId { get; set; }
    public Guid WalletId { get; set; }
    public float Amount { get; set; }
    public OperationType OperationType { get; set; }
}