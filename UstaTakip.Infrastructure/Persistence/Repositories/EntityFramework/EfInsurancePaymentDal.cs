using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.Repositories;
using UstaTakip.Domain.Entities;
using UstaTakip.Infrastructure.Persistence.Context;

namespace UstaTakip.Infrastructure.Persistence.Repositories.EntityFramework
{
    public class EfInsurancePaymentDal : EfEntityRepositoryBase<InsurancePayment, DataContext>, IInsurancePaymentDal
    {
        public EfInsurancePaymentDal(DataContext context) : base(context) { }

        public async Task<List<InsurancePayment>> GetAllWithDetailsAsync()
        {
            return await _context.InsurancePayments
                .Include(p => p.RepairJob)
                .Include(p => p.InsurancePolicy)
                .ToListAsync();
        }

        public async Task<InsurancePayment?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.InsurancePayments
                .Include(p => p.RepairJob)
                .Include(p => p.InsurancePolicy)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<InsurancePayment?> GetByRepairJobIdAsync(Guid repairJobId)
        {
            return await _context.InsurancePayments
                .Include(p => p.RepairJob)
                .Include(p => p.InsurancePolicy)
                .FirstOrDefaultAsync(p => p.RepairJobId == repairJobId);
        }

        public async Task<List<InsurancePayment>> GetByPolicyIdWithDetailsAsync(Guid insurancePolicyId)
        {
            return await _context.InsurancePayments
                .Include(p => p.RepairJob)
                .Include(p => p.InsurancePolicy)
                .Where(p => p.InsurancePolicyId == insurancePolicyId)
                .ToListAsync();
        }
    }

}
