namespace MyCarAuction.Api.Common.CustomException;

internal sealed class AuctionNotActiveException : Exception
{
    public AuctionNotActiveException() : base("The specified auction is not active.")
    {
    }

    public AuctionNotActiveException(string? message) : base(message)
    {
    }

    public AuctionNotActiveException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
