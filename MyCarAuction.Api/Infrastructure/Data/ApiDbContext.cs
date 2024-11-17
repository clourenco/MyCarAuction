using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Infrastructure.Data.Entities;

namespace MyCarAuction.Api.Infrastructure.Data;

internal class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    public DbSet<VehicleEntity> Vehicles { get; set; }
    public DbSet<AuctionEntity> Auctions { get; set; }
    public DbSet<BidEntity> Bids { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}
