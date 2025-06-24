using UstaTakip.Core.Abstractions;
namespace UstaTakip.Application.DTOs.Users
{
    public class OperationClaimListDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
