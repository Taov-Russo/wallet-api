using System;
using Wallet.Api.Infrastructure.Http;

namespace Wallet.Api.Models;

public class WalletCreateRequest : JsonModel
{
    public Guid UserId { get; set; }
}