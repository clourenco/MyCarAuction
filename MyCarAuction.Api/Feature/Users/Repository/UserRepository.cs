using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Infrastructure.Data;
using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Users.Repository;

internal sealed class UserRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<UserEntity>(dbContextFactory), IUserRepository
{
    public async Task<IEnumerable<UserEntity>> FindUser(string name, string email, CancellationToken cancellationToken)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Set<UserEntity>().Where(u =>
            (name == null || EF.Functions.Like(u.Name, $"%{name}%")) &&
            (email == null || u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
        ).ToListAsync(cancellationToken);
    }
}
