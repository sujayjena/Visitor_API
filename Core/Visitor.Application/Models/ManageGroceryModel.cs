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
    public class GroceryRequisition_Request : BaseEntity
    {
        #region Grocery requisition
        public GroceryRequisition_Request()
        {
            GroceryRequisitionDetails = new List<GroceryRequisitionDetails_Request>();
        }

        [DefaultValue(1)]
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }

        [DefaultValue(false)]
        public bool? IsReceived { get; set; }
        public List<GroceryRequisitionDetails_Request> GroceryRequisitionDetails { get; set; }
    }
    public class GroceryRequisition_Search : BaseSearchEntity
    {
        public int? StatusId { get; set; }
        public bool? IsReceived { get; set; }
        public int? RequisitionId { get; set; }
    }
    public class GroceryRequisitionList_Response : BaseResponseEntity
    {
        public string? RequisitionId { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Remarks { get; set; }
        public int? Approver1_Id { get; set; }
        public string? Approver1_Name { get; set; }
        public int? Approver2_Id { get; set; }
        public string? Approver2_Name { get; set; }
        public bool? IsReceived { get; set; }
    }
    public class GroceryRequisition_Response : BaseResponseEntity
    {
        public GroceryRequisition_Response()
        {
            GroceryRequisitionDetails = new List<GroceryRequisitionDetails_Response>();
        }
        public string? RequisitionId { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Remarks { get; set; }
        public int? Approver1_Id { get; set; }
        public string? Approver1_Name { get; set; }
        public int? Approver2_Id { get; set; }
        public string? Approver2_Name { get; set; }
        public bool? IsReceived { get; set; }
        public List<GroceryRequisitionDetails_Response> GroceryRequisitionDetails { get; set; }
    }
    public class GroceryRequisition_ApproveNRejectHistory_Search : BaseSearchEntity
    {
        public int GroceryRequisitionId { get; set; }
    }
    public class GroceryRequisition_ApproveNRejectHistory_Response
    {
        public int? Id { get; set; }
        public string? Remarks { get; set; }
        public string? ApprovalType { get; set; }
        public string? ApproveOrReject { get; set; }
        public string? CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class GroceryRequisition_ApproveNReject
    {
        public int? Id { get; set; }
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
    }
    #endregion

    #region grocery requisition details
    public class GroceryRequisitionDetails_Request : BaseEntity
    {
        public int? GroceryRequisitionId { get; set; }
        public int? GroceryId { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? ReceivedQty { get; set; }
        public int? IsOK { get; set; }
    }
    public class GroceryRequisitionDetails_Search : BaseSearchEntity
    {
        public int? GroceryRequisitionId { get; set; }
        public int? IsOk { get; set; }
    }
    public class GroceryRequisitionDetails_Response : BaseEntity
    {
        public int? GroceryRequisitionId { get; set; }
        public string? RequisitionId { get; set; }
        public int? GroceryId { get; set; }
        public string? GroceryName { get; set; }
        public decimal? OrderQty { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? AvailableQty { get; set; }
        public decimal? ReceivedQty { get; set; }
        public int? IsOK { get; set; }
    }

    #endregion

    #region grocery outwarding
    public class GroceryOutwarding_Request : BaseEntity
    {
        public int? GroceryId { get; set; }
        public decimal? AvailableQty { get; set; }
        public decimal? OutwardingQty { get; set; }
        public decimal? RemainingQty { get; set; }
    }
    public class GroceryOutwarding_Search : BaseSearchEntity
    {
    }
    public class GroceryOutwarding_Response : BaseEntity
    {
        public string? OutwardingId { get; set; }
        public int? GroceryId { get; set; }
        public string? GroceryName { get; set; }
        public string? GroceryDesc { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? AvailableQty { get; set; }
        public decimal? OutwardingQty { get; set; }
        public decimal? RemainingQty { get; set; }
    }
    #endregion
}
