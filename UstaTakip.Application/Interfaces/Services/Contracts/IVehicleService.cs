using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.Vehicles;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IVehicleService
    {
        Task<IDataResult<List<VehicleListDto>>> GetAllAsync();
        Task<IDataResult<VehicleListDto>> GetByIdAsync(Guid id);
        Task<IResult> AddAsync(VehicleCreateDto dto);
        Task<IResult> UpdateAsync(VehicleUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<VehicleListDto>>> GetListAsync(); // ✅ Bunu ekleyin

        Task<IDataResult<List<VehicleListDto>>> GetByCustomerIdAsync(Guid customerId);
    }

}
