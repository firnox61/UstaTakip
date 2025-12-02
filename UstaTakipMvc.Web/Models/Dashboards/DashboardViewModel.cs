using UstaTakipMvc.Web.Models.InsurancePolicies;
using UstaTakipMvc.Web.Models.RepairJobs;
using UstaTakipMvc.Web.Models.Vehicles;

namespace UstaTakipMvc.Web.Models.Dashboards
{
    public class DashboardViewModel
    {
        // Sayaçlar
        public int VehicleCount { get; set; }
        public int CustomerCount { get; set; }
        public int OpenRepairCount { get; set; }
        public int InProgressRepairCount { get; set; }
        public int ExpiringPolicyCount { get; set; }

        // Listeler
        public List<VehicleListDto> RecentVehicles { get; set; } = new();
        public List<InsurancePolicyListDto> ExpiringPolicies { get; set; } = new();
        public List<RepairJobListDto> RecentRepairs { get; set; } = new();

        // Grafik verisi
        public List<MonthlyRepairJobDto> MonthlyStats { get; set; } = new();
    }
}
