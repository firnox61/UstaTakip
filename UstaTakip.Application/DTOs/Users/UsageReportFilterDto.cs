using UstaTakip.Core.Abstractions;
namespace UstaTakip.Application.DTOs.Users
{
    public class UsageReportFilterDto : IDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
