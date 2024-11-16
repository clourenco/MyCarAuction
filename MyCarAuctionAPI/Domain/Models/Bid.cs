namespace MyCarAuction.Api.Domain.Models
{
    public class Bid(Guid id, Guid auctionId, Guid vehicleId, Guid bidderId, decimal amount)
    {
        public Guid Id { get; } = id;
        public Guid AuctionId { get; } = auctionId;
        public Guid VehicleId { get; } = vehicleId;
        public Guid BidderId { get; } = bidderId;
        public decimal Amount { get; } = amount;
    }
}
