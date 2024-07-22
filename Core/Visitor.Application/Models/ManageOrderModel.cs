using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    public class ManageOrderModel
    {
    }

    public class FoodOrder_Request : BaseEntity
    {
        public FoodOrder_Request()
        {
            foodItemList = new List<FoodOrderItem_Request>();
        }
        public DateTime? OrderDate { get; set; }
        public int? CanteenId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CanteenCouponId { get; set; }

        [DefaultValue("")]
        public string? DeliveryAddress { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsActive { get; set; }

        public List<FoodOrderItem_Request> foodItemList { get; set; }
    }

    public class FoodOrderSearch_Request : BaseSearchEntity
    {
        public int? CanteenId { get; set; }
        public int? EmployeeId { get; set; }
    }

    public class FoodOrder_Response : BaseResponseEntity
    {
        public FoodOrder_Response()
        {
            foodItemList = new List<FoodOrderItem_Response>();
        }
        public string? OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CanteenId { get; set; }
        public string? CanteenName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? CanteenCouponId { get; set; }
        public string? CouponCode { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsActive { get; set; }

        public List<FoodOrderItem_Response> foodItemList { get; set; }
    }

    public class FoodOrderItem_Request : BaseEntity
    {
        [JsonIgnore]
        public int? FoodOrderId { get; set; }
        public int? FoodItemId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public bool? IsActive { get; set; }
    }

    public class FoodOrderItemSearch_Request : BaseSearchEntity
    {
        public int? FoodOrderId { get; set; }
    }

    public class FoodOrderItem_Response : BaseEntity
    {
        public int? FoodOrderId { get; set; }
        public int? FoodItemId { get; set; }
        public string? FoodItemName { get; set; }
        public string? FoodItemDesc { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public bool? IsActive { get; set; }
    }

}
