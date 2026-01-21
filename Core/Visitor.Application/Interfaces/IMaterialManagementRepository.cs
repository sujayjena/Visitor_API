using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IMaterialManagementRepository
    {
        #region Material Request
        Task<int> SaveMaterialRequest(MaterialRequest_Request parameters);
        Task<IEnumerable<MaterialRequestList_Response>> GetMaterialRequestList(MaterialRequest_Search parameters);
        Task<MaterialRequest_Response?> GetMaterialRequestById(int Id);
        Task<int> MaterialRequestApproveNReject(MaterialRequest_ApproveNReject parameters);
        #endregion

        #region Material Request Details
        Task<int> SaveMaterialRequestDetails(MaterialRequestDetails_Request parameters);
        Task<IEnumerable<MaterialRequestDetails_Response>> GetMaterialRequestDetailsList(MaterialRequestDetails_Search parameters);
        Task<int> DeleteMaterialRequestDetails(int Id);
        #endregion
    }
}
