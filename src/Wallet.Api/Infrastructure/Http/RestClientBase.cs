using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wallet.Api.Infrastructure.Http;

public class RestClientBase
{
    protected readonly HttpClient HttpClient;
    private readonly Uri baseUri;
    private readonly JsonSerializerOptions options = new ()
    {
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RestClientBase(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public RestClientBase(HttpClient httpClient, string baseUri)
    {
        this.baseUri = new Uri(baseUri);
        HttpClient = httpClient;
    }

    public async Task<Response> Post(string uri, object obj)
    {
        Uri requestUri = getRequestUri(uri);
        JsonContent content = new JsonContent(obj, options);
        HttpResponseMessage response = await HttpClient.PostAsync(requestUri, content);
        Response response1 = await createResponse(response);
        return response1;
    }

    private async Task<Response> createResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return new Response()
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode
            };
        Response response1 = new Response();
        response1.IsSuccessStatusCode = response.IsSuccessStatusCode;
        response1.StatusCode = response.StatusCode;
        return response1;
    }

    private Uri getRequestUri(string uri) => baseUri == null ? new Uri(uri) : new Uri(baseUri, uri);
}