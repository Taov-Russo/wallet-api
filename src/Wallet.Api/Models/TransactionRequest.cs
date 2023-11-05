using System;
using Wallet.Api.Infrastructure.Http;
using Wallet.Api.Models.Enums;

namespace Wallet.Api.Models;

public class TransactionRequest : JsonModel
{
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
    public OperationType OperationType { get; set; }
}