namespace MyCarAuction.Api.Common.CustomException;

internal sealed class VehicleNotInAuctionException : Exception
{
    public VehicleNotInAuctionException() : base("The specified vehicle is not in the specified auction.")
    {
    }

    public VehicleNotInAuctionException(string? message) : base(message)
    {
    }

    public VehicleNotInAuctionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
