
using UstaTakip.Application.Repositories;
using UstaTakip.Domain.Entities;
using UstaTakip.Infrastructure.Persistence.Context;

namespace UstaTakip.Infrastructure.Persistence.Repositories.EntityFramework
{
    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, DataContext>, IOperationClaimDal
    {
        public EfOperationClaimDal(DataContext context) : base(context) { }
    }
}
