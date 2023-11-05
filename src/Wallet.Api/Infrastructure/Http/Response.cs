using System.Net;
using System.Text.Json;

namespace Wallet.Api.Infrastructure.Http;

public class Response
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccessStatusCode { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Response Ok()
    {
        return new Response
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccessStatusCode = true
        };
    }

    public static Response NotFound()
    {
        return new Response
        {
            StatusCode = HttpStatusCode.NotFound,
            IsSuccessStatusCode = false
        };
    }

    public static Response InternalServerError()
    {
        return new Response
        {
            StatusCode = HttpStatusCode.InternalServerError,
            IsSuccessStatusCode = false
        };
    }
}

public class Response<T> : Response
{
    public T Content { get; set; }

    public static Response<T> Ok(T content)
    {
        return new Response<T>
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccessStatusCode = true,
            Content = content
        };
    }

    public new static Response<T> Ok()
    {
        return new Response<T>
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccessStatusCode = true
        };
    }

    public new static Response<T> BadRequest()
    {
        return new Response<T>
        {
            StatusCode = HttpStatusCode.BadRequest,
            IsSuccessStatusCode = false
        };
    }

    public new static Response<T> Forbidden()
    {
        return new Response<T>
        {
            StatusCode = HttpStatusCode.Forbidden,
            IsSuccessStatusCode = false
        };
    }

    public new static Response<T> NotFound()
    {
        return new Response<T>
        {
            StatusCode = HttpStatusCode.NotFound,
            IsSuccessStatusCode = false
        };
    }

    public new static Response<T> InternalServerError()
    {
        return new Response<T>
        {
            StatusCode = HttpStatusCode.InternalServerError,
            IsSuccessStatusCode = false
        };
    }
}

public class Response<TSuccessModel, TBadRequestErrorModel> : Response<TSuccessModel>
{
    public TBadRequestErrorModel ErrorModel { get; set; }

    public new static Response<TSuccessModel, TBadRequestErrorModel> Ok(TSuccessModel content)
    {
        return new Response<TSuccessModel, TBadRequestErrorModel>
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccessStatusCode = true,
            Content = content
        };
    }

    public static Response<TSuccessModel, TBadRequestErrorModel> BadRequest(TBadRequestErrorModel errorModel)
    {
        return new Response<TSuccessModel, TBadRequestErrorModel>
        {
            StatusCode = HttpStatusCode.BadRequest,
            IsSuccessStatusCode = false,
            ErrorModel = errorModel
        };
    }

    public new static Response<TSuccessModel, TBadRequestErrorModel> Forbidden()
    {
        return new Response<TSuccessModel, TBadRequestErrorModel>
        {
            StatusCode = HttpStatusCode.Forbidden,
            IsSuccessStatusCode = false
        };
    }

    public new static Response<TSuccessModel, TBadRequestErrorModel> NotFound()
    {
        return new Response<TSuccessModel, TBadRequestErrorModel>
        {
            StatusCode = HttpStatusCode.NotFound,
            IsSuccessStatusCode = false
        };
    }

    public new static Response<TSuccessModel, TBadRequestErrorModel> InternalServerError()
    {
        return new Response<TSuccessModel, TBadRequestErrorModel>
        {
            StatusCode = HttpStatusCode.InternalServerError,
            IsSuccessStatusCode = false
        };
    }
}