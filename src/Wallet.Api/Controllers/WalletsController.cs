using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wallet.Api.Domain;
using Wallet.Api.Infrastructure.Http;
using Wallet.Api.Models;
using Wallet.Api.Models.Enums;
using ControllerBase = Wallet.Api.Infrastructure.Http.ControllerBase;

namespace Wallet.Api.Controllers;

[Route("api/v1/[controller]")]
public class WalletsController : ControllerBase
{
    private readonly IWalletManager manager;

    public WalletsController(IWalletManager manager)
    {
        this.manager = manager;
    }

    [SwaggerOperation("Create a wallet")]
    [HttpPost]
    [ProducesResponseType(typeof(WalletCreateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel<ErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWallet([FromBody] WalletCreateRequest request)
    {
        return MakeResponse(await manager.CreateWallet(request));
    }

    [SwaggerOperation("Deposit or withdraw from a wallet")]
    [HttpPost("{walletId:Guid}/process-transaction")]
    [ProducesResponseType(typeof(WalletModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel<ErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessTransaction([FromRoute] Guid walletId, [FromBody] TransactionRequest request)
    {
        return MakeResponse(await manager.ProcessTransaction(walletId, request));
    }

    [SwaggerOperation("Get a wallet")]
    [HttpGet("{walletId:Guid}")]
    [ProducesResponseType(typeof(WalletModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel<ErrorCode>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWallet([FromRoute] Guid walletId)
    {
        return MakeResponse(await manager.GetWallet(walletId));
    }
}