namespace MyCarAuction.Api.Features.Auctions.Common.Exceptions
{
    public sealed class AuctionContainsVehicleException : Exception
    {
        public AuctionContainsVehicleException() : base("The specified vehicle is already in a auction.")
        {
        }

        public AuctionContainsVehicleException(string? message) : base(message)
        {
        }

        public AuctionContainsVehicleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
