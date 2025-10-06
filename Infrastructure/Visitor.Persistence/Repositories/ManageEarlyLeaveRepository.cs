using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.Persistence.Repositories
{
    public class ManageEarlyLeaveRepository : GenericRepository, IManageEarlyLeaveRepository
    {
        private IConfiguration _configuration;

        public ManageEarlyLeaveRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Employee
        public async Task<int> SaveEarlyLeave(EarlyLeave_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@LeaveTypeId", parameters.LeaveTypeId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@Reason", parameters.Reason);
            queryParameters.Add("@LeaveDesc", parameters.LeaveDesc);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveEarlyLeave", queryParameters);
        }

        public async Task<IEnumerable<EarlyLeave_Response>> GetEarlyLeaveList(EarlyLeave_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<EarlyLeave_Response>("GetEarlyLeaveList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<EarlyLeave_Response?> GetEarlyLeaveById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<EarlyLeave_Response>("GetEarlyLeaveById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<EmployeeEarlyLeave_CheckedInOut_Response>> GetEmployeeEarlyLeave_CheckedInOut_List(EmployeeEarlyLeave_CheckedInOut_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitDate", parameters.VisitDate);
            //queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@IsCheckIn_CheckOut", parameters.IsCheckIn_CheckOut);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<EmployeeEarlyLeave_CheckedInOut_Response>("GetEmployeeEarlyLeave_CheckedInOut_List", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion

        #region Worker
        public async Task<int> SaveWorkerEarlyLeave(WorkerEarlyLeave_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@LeaveTypeId", parameters.LeaveTypeId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@WorkerId", parameters.WorkerId);
            queryParameters.Add("@Reason", parameters.Reason);
            queryParameters.Add("@LeaveDesc", parameters.LeaveDesc);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkerEarlyLeave", queryParameters);
        }

        public async Task<IEnumerable<WorkerEarlyLeave_Response>> GetWorkerEarlyLeaveList(WorkerEarlyLeave_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkerEarlyLeave_Response>("GetWorkerEarlyLeaveList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<WorkerEarlyLeave_Response?> GetWorkerEarlyLeaveById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<WorkerEarlyLeave_Response>("GetWorkerEarlyLeaveById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<WorkerEarlyLeave_CheckedInOut_Response>> GetWorkerEarlyLeave_CheckedInOut_List(WorkerEarlyLeave_CheckedInOut_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitDate", parameters.VisitDate);
            //queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@IsCheckIn_CheckOut", parameters.IsCheckIn_CheckOut);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkerEarlyLeave_CheckedInOut_Response>("GetWorkerEarlyLeave_CheckedInOut_List", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion

        public async Task<int> EarlyLeaveApproveNReject(EarlyLeave_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("EarlyLeaveApproveNReject", queryParameters);
        }

        public async Task<IEnumerable<EarlyLeaveApproveNRejectHistory_Response>> GetEarlyLeaveApproveNRejectHistoryListById(EarlyLeaveApproveNRejectHistory_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);

            var result = await ListByStoredProcedure<EarlyLeaveApproveNRejectHistory_Response>("GetEarlyLeaveApproveNRejectHistoryListById", queryParameters);
            return result;
        }
    }
}
