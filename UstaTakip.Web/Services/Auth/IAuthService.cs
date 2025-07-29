using UstaTakip.Web.Models.Users;

namespace UstaTakip.Web.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(UserForLoginDto loginModel);
        Task LogoutAsync();
        Task<string?> GetTokenAsync();
        bool IsAuthenticated { get; }
    }
}
