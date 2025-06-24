using Microsoft.EntityFrameworkCore;
using UstaTakip.Application.Repositories;
using UstaTakip.Domain.Entities;
using UstaTakip.Infrastructure.Persistence.Context;

namespace UstaTakip.Infrastructure.Persistence.Repositories.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, DataContext>, IUserDal
    {

        public EfUserDal(DataContext context) : base(context) { }

        public async Task<List<OperationClaim>> GetClaimsAsync(User user)
        {
            var claims = await _context.UserOperationClaims
                .Where(uoc => uoc.UserId == user.Id)
                .Include(uoc => uoc.OperationClaim)
                .Select(uoc => uoc.OperationClaim)
                .ToListAsync();

            return claims;
        }
    }
}
