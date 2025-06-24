
using UstaTakip.Application.DTOs.Users;
using UstaTakip.Core.Utilities.Results;
using UstaTakip.Domain.Entities;
using UstaTakip.Domain.Security;

namespace UstaTakip.Application.Interfaces.Services.Contracts
{
    public interface IAuthService
    {
        Task<IDataResult<User>> RegisterAsync(UserForRegisterDto registerDto, string password);
        Task<IDataResult<User>> LoginAsync(UserForLoginDto loginDto);
        Task<IResult> UserExistsAsync(string email);
        Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user);
    }
}
