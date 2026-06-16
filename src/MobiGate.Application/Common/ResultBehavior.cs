using MediatR;
using Microsoft.Extensions.Logging;
using MobiGate.Domain.Common;

namespace MobiGate.Application.Common;

public class ResultBehavior<TRequest, TResponse>(ILogger<ResultBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next(ct);

        if (response is IResult { IsSuccess: false } result)
        {
            logger.LogWarning("Request {Request} failed: {Error} (HTTP {StatusCode})",
                typeof(TRequest).Name, result.Error, result.StatusCode);

            throw new ResultException(result.Error!, result.StatusCode!.Value);
        }

        return response;
    }
}
