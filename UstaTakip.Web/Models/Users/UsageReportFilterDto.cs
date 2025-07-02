using UstaTakip.Core.Abstractions;
namespace UstaTakip.Web.Models.Users
{
    public class UsageReportFilterDto 
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
