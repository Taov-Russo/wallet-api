using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Wallet.Api.Models;
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
    private readonly IConfiguration configuration;

    public WalletRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<int> CreateWallet(Guid walletId, Guid userId)
    {
        await using var connection = createConnection();
        return await connection.ExecuteAsync(@"
            INSERT INTO Wallet
            (
                WalletId,
                UserId,
                Balance
            )
            VALUES
            (
                @WalletId,
                @UserId,
                0
            )"
            , new
            {
                walletId,
                userId
            });
    }

    public async Task<WalletModel> GetWallet(Guid walletId)
    {
        await using var connection = createConnection();
        return await connection.QueryFirstOrDefaultAsync<WalletModel>(@"
            SELECT
                WalletId,
                UserId,
                Balance
            FROM Wallet
            WHERE
                WalletId = @WalletId"
            , new
            {
                walletId
            });
    }

    public async Task<WalletModel> GetWalletByUserId(Guid userId)
    {
        await using var connection = createConnection();
        return await connection.QueryFirstOrDefaultAsync<WalletModel>(@"
            SELECT
                WalletId,
                UserId,
                Balance
            FROM Wallet
            WHERE
                UserId = @UserId"
            , new
            {
                userId
            });
    }

    public async Task UpdateWalletBalance(Guid walletId, TransactionRequest request)
    {
        await using var connection = createConnection();
        await connection.OpenAsync();
        await using var tx = connection.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            var dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("TransationId", request.TransactionId);
            dynamicParameters.Add("WalletId", walletId);
            dynamicParameters.Add("Amount", request.Amount);
            dynamicParameters.Add("OperationType", request.OperationType);

            await connection.ExecuteAsync($@"
                UPDATE Wallet
                SET
                    Balance = Balance + IIF(@OperationType = {OperationType.Deposit:D}, @Amount, -@Amount)
                WHERE
                    WalletId = @WalletId", dynamicParameters, tx);

            await connection.ExecuteAsync(@"
                INSERT INTO [Transaction]
                (
                    TransactionId,
                    WalletId,
                    Amount,
                    OperationType
                )
                VALUES
                (
                    @TransationId,
                    @WalletId,
                    @Amount,
                    @OperationType
                )", dynamicParameters, tx);

            tx.Commit();
        }
        catch (Exception ex)
        {
            tx.Rollback();
            throw new Exception("Failed to update balance.", ex);;
        }
    }

    private SqlConnection createConnection()
    {
        return new(configuration.GetConnectionString("Database"));
    }
}