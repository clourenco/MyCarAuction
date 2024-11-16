using System.ComponentModel.DataAnnotations;

namespace MyCarAuctionAPI.Infrastructure.Data.Entities
{
    public class AuctionEntity
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}
