using UstaTakip.Core.Abstractions;
namespace UstaTakip.Web.Models.Users
{
    public class UserOperationClaimCreateDto 
    {
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
