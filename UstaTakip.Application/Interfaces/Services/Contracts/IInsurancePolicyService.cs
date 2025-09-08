using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaTakip.Application.DTOs.InsurancePolicys;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IInsurancePolicyService
    {
        Task<IDataResult<List<InsurancePolicyListDto>>> GetAllAsync();
        Task<IDataResult<InsurancePolicyListDto>> GetByIdAsync(Guid id);
        Task<IDataResult<List<InsurancePolicyListDto>>> GetByVehicleIdAsync(Guid vehicleId);
        Task<IResult> AddAsync(InsurancePolicyCreateDto dto);
        Task<IResult> UpdateAsync(InsurancePolicyUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<InsurancePolicyListDto>>> GetExpiringAsync(int days, int take);
        Task<IDataResult<int>> GetActiveCountAsync();
    }


}
