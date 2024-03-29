﻿using Microsoft.AspNetCore.Diagnostics;

namespace BookStoreAPI.Infrastructure.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (int statusCode, string errorMessage) = exception switch
            {
                ForbidException forbidException => (403, forbidException.Message),
                BadRequestException badRequestException => (400, badRequestException.Message),
                NotFoundException notFoundException => (404, notFoundException.Message),
                UnauthorizedException unauthorizedException => (401, unauthorizedException.Message),
                AccountException accountException => (400, accountException.Message),
                WishlistException wishlistException => (400, wishlistException.Message),
                OrderException orderException => (400, orderException.Message),
                ValidationException validationException => (400, validationException.Message),
                _ => (500, "Something went wrong...")
            };

            _logger.LogError(exception, exception.Message);

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(errorMessage);

            return true;
        }
    }
}
