using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IManageGroceryRepository
    {
        #region Grocery Requisition
        Task<int> SaveGroceryRequisition(GroceryRequisition_Request parameters);
        Task<IEnumerable<GroceryRequisitionList_Response>> GetGroceryRequisitionList(GroceryRequisition_Search parameters);
        Task<GroceryRequisition_Response?> GetGroceryRequisitionById(int Id);
        Task<IEnumerable<GroceryRequisition_ApproveNRejectHistory_Response>> GetGroceryRequisition_ApproveNRejectHistoryListById(GroceryRequisition_ApproveNRejectHistory_Search parameters);
        #endregion

        #region Grocery Requisition Details
        Task<int> SaveGroceryRequisitionDetails(GroceryRequisitionDetails_Request parameters);

        Task<IEnumerable<GroceryRequisitionDetails_Response>> GetGroceryRequisitionDetailsList(GroceryRequisitionDetails_Search parameters);
        #endregion

        #region Grocery Outwarding
        Task<int> SaveGroceryOutwarding(GroceryOutwarding_Request parameters);

        Task<IEnumerable<GroceryOutwarding_Response>> GetGroceryOutwardingList(GroceryOutwarding_Search parameters);
        #endregion
    }
}
