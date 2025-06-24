using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UstaTakip.Infrastructure.Security.Encryption
{//vereceğimiz şifreyi token jsonunn anlayabileceği hale getirmek için yapıyrouz byte array hale getiriyo
    public class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }

    }
}

