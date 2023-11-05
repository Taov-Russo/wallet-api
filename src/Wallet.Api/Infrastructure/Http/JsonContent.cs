using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Wallet.Api.Infrastructure.Http;

public class JsonContent : StringContent
{
    public JsonContent(object obj, JsonSerializerOptions options)
        : base(JsonSerializer.Serialize(obj, options), Encoding.UTF8, "application/json")
    {
    }
}