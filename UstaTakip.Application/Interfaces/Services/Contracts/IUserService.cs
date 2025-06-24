
using UstaTakip.Application.DTOs.Users;
using UstaTakip.Domain.Entities;
using UstaTakip.Core.Utilities.Results;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IUserService
    {
        Task<IDataResult<List<UserListDto>>> GetAllAsync();
        Task<IResult> EditProfil(UserDto userDto, string password);
        Task<IDataResult<UserListDto>> GetByIdAsync(int id);
        Task<IDataResult<User>> GetByEmailAsync(string email);
        Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user);
        Task<IResult> AddAsync(UserCreateDto dto);
        Task<IResult> UpdateAsync(UserUpdateDto dto);
        Task<IResult> DeleteAsync(int id);


    }
}
