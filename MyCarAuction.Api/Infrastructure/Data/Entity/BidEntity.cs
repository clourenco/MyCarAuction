using System.ComponentModel.DataAnnotations;

namespace MyCarAuction.Api.Infrastructure.Data.Entities;

internal class BidEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid AuctionId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid BidderId { get; set; }
    public decimal Amount { get; set; }
}
