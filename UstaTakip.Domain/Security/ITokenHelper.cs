using UstaTakip.Domain.Entities;

namespace UstaTakip.Domain.Security
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);//ilgili kullanıcının ilgili claimlerini üretecek
    }
}
