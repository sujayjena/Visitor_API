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
        public GroceryRequisition_Request()
        {
            GroceryRequisitionDetails = new List<GroceryRequisitionDetails_Request>();
        }

        [DefaultValue(1)]
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
        public List<GroceryRequisitionDetails_Request> GroceryRequisitionDetails { get; set; }
    }
    public class GroceryRequisition_Search : BaseSearchEntity
    {
        public int? StatusId { get; set; }
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
    }
    public class GroceryRequisition_Response : BaseEntity
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
        public List<GroceryRequisitionDetails_Response> GroceryRequisitionDetails { get; set; }
    }
    public class GroceryRequisitionDetails_Request : BaseEntity
    {
        public int? GroceryRequisitionId { get; set; }
        public int? GroceryId { get; set; }
        public decimal? OrderQty { get; set; }
    }
    public class GroceryRequisitionDetails_Search : BaseSearchEntity
    {
        public int? GroceryRequisitionId { get; set; }
    }
    public class GroceryRequisitionDetails_Response : BaseEntity
    {
        public int? GroceryRequisitionId { get; set; }
        public int? GroceryId { get; set; }
        public string? GroceryName { get; set; }
        public decimal? OrderQty { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? AvailableQty { get; set; }
    }
}
