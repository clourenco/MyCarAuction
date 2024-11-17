using System.ComponentModel.DataAnnotations;

namespace MyCarAuction.Api.Infrastructure.Data.Entities;

internal class UserEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
