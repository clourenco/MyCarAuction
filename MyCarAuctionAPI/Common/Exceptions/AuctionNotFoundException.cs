namespace MyCarAuction.Api.Common.Exceptions
{
    public sealed class AuctionNotFoundException : Exception
    {
        public AuctionNotFoundException() : base("The specified auction was not found.")
        {
        }

        public AuctionNotFoundException(string? message) : base(message)
        {
        }

        public AuctionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
