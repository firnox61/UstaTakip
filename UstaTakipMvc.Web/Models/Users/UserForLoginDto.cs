using System.ComponentModel.DataAnnotations;
using UstaTakip.Core.Abstractions;
namespace UstaTakipMvc.Web.Models.Users
{
    public class UserForLoginDto 
    {
        [Required(ErrorMessage = "Email zorunludur")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; } = string.Empty;
    }
}
