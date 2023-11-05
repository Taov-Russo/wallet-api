using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Models;
using Wallet.Api.Models.DBContexts;
using Wallet.Api.Models.Enums;

namespace Wallet.Api.Data;

public interface IWalletRepository
{
    Task<int> CreateWallet(Guid walletId, Guid userId);
    Task<WalletModel> GetWallet(Guid walletId);
    Task<WalletModel> GetWalletByUserId(Guid userId);
    Task UpdateWalletBalance(Guid walletId, TransactionRequest request);
}

public class WalletRepository : IWalletRepository
{
    private readonly WalletContext context;

    public WalletRepository(WalletContext context)
    {
        this.context = context;
    }

    public async Task<int> CreateWallet(Guid walletId, Guid userId)
    {
        var wallet = new WalletModel
        {
            WalletId = walletId,
            UserId = userId,
            Balance = 0
        };

        context.Wallet.Add(wallet);
        return await context.SaveChangesAsync();
    }

    public async Task<WalletModel> GetWallet(Guid walletId)
    {
        return await context.Wallet
            .Where(w => w.WalletId == walletId)
            .FirstOrDefaultAsync();
    }

    public async Task<WalletModel> GetWalletByUserId(Guid userId)
    {
        return await context.Wallet
            .Where(w => w.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateWalletBalance(Guid walletId, TransactionRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var wallet = await context.Wallet.FirstOrDefaultAsync(w => w.WalletId == walletId);
            if (wallet is null)
                throw new Exception("Wallet not found.");

            wallet.Balance += request.OperationType == OperationType.Deposit
                ? request.Amount
                : -request.Amount;

            var transactionEntity = new TransactionModel
            {
                TransactionId = request.TransactionId,
                WalletId = walletId,
                Amount = request.Amount,
                OperationType = request.OperationType
            };

            context.Transaction.Add(transactionEntity);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Failed to update balance.", ex);
        }
    }
}