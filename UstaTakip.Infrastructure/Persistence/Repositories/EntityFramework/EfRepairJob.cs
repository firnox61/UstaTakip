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
    public class EfRepairJobDal : EfEntityRepositoryBase<RepairJob, DataContext>, IRepairJobDal
    {
        public EfRepairJobDal(DataContext context) : base(context) { }

        public async Task<List<RepairJob>> GetAllWithVehicleAsync()
        {
            return await _context.RepairJobs
                .Include(r => r.Vehicle) // plaka için eager load
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RepairJob?> GetByIdWithVehicleAsync(Guid id)
        {
            return await _context.RepairJobs
                .Include(r => r.Vehicle)
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == id);
        }
        public async Task<List<RepairJob>> GetRecentWithVehicleAsync(int take)
        {
            take = Math.Clamp(take, 1, 100);
            return await _context.RepairJobs
                .Include(r => r.Vehicle)
                .AsNoTracking()
                .OrderByDescending(r => r.Date)
                .Take(take)
                .ToListAsync();
        }
    }
}
