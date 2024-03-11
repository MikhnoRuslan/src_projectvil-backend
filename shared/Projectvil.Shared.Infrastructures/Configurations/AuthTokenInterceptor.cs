using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace Projectvil.Shared.Infrastructures.Configurations;

public class AuthTokenInterceptor : Interceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthTokenInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var metadata = context.Options.Headers;
        if (metadata == null)
        {
            metadata = new Metadata();
            context = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method, context.Host, new CallOptions(metadata));
        }

        var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        
        if (!string.IsNullOrEmpty(accessToken)) metadata.Add("Authorization", accessToken);;

        return continuation(request, context);
    }
}