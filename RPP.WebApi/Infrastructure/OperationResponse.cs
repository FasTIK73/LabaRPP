using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RPP.WebApi.Infrastructure;

/// <summary>
/// Базовый класс для формирования ответов API
/// </summary>
public abstract class OperationResponse
{
    protected HttpStatusCode StatusCode { get; set; }
    protected object? Result { get; set; }

    public IActionResult GetResponse(HttpRequest request, HttpResponse response)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(response);

        response.StatusCode = (int)StatusCode;

        if (Result is null)
        {
            return new StatusCodeResult((int)StatusCode);
        }

        return new ObjectResult(Result);
    }

    protected static TResult OK<TResult, TData>(TData data) where TResult : OperationResponse, new()
    {
        return new TResult { StatusCode = HttpStatusCode.OK, Result = data };
    }

    protected static TResult NoContent<TResult>() where TResult : OperationResponse, new()
    {
        return new TResult { StatusCode = HttpStatusCode.NoContent };
    }

    protected static TResult BadRequest<TResult>(string? errorMessage = null) where TResult : OperationResponse, new()
    {
        return new TResult { StatusCode = HttpStatusCode.BadRequest, Result = errorMessage };
    }

    protected static TResult NotFound<TResult>(string? errorMessage = null) where TResult : OperationResponse, new()
    {
        return new TResult { StatusCode = HttpStatusCode.NotFound, Result = errorMessage };
    }

    protected static TResult InternalServerError<TResult>(string? errorMessage = null) where TResult : OperationResponse, new()
    {
        return new TResult { StatusCode = HttpStatusCode.InternalServerError, Result = errorMessage };
    }
}