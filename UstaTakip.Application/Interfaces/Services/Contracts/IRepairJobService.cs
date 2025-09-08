using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobs;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IRepairJobService
    {
        Task<IDataResult<List<RepairJobListDto>>> GetRecentAsync(int take);
        Task<IDataResult<List<RepairJobListDto>>> GetAllAsync();
        Task<IDataResult<RepairJobListDto>> GetByIdAsync(Guid id);
        Task<IResult> AddAsync(RepairJobCreateDto dto);
        Task<IResult> UpdateAsync(RepairJobUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<RepairJobListDto>>> GetByVehicleIdAsync(Guid vehicleId);
    }

}
