using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Application.Models
{
    public class Dashboard_Search_Request
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [DefaultValue(0)]
        public int GateDetailsId { get; set; }
    }

    public class Dashboard_TotalSummary_Result
    {
        public int? EmployeeCount { get; set; }
        public int? VisitorCount { get; set; }
        public int? WorkerCount { get; set; }
        public int? PeopleInsideCount { get; set; }
        public int? TotalLeftCount { get; set; }
        public int? CanteenCount { get; set; }
    }
}
