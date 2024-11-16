using System.ComponentModel.DataAnnotations;

namespace MyCarAuctionAPI.Features.Auctions.Models
{
    public class Auction(Guid id, bool isActive, string description)
    {
        public Guid Id { get; } = id;
        public bool IsActive { get; } = isActive;
        public string Description { get; } = description;
    }
}
