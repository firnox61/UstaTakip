using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Repositories
{
    public interface IInsurancePolicyDal : IEntityRepository<InsurancePolicy>
    {
        Task<int> GetActiveCountAsync();
        Task<List<InsurancePolicy>> GetAllWithDetailsAsync();
        Task<InsurancePolicy?> GetByIdWithDetailsAsync(Guid id);
        Task<List<InsurancePolicy>> GetByVehicleIdWithDetailsAsync(Guid vehicleId);
        Task<List<InsurancePolicy>> GetExpiringAsync(DateTime utcNow, int days, int take);
    }

}
