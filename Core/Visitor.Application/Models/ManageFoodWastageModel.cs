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
    public class FoodWastage_Search : BaseSearchEntity
    {
        [DefaultValue("")]
        public string? MealType { get; set; }
    }
    public class FoodWastage_Request : BaseEntity
    {
        public DateTime? FWDate { get; set; }
        public string? MealType { get; set; }
        public string? ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public int? UOMId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class FoodWastage_Response : BaseResponseEntity
    {
        public DateTime? FWDate { get; set; }
        public string? MealType { get; set; }
        public string? ItemName { get; set; }
        public decimal? Quantity { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public bool? IsActive { get; set; }
    }
}
