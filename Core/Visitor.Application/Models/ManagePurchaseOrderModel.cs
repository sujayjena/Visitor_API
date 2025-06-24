using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    public class PurchaseOrder_Request : BaseEntity
    {
        public int? ContractorId { get; set; }
        public string? PONumber { get; set; }
        public string? POName { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string? DocumentOriginalFileName { get; set; }

        [JsonIgnore]
        public string? DocumentFileName { get; set; }
        public string? Document_Base64 { get; set; }
        public int? NoofPOWorker { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PurchaseOrderSearch_Request : BaseSearchEntity
    {
        public int? ContractorId { get; set; }
    }

    public class PurchaseOrder_Response : BaseResponseEntity
    {
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public string? PONumber { get; set; }
        public string? POName { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public DateTime? ContractorValidity { get; set; }
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentFileName { get; set; }
        public string? DocumentURL { get; set; }
        public int? NoofPOWorker { get; set; }
        public bool? IsActive { get; set; }
    }
}
