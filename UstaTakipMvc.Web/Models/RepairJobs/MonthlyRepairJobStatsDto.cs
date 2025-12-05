using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaTakipMvc.Web.Models.RepairJobs
{
    public class MonthlyRepairJobStatsDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public int Open { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
    }

}
