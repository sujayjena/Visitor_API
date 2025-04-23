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

        Task<int> SaveVisitorLogHistory(int VisitorId);
        Task<IEnumerable<VisitorLogHistory_Response>> GetVisitorLogHistoryList(VisitorLogHistory_Search parameters);

        Task<IEnumerable<VisitorPlanned_Response>> GetVisitorPlannedList(VisitorPlanned_Search parameters);
        Task<int> SaveVisitorCheckedInOut(VisitorCheckedInOut_Request parameters);

        Task<int> SaveVisitorDocumentVerification(VisitorDocumentVerification_Request parameters);
        Task<IEnumerable<VisitorDocumentVerification_Response>> GetVisitorDocumentVerificationList(VisitorDocumentVerification_Search parameters);

        Task<int> SaveVisitorAsset(VisitorAsset_Request parameters);
        Task<IEnumerable<VisitorAsset_Response>> GetVisitorAssetList(VisitorDocumentVerification_Search parameters);

        Task<int> SaveBarcode(Barcode_Request parameters);
        Task<Barcode_Response?> GetBarcodeById(string BarcodeNo);
    }
}
