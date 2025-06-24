

using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Repositories
{
    public interface IUserDal : IEntityRepository<User>
    {
        Task<List<OperationClaim>> GetClaimsAsync(User user);
    }
}
