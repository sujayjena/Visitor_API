using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IManageVisitorsRepository
    {
        Task<int> SaveVisitors(Visitors_Request parameters);
        Task<IEnumerable<Visitors_Response>> GetVisitorsList(Visitors_Search parameters);
        Task<Visitors_Response?> GetVisitorsById(int Id);
        Task<int> VisitorsApproveNReject(Visitor_ApproveNReject parameters);
        Task<Visitors_Response?> GetVisitorDetailByMobileNumber(string MobileNumber);

        Task<int> SaveVisitorsGateNo(VisitorGateNo_Request parameters);
        Task<IEnumerable<VisitorGateNo_Response>> GetVisitorsGateNoByVisitorId(long VisitorId, long GateDetailsId);
        Task<IEnumerable<VisitorApproveNRejectHistory_Response>> GetVisitorApproveNRejectHistoryListById(VisitorApproveNRejectHistory_Search parameters);
    }
}
