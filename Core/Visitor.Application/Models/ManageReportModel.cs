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
    public class CanteenUsageReport_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }

        [DefaultValue("Visitor")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }

        [DefaultValue(0)]
        public int? IsExportType { get; set; }
    }

    public class CanteenUsageReport_Response : BaseEntity
    {
        public DateTime? ConsumptionDate { get; set; }
        public string? RefName { get; set; }
        public string? RefTypeID { get; set; }
        public string? CompanyName { get; set; }
        public int? Breakfast { get; set; }
        public int? Lunch { get; set; }
        public int? Snacks { get; set; }
        public int? Dinner { get; set; }
    }

    public class CanteenWastageReport_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }

        [DefaultValue("")]
        public string? MealType { get; set; }
    }

    public class CanteenWastageReport_Response : BaseEntity
    {
        public DateTime? FWDate { get; set; }
        public string? MealType { get; set; }
        public string? ItemName { get; set; }
        public string? Quantity { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatorName { get; set; }
    }
    public class SafetyReport_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
        public int? BranchId { get; set; }
    }
    public class SafetyReport_Response : BaseEntity
    {
        public DateTime? CheckInDate { get; set; }
        public int? Employee { get; set; }
        public int? Worker { get; set; }
        public int? Visitor { get; set; }
    }
}
