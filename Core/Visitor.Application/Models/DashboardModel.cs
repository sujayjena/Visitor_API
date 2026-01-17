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
        public int DepartmentId { get; set; }
        public int BranchId { get; set; }
    }

    public class Dashboard_TotalSummary_Result
    {
        public int? VisitorCount { get; set; }
        public int? VisitorPendingCount { get; set; }
        public int? VisitorApproveCount { get; set; }
        public int? VisitorRejectCount { get; set; }
        public int? EmployeeCount { get; set; }
        public int? EmployeePendingCount { get; set; }
        public int? EmployeeApproveCount { get; set; }
        public int? EmployeeRejectCount { get; set; }
        public int? WorkerCount { get; set; }
        public int? WorkerPendingCount { get; set; }
        public int? WorkerApproveCount { get; set; }
        public int? WorkerRejectCount { get; set; }
        public int? PeopleInsideCount { get; set; }
        public int? TotalLeftCount { get; set; }
        public int? CanteenCount { get; set; }
    }

    public class Dashboard_TokenCountSummary_Search
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [DefaultValue("")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }
    }

    public class Dashboard_TokenCountSummary_Result
    {
        public int? BreakfastCount { get; set; }
        public int? LunchCount { get; set; }
        public int? SnacksCount { get; set; }
        public int? DinnerCount { get; set; }
    }
    public class Dashboard_Security_Search_Request
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [DefaultValue(0)]
        public int GateDetailsId { get; set; }
        public int DepartmentId { get; set; }
        public int BranchId { get; set; }

        [DefaultValue("Visitor")]
        public string? RefType { get; set; }
    }
    public class Dashboard_Security_TotalSummary_Result
    {
        public int? TotalCount { get; set; }
        public int? TotalPendingCount { get; set; }
        public int? TotalCheckedInCount { get; set; }
        public int? TotalCheckedOutCount { get; set; }
    }
}
