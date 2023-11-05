using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Wallet.Api.Models;

namespace Wallet.Api.Data;

public interface ITransactionRepository
{
    Task<TransactionModel> GetTransaction(Guid transactionId);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly IConfiguration configuration;

    public TransactionRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private SqlConnection createConnection()
    {
        return new(configuration.GetConnectionString("Database"));
    }

    public async Task<TransactionModel> GetTransaction(Guid transactionId)
    {
        await using var connection = createConnection();
        return await connection.QueryFirstOrDefaultAsync<TransactionModel>(@"
            SELECT
                TransactionId,
                WalletId,
                Amount,
                OperationType
            FROM [Transaction]
            WHERE
                TransactionId = @TransactionId"
            , new
            {
                transactionId
            });
    }
}