namespace MyCarAuction.Api.Features.Bids.Common.Exceptions
{
    public sealed class BidAmountSmallerThanHighestBidAmountException : Exception
    {
        public BidAmountSmallerThanHighestBidAmountException(): base("The specified bid amount is smaller than the highest bid amount.")
        {
        }

        public BidAmountSmallerThanHighestBidAmountException(string? message) : base(message)
        {
        }

        public BidAmountSmallerThanHighestBidAmountException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
