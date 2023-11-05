using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Wallet.Api.Data;
using Wallet.Api.Domain;
using Wallet.Api.Domain.Validation;
using Wallet.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services
    .AddRouting(x => x.LowercaseUrls = true)
    .AddMvc()
    .AddFluentValidation();

var applicationName = builder.Environment.ApplicationName;

builder.Services
    .AddTransient<IWalletManager, WalletManager>()
    .AddTransient<IWalletRepository, WalletRepository>()
    .AddTransient<ITransactionRepository, TransactionRepository>()

    .AddTransient<IValidator<WalletCreateRequest>, WalletCreateValidator>()
    .AddTransient<IValidator<TransactionRequest>, WalletProcessTransactionValidator>();

if (builder.Configuration.GetValue<bool>("EnableSwagger"))
    builder.Services.AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc("v1", new OpenApiInfo { Title = applicationName, Version = "v1" });
    });

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

var app = builder.Build();

if (!app.Environment.IsProduction())
    app.UseDeveloperExceptionPage();

app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Lifetime.ApplicationStopped.Register(() =>
{
    Log.Logger.Information($"{applicationName} has been stopped");
});

if (builder.Configuration.GetValue<bool>("EnableSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./swagger/v1/swagger.json", $"{applicationName} v1");
        c.RoutePrefix = string.Empty;
    });
}

Log.Logger.Information($"{applicationName} has been started");
app.Run();