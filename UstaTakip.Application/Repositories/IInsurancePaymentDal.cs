using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Repositories
{
    public interface IInsurancePaymentDal : IEntityRepository<InsurancePayment>
    {
        Task<List<InsurancePayment>> GetAllWithDetailsAsync();
        Task<InsurancePayment?> GetByIdWithDetailsAsync(Guid id);
        Task<InsurancePayment?> GetByRepairJobIdAsync(Guid repairJobId);
        Task<List<InsurancePayment>> GetByPolicyIdWithDetailsAsync(Guid insurancePolicyId);
    }

}
