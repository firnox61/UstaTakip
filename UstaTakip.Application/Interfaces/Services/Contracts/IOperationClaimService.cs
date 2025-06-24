
using UstaTakip.Application.DTOs.Users;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IOperationClaimService
    {
        Task<IDataResult<List<OperationClaimListDto>>> GetAllAsync();
        Task<IDataResult<OperationClaimListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(OperationClaimCreateDto dto);
        Task<IResult> UpdateAsync(OperationClaimUpdateDto dto); // ✅ güncel hali
        Task<IResult> DeleteAsync(int id);
    }
}
