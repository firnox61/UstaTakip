using UstaTakip.Core.Abstractions;
namespace UstaTakipMvc.Web.Models.Users
{
    public class UserForRegisterDto 
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
