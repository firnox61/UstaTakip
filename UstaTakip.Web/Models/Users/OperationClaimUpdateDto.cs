using UstaTakip.Core.Abstractions;
namespace UstaTakip.Web.Models.Users
{
    public class OperationClaimUpdateDto 
    {
        public int Id { get; set; } // 🟢 güncellenecek ID burada gelmeli
        public string Name { get; set; }
    }
}
