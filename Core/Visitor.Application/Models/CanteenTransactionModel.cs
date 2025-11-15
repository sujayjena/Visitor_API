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
    public class CanteenTransaction_Request : BaseEntity
    {
        [DefaultValue("Visitor")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }
        public int? FoodItemId { get; set; }
        public string? TokenNo { get; set; }

        [DefaultValue(false)]
        public bool? IsPaid { get; set; }
        public DateTime? CTDate { get; set; }
    }
    public class CanteenTransaction_Search : BaseSearchEntity
    {
        [DefaultValue("Visitor")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }
    }
    public class CanteenTransaction_Response : BaseEntity
    {
        public string? RefType { get; set; }
        public int? RefId { get; set; }
        public string? RefName { get; set; }
        public string? MealType { get; set; }
        public int? FoodItemId { get; set; }
        public int? MenuItemId { get; set; }
        public string? MenuItemName { get; set; }
        public decimal? SellingPrice { get; set; }
        public bool? IsSubsidized { get; set; }
        public decimal? SubsidizedPrice { get; set; }
        public string? TokenNo { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? CTDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public string? CreatorName { get; set; }
    }

    public class CanteenTransactionToken_Request : BaseEntity
    {
        [DefaultValue("")]
        public string? MealType { get; set; }
    }

    public class CanteenTransactionToken_Response : BaseEntity
    {
        public string? TokenNo { get; set; }
    }
}
