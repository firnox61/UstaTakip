using System.Globalization;

namespace UstaTakipMvc.Web.Models.RepairJobs
{
    public class MonthlyRepairJobDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public int Open { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
        public string MonthName => new DateTime(Year, Month, 1)
            .ToString("MMMM", new CultureInfo("tr-TR"));
    }

}
