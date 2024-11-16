using System.ComponentModel.DataAnnotations;

namespace MyCarAuctionAPI.Infrastructure.Data.Entities
{
    public class BidEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid BidderId { get; set; }
        public decimal Amount { get; set; }
    }
}
