using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IManageEarlyLeaveRepository
    {
        #region Employee
        Task<int> SaveEarlyLeave(EarlyLeave_Request parameters);
        Task<IEnumerable<EarlyLeave_Response>> GetEarlyLeaveList(EarlyLeave_Search parameters);
        Task<EarlyLeave_Response?> GetEarlyLeaveById(int Id);
        Task<IEnumerable<EmployeeEarlyLeave_CheckedInOut_Response>> GetEmployeeEarlyLeave_CheckedInOut_List(EmployeeEarlyLeave_CheckedInOut_Search parameters);

        #endregion

        #region Worker
        Task<int> SaveWorkerEarlyLeave(WorkerEarlyLeave_Request parameters);
        Task<IEnumerable<WorkerEarlyLeave_Response>> GetWorkerEarlyLeaveList(WorkerEarlyLeave_Search parameters);
        Task<WorkerEarlyLeave_Response?> GetWorkerEarlyLeaveById(int Id);
        Task<IEnumerable<WorkerEarlyLeave_CheckedInOut_Response>> GetWorkerEarlyLeave_CheckedInOut_List(WorkerEarlyLeave_CheckedInOut_Search parameters);

        #endregion

        Task<int> EarlyLeaveApproveNReject(EarlyLeave_ApproveNReject parameters);
        Task<IEnumerable<EarlyLeaveApproveNRejectHistory_Response>> GetEarlyLeaveApproveNRejectHistoryListById(EarlyLeaveApproveNRejectHistory_Search parameters);
    }
}
