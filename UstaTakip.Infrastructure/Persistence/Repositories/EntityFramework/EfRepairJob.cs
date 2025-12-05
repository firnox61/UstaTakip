using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Application.Repositories;
using UstaTakip.Domain.Entities;
using UstaTakip.Infrastructure.Persistence.Context;

namespace UstaTakip.Infrastructure.Persistence.Repositories.EntityFramework
{
    public class EfRepairJobDal : EfEntityRepositoryBase<RepairJob, DataContext>, IRepairJobDal
    {
        public EfRepairJobDal(DataContext context) : base(context) { }

        public async Task<List<RepairJob>> GetAllWithVehicleAsync()
        {
            return await _context.RepairJobs
                .Include(r => r.Vehicle)
                .Include(r => r.InsurancePolicy)
                .Include(r => r.InsurancePayments)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<RepairJob?> GetByIdWithVehicleAndPolicyAsync(Guid id)
        {
            return await _context.RepairJobs
                .Include(r => r.Vehicle)
                .Include(r => r.InsurancePolicy)
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RepairJob?> GetByIdWithVehicleAsync(Guid id)
        {
            return await _context.RepairJobs
                .Include(r => r.Vehicle)
                .Include(r => r.InsurancePolicy)
                .Include(r => r.InsurancePayments)
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<RepairJob>> GetRecentWithVehicleAsync(int take)
        {
            return await _context.RepairJobs
                .Include(r => r.Vehicle)
                .Include(r => r.InsurancePolicy)
                .Include(r => r.InsurancePayments)
                .AsNoTracking()
                .OrderByDescending(r => r.Date)
                .Take(Math.Clamp(take, 1, 100))
                .ToListAsync();
        }

        public async Task<List<MonthlyRepairJobStatsDto>> GetMonthlyStatsAsync()
        {
            return await _context.RepairJobs
                .AsNoTracking()
                .GroupBy(j => new { j.Date.Year, j.Date.Month })
                .Select(g => new MonthlyRepairJobStatsDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,

                    Open = g.Count(x => x.Status == "Open"),
                    InProgress = g.Count(x => x.Status == "InProgress"),
                    Completed = g.Count(x => x.Status == "Completed")
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();
        }

    }

}
