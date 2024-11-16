namespace MyCarAuctionAPI.Features.Bids.Models
{
    public class Bid(Guid auctionId, Guid vehicleId, Guid bidderId, decimal amount)
    {
        public Guid AuctionId { get; } = auctionId;
        public Guid VehicleId { get; } = vehicleId;
        public Guid BidderId { get; } = bidderId;
        public decimal Amount { get; } = amount;
    }
}
