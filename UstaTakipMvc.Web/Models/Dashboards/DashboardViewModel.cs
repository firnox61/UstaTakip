using UstaTakipMvc.Web.Models.InsurancePolicies;
using UstaTakipMvc.Web.Models.RepairJobs;
using UstaTakipMvc.Web.Models.Vehicles;

namespace UstaTakipMvc.Web.Models.Dashboards
{
    public class DashboardViewModel
    {
        // Sayaçlar
        public int VehicleCount { get; set; }
        public int ActivePolicyCount { get; set; }
        public int PoliciesExpiringSoonCount { get; set; }
        public int OpenRepairCount { get; set; }
        public int DuePaymentCount { get; set; }

        // Listeler (kart altı tablolar)
        public List<VehicleListDto> RecentVehicles { get; set; } = new();
        public List<InsurancePolicyListDto> ExpiringPolicies { get; set; } = new();
        public List<RepairJobListDto> RecentRepairs { get; set; } = new();
    }
}
