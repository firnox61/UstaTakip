using UstaTakip.Application.DTOs.Users;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IUserOperationClaimService
    {
        Task<IDataResult<List<UserOperationClaimListDto>>> GetByUserIdAsync(int userId);
        Task<IResult> AddAsync(UserOperationClaimCreateDto dto);
        Task<IResult> DeleteAsync(int id);
    }
}
