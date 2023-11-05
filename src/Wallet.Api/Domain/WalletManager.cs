using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wallet.Api.Data;
using Wallet.Api.Infrastructure.Extensions;
using Wallet.Api.Infrastructure.Http;
using Wallet.Api.Models;
using Wallet.Api.Models.Enums;

namespace Wallet.Api.Domain;

public interface IWalletManager
{
    Task<Response<WalletCreateResponse, ErrorModel<ErrorCode>>> CreateWallet(WalletCreateRequest request);
    Task<Response<EmptyModel, ErrorModel<ErrorCode>>> ProcessTransaction(Guid walletId, TransactionRequest request);
    Task<Response<WalletModel, ErrorModel<ErrorCode>>> GetWallet(Guid walletId);
}

public class WalletManager : IWalletManager
{
    private readonly ILogger<WalletManager> logger;
    private readonly IWalletRepository walletRepository;
    private readonly ITransactionRepository transactionRepository;

    public WalletManager(ILogger<WalletManager> logger, IWalletRepository walletRepository, ITransactionRepository transactionRepository)
    {
        this.logger = logger;
        this.walletRepository = walletRepository;
        this.transactionRepository = transactionRepository;
    }

    public async Task<Response<WalletCreateResponse, ErrorModel<ErrorCode>>> CreateWallet(WalletCreateRequest request)
    {
        var beginTime = DateTime.Now;
        try
        {
            var wallet = await walletRepository.GetWalletByUserId(request.UserId);
            if (wallet is not null)
            {
                logger.LogDebug($"{Method.GetName()} fail. Wallet already exists. Duration: {beginTime.GetDurationMilliseconds()} {request.ToLogString()}");
                return Response<WalletCreateResponse, ErrorModel<ErrorCode>>.BadRequest(new ErrorModel<ErrorCode>(ErrorCode.WalletAlreadyExists));
            }

            var walletId = Guid.NewGuid();
            var rowsCount = await walletRepository.CreateWallet(walletId, request.UserId);
            if (rowsCount == 0)
            {
                logger.LogError($"{Method.GetName()} error. {rowsCount} rows inserted. Duration: {beginTime.GetDurationMilliseconds()} {request.ToLogString()}");
                return Response<WalletCreateResponse, ErrorModel<ErrorCode>>.InternalServerError();
            }

            logger.LogInformation($"{Method.GetName()} success. Duration: {beginTime.GetDurationMilliseconds()} {request.ToLogString()}");
            return Response<WalletCreateResponse, ErrorModel<ErrorCode>>.Ok(new WalletCreateResponse
            {
                WalletId = walletId
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{Method.GetName()} error. Duration: {beginTime.GetDurationMilliseconds()} {request.ToLogString()}");
            return Response<WalletCreateResponse, ErrorModel<ErrorCode>>.InternalServerError();
        }
    }

    public async Task<Response<EmptyModel, ErrorModel<ErrorCode>>> ProcessTransaction(Guid walletId, TransactionRequest request)
    {
        var beginTime = DateTime.Now;
        try
        {
            var wallet = await walletRepository.GetWallet(walletId);
            if (wallet is null)
            {
                logger.LogDebug($"{Method.GetName()} fail. Wallet not found. Duration: {beginTime.GetDurationMilliseconds()} WalletId: {walletId}, {request.ToLogString()}");
                return Response<EmptyModel, ErrorModel<ErrorCode>>.BadRequest(new ErrorModel<ErrorCode>(ErrorCode.WalletNotFound));
            }

            if (request.OperationType == OperationType.Withdrawal && wallet.Balance - request.Amount < 0)
            {
                logger.LogDebug($"{Method.GetName()} fail. Insufficient balance. Duration: {beginTime.GetDurationMilliseconds()} WalletId: {walletId}, {request.ToLogString()}");
                return Response<EmptyModel, ErrorModel<ErrorCode>>.BadRequest(new ErrorModel<ErrorCode>(ErrorCode.InsufficientBalance));
            }

            var transaction = await transactionRepository.GetTransaction(request.TransactionId);
            if (transaction is not null)
            {
                logger.LogDebug($"{Method.GetName()} fail. Transaction already exists. Duration: {beginTime.GetDurationMilliseconds()} WalletId: {walletId}, {request.ToLogString()}");
                return Response<EmptyModel, ErrorModel<ErrorCode>>.BadRequest(new ErrorModel<ErrorCode>(ErrorCode.TransactionAlreadyExists));
            }

            await walletRepository.UpdateWalletBalance(walletId, request);

            logger.LogInformation($"{Method.GetName()} success. Duration: {beginTime.GetDurationMilliseconds()} WalletId: {walletId}, {request.ToLogString()}");
            return Response<EmptyModel, ErrorModel<ErrorCode>>.Ok(new EmptyModel());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{Method.GetName()} error. Duration: {beginTime.GetDurationMilliseconds()} WalletId: {walletId}, {request.ToLogString()}");
            return Response<EmptyModel, ErrorModel<ErrorCode>>.InternalServerError();
        }
    }

    public async Task<Response<WalletModel, ErrorModel<ErrorCode>>> GetWallet(Guid walletId)
    {
        var beginTime = DateTime.Now;
        try
        {
            var wallet = await walletRepository.GetWallet(walletId);
            if (wallet is not null)
                return Response<WalletModel, ErrorModel<ErrorCode>>.Ok(wallet);

            logger.LogInformation($"{Method.GetName()} fail. Wallet not found. Duration: {beginTime.GetDurationMilliseconds()} WalletId: {walletId}");
            return Response<WalletModel, ErrorModel<ErrorCode>>.BadRequest(new ErrorModel<ErrorCode>(ErrorCode.WalletNotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{Method.GetName()} error. Duration: {beginTime.GetDurationMilliseconds()} WalletId: {walletId}");
            return Response<WalletModel, ErrorModel<ErrorCode>>.InternalServerError();
        }
    }
}