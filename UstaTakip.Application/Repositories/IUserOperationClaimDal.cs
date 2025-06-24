using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Repositories
{
    public interface IUserOperationClaimDal : IEntityRepository<UserOperationClaim>
    {
        Task<List<UserOperationClaim>> GetWithDetailsByUserIdAsync(int userId);
    }
}
