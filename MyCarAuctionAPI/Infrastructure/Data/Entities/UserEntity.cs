using System.ComponentModel.DataAnnotations;

namespace MyCarAuctionAPI.Infrastructure.Data.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
