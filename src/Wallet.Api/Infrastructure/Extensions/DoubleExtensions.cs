using System.Runtime.CompilerServices;

namespace Wallet.Api.Infrastructure.Extensions;

public static class DoubleExtensions
{
    public static string Round(this double number)
    {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler.AppendFormatted<double>(number, "0.0");
        return interpolatedStringHandler.ToStringAndClear();
    }
}