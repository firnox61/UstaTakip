using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.VehicleImages;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IVehicleImageService
    {
        Task<IResult> AddAsync(VehicleImage entity);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<VehicleImageListDto>>> GetByVehicleIdAsync(Guid vehicleId);
        Task<IDataResult<VehicleImageListDto>> GetByIdAsync(Guid id);
    }


}
