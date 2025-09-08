namespace UstaTakipMvc.Web.Models.InsurancePolicies
{
    public class InsurancePolicyCreateDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal CoverageAmount { get; set; }
        public Guid VehicleId { get; set; }
    }
}
