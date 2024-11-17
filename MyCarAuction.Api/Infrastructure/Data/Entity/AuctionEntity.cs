using System.ComponentModel.DataAnnotations;

namespace MyCarAuction.Api.Infrastructure.Data.Entities;

internal class AuctionEntity
{
    [Key]
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
}
