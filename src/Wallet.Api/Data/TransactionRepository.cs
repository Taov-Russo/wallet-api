using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Models;
using Wallet.Api.Models.DBContexts;

namespace Wallet.Api.Data;

public interface ITransactionRepository
{
    Task<TransactionModel> GetTransaction(Guid transactionId);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly WalletContext context;

    public TransactionRepository(WalletContext context)
    {
        this.context = context;
    }

    public async Task<TransactionModel> GetTransaction(Guid transactionId)
    {
        return await context.Transaction
            .Where(t => t.TransactionId == transactionId)
            .FirstOrDefaultAsync();
    }
}