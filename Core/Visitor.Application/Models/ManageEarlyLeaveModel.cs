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
    #region Employee
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
        public string? UserCode { get; set; }
        public string? MobileNumber { get; set; }
        public int? Reason { get; set; }
        public string? LeaveDesc { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Worker
    public class WorkerEarlyLeave_Request : BaseEntity
    {
        public int? LeaveTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public int? EmployeeId { get; set; }
        public int? WorkerId { get; set; }
        public int? Reason { get; set; }
        public string? LeaveDesc { get; set; }

        [DefaultValue(1)]
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class WorkerEarlyLeave_Search : BaseSearchEntity
    {
        public int? StatusId { get; set; }
    }

    public class WorkerEarlyLeave_Response : BaseResponseEntity
    {
        public int? LeaveTypeId { get; set; }
        public string? LeaveType { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public string? WorkerCode { get; set; }
        public string? WorkerMobileNo { get; set; }
        public int? Reason { get; set; }
        public string? LeaveDesc { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion

    public class EarlyLeave_ApproveNReject
    {
        public int? Id { get; set; }

        [DefaultValue("Employee")]
        public string? RefType { get; set; }
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
    }
    public class EarlyLeaveApproveNRejectHistory_Search : BaseSearchEntity
    {
        public int? RefId { get; set; }

        [DefaultValue("Employee")]
        public string? RefType { get; set; }
    }
    public class EarlyLeaveApproveNRejectHistory_Response
    {
        public int? Id { get; set; }
        public int? RefId { get; set; }
        public string? RefType { get; set; }
        public string? Remarks { get; set; }
        public string? ApproveOrReject { get; set; }
        public string? CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
