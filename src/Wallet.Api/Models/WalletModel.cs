using System;
using Wallet.Api.Infrastructure.Http;

namespace Wallet.Api.Models;

public class WalletModel : JsonModel
{
    public Guid WalletId { get; set; }
    public Guid UserId { get; set; }
    public float Balance { get; set; }
}