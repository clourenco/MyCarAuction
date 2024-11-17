using System.ComponentModel.DataAnnotations;

namespace MyCarAuction.Api.Infrastructure.Data.Entities;

internal class VehicleEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int NumberOfDoors { get; set; }
    public int? NumberOfSeats { get; set; }
    public int? LoadCapacity { get; set; }
    public decimal StartingBid { get; set; }
}
