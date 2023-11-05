using Microsoft.AspNetCore.Mvc;

namespace Wallet.Api.Infrastructure.Http;

[ApiController]
public class ControllerBase : Controller
{
    protected IActionResult MakeResponse(Response result)
        => !result.IsSuccessStatusCode
            ? (IActionResult) StatusCode((int) result.StatusCode)
            : Ok();

    protected IActionResult MakeResponse<T>(Response<T> result)
        => !result.IsSuccessStatusCode
            ? StatusCode((int) result.StatusCode)
            : Ok(result.Content);

    protected IActionResult MakeResponse<T, TBadRequestModel>(Response<T, TBadRequestModel> result)
        => !result.IsSuccessStatusCode
            ? StatusCode((int) result.StatusCode, result.ErrorModel)
            : Ok(result.Content);
}