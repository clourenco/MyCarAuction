using FluentValidation;
using MyCarAuction.Api.Common.CustomException;
using MyCarAuction.Api.Feature.Auctions.Common.Exceptions;
using MyCarAuction.Api.Features.Auctions.Common.Exceptions;
using System.Net;

namespace MyCarAuction.Api.Middleware;

internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        //context.Response.ContentType = "application/json";
        context.Response.ContentType = "text/plain";

        //var response = new { message = exception.Message };

        context.Response.StatusCode = exception switch
        {
            ArgumentException or
            ArgumentNullException or
            ValidationException or
            AuctionContainsVehicleException or
            AuctionNotActiveException or
            BidAmountSmallerThanHighestBidAmountException or
            VehicleSoldException => (int)HttpStatusCode.BadRequest,
            VehicleNotFoundException or
            AuctionNotFoundException or
            VehicleNotInAuctionException => (int)HttpStatusCode.NotFound,
            InvalidOperationException or KeyViolationException => (int)HttpStatusCode.Conflict,
            _ => (int)HttpStatusCode.InternalServerError
        };

        //return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        return context.Response.WriteAsync(exception.Message);
    }
}
