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
    public class EfInsurancePolicyDal : EfEntityRepositoryBase<InsurancePolicy, DataContext>, IInsurancePolicyDal
    {
        public EfInsurancePolicyDal(DataContext context) : base(context) { }

        public async Task<List<InsurancePolicy>> GetAllWithDetailsAsync()
        {
            return await _context.InsurancePolicies
                .Include(p => p.Vehicle)
                .Include(p => p.InsurancePayments)
                .ToListAsync();
        }

        public async Task<InsurancePolicy?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.InsurancePolicies
                .Include(p => p.Vehicle)
                .Include(p => p.InsurancePayments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<InsurancePolicy>> GetByVehicleIdWithDetailsAsync(Guid vehicleId)
        {
            return await _context.InsurancePolicies
                .Include(p => p.Vehicle)
                .Include(p => p.InsurancePayments)
                .Where(p => p.VehicleId == vehicleId)
                .ToListAsync();
        }
    }

}
