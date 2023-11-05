using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Wallet.Api.Models.DBContexts;

public class WalletContext : DbContext
{
    private readonly IConfiguration configuration;

    public WalletContext(DbContextOptions<WalletContext> options, IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }

    public DbSet<WalletModel> Wallet { get; set; }
    public DbSet<TransactionModel> Transaction { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WalletModel>(x =>
        {
            x.HasKey(t => t.WalletId);
            x.Property(t => t.Balance)
                .HasColumnType("decimal(9,2");
        });

        modelBuilder.Entity<TransactionModel>(x =>
        {
            x.HasKey(t => t.TransactionId);
            x.Property(t => t.Amount)
                .HasColumnType("decimal(9,2)");
        });
    }
}