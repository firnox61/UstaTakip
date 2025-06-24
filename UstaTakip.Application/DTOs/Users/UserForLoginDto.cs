using UstaTakip.Core.Abstractions;
namespace UstaTakip.Application.DTOs.Users
{
    public class UserForLoginDto : IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
