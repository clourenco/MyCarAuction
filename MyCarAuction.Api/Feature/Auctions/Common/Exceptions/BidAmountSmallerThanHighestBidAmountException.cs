namespace MyCarAuction.Api.Feature.Auctions.Common.Exceptions;

internal sealed class BidAmountSmallerThanHighestBidAmountException : Exception
{
    public BidAmountSmallerThanHighestBidAmountException() : base("The specified bid amount is smaller than the highest bid amount.")
    {
    }

    public BidAmountSmallerThanHighestBidAmountException(string? message) : base(message)
    {
    }

    public BidAmountSmallerThanHighestBidAmountException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
