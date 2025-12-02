using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.RepairJobImage;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IRepairJobImageService
    {
        Task<IDataResult<List<RepairJobImageListDto>>> GetByRepairJobIdAsync(Guid repairJobId);
        Task<IDataResult<RepairJobImageListDto>> GetByIdAsync(Guid id);
        Task<IResult> AddAsync(RepairJobImage entity);
        Task<IResult> DeleteAsync(Guid id);
    }

}
