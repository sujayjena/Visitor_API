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
    public class EarlyLeave_Request : BaseEntity
    {
        public int? LeaveTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public int? EmployeeId { get; set; }
        public int? Reason { get; set; }
        public string? LeaveDesc { get; set; }

        [DefaultValue(1)]
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class EarlyLeave_Search : BaseSearchEntity
    {
        public int? StatusId { get; set; }
    }


    public class EarlyLeave_Response : BaseResponseEntity
    {
        public int? LeaveTypeId { get; set; }
        public string? LeaveType { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? Reason { get; set; }
        public string? LeaveDesc { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }
    }
}
