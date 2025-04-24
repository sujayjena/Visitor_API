using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    public class ManageAttendance_Search : BaseSearchEntity
    {
        public int? EmployeeId { get; set; }

        [DefaultValue("")]
        public string? GateDetailsId { get; set; }

        public int? VisitorId { get; set; }
    }

    public class ManageAttendance_Request : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public int? GateDetailsId { get; set; }
        public string? AttendanceStatus { get; set; }
        public string? BatteryStatus { get; set; }
        public int? VisitorId { get; set; }
        public string? Remarks { get; set; }
    }

    public class ManageAttendance_Response : BaseResponseEntity
    {
        public int? EmployeeId { get; set; }
        public string? SecurityName { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
        public string? AttendanceStatus { get; set; }
        public string? BatteryStatus { get; set; }
        public int? VisitorId { get; set; }
        public string? VisitorName { get; set; }
        public string? Remarks { get; set; }
    }
}
