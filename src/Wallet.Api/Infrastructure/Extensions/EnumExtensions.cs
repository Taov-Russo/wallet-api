using System;
using System.ComponentModel;

namespace Wallet.Api.Infrastructure.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum en)
    {
        if (en == null)
            return string.Empty;
        var member = en.GetType().GetMember(en.ToString());
        if (member.Length != 0)
        {
            object[] customAttributes = member[0].GetCustomAttributes(typeof (DescriptionAttribute), false);
            if (customAttributes.Length != 0)
                return ((DescriptionAttribute) customAttributes[0]).Description;
        }
        return en.ToString();
    }
}