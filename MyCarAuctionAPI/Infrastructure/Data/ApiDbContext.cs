using Microsoft.EntityFrameworkCore;
using MyCarAuctionAPI.Infrastructure.Data.Entities;

namespace MyCarAuctionAPI.Infrastructure.Data
{
    public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
    {
        public DbSet<VehicleEntity> Vehicles { get; set; }
        public DbSet<AuctionEntity> Auctions { get; set; }
        public DbSet<BidEntity> Bids { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}
