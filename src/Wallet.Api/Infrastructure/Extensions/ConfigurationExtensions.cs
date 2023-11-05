using System;
using Microsoft.Extensions.Configuration;

namespace Wallet.Api.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static T BindFromAppConfig<T>(this IConfiguration configuration)
    {
        Type type = typeof (T);
        T instance = (T) Activator.CreateInstance(type);
        configuration.Bind(type.Name, instance);
        return instance;
    }

    public static T BindFromAppConfig<T>(this IConfiguration configuration, string configPath)
    {
        T instance = (T) Activator.CreateInstance(typeof (T));
        configuration.Bind(configPath, instance);
        return instance;
    }
}