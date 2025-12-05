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
                    .ThenInclude(ip => ip.RepairJob)
                .ToListAsync();
        }

        public async Task<InsurancePolicy?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.InsurancePolicies
                .Include(p => p.Vehicle)
                .Include(p => p.InsurancePayments)
                    .ThenInclude(ip => ip.RepairJob)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<InsurancePolicy>> GetByVehicleIdWithDetailsAsync(Guid vehicleId)
        {
            return await _context.InsurancePolicies
                .Include(p => p.Vehicle)
                .Include(p => p.InsurancePayments)
                    .ThenInclude(ip => ip.RepairJob)
                .Where(p => p.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<List<InsurancePolicy>> GetExpiringAsync(DateTime utcNow, int days, int take)
        {
            var until = utcNow.AddDays(days);

            return await _context.InsurancePolicies
                .Include(p => p.Vehicle)
                .Include(p => p.InsurancePayments)
                    .ThenInclude(ip => ip.RepairJob)
                .Where(p => p.EndDate >= utcNow && p.EndDate <= until)
                .OrderBy(p => p.EndDate)
                .Take(Math.Clamp(take, 1, 100))
                .ToListAsync();
        }

        public async Task<int> GetActiveCountAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.InsurancePolicies
                .AsNoTracking()
                .CountAsync(p => p.EndDate >= now);
        }
    }

}