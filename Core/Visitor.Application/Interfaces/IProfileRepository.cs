﻿using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IProfileRepository
    {
        #region Department

        Task<int> SaveDepartment(Department_Request parameters);

        Task<IEnumerable<Department_Response>> GetDepartmentList(Department_Search parameters);

        Task<Department_Response?> GetDepartmentById(long Id);

        #endregion

        #region Role 

        Task<int> SaveRole(Role_Request parameters);

        Task<IEnumerable<Role_Response>> GetRoleList(Role_Search parameters);

        Task<Role_Response?> GetRoleById(long Id);

        #endregion

        #region RoleHierarchy 

        Task<int> SaveRoleHierarchy(RoleHierarchy_Request parameters);

        Task<IEnumerable<RoleHierarchy_Response>> GetRoleHierarchyList(RoleHierarchy_Search parameters);

        Task<RoleHierarchy_Response?> GetRoleHierarchyById(long Id);

        #endregion

        #region Reporting To 

        Task<IEnumerable<SelectListResponse>> GetReportingToEmployeeForSelectList(ReportingToEmpListParameters parameters);
        Task<IEnumerable<EmployeesListByReportingTo_Response>> GetEmployeesListByReportingTo(int EmployeeId);

        #endregion
    }
}
