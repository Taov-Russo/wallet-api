using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Wallet.Api.Infrastructure.Http;

public class JsonModel
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, GetType(), new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        });
    }

    public string ToLogString()
    {
        return $"{GetType().Name}: {this}";
    }
}