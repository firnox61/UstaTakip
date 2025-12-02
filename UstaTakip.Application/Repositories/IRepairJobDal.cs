using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Repositories
{
    public interface IRepairJobDal : IEntityRepository<RepairJob>
    {
        Task<List<RepairJob>> GetAllWithVehicleAsync();
        Task<RepairJob?> GetByIdWithVehicleAsync(Guid id);
        Task<List<RepairJob>> GetRecentWithVehicleAsync(int take);
        Task<List<MonthlyRepairJobDto>> GetMonthlyStatsAsync();

    }
}
