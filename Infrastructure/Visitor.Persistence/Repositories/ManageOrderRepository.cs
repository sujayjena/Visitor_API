using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.Persistence.Repositories
{
    public class ManageOrderRepository : GenericRepository, IManageOrderRepository
    {
        private IConfiguration _configuration;

        public ManageOrderRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveFoodOrder(FoodOrder_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@OrderDate", parameters.OrderDate);
            queryParameters.Add("@CanteenId", parameters.CanteenId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@CanteenCouponId", parameters.CanteenCouponId);
            queryParameters.Add("@DeliveryAddress", parameters.DeliveryAddress);
            queryParameters.Add("@TotalAmount", parameters.TotalAmount);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFoodOrder", queryParameters);
        }

        public async Task<IEnumerable<FoodOrder_Response>> GetFoodOrderList(FoodOrderSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CanteenId", parameters.CanteenId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoodOrder_Response>("GetFoodOrderList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<FoodOrder_Response?> GetFoodOrderById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<FoodOrder_Response>("GetFoodOrderById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveFoodOrderItem(FoodOrderItem_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@FoodOrderId", parameters.FoodOrderId);
            queryParameters.Add("@FoodItemId", parameters.FoodItemId);
            queryParameters.Add("@Price", parameters.Price);
            queryParameters.Add("@Quantity", parameters.Quantity);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFoodOrderItem", queryParameters);
        }

        public async Task<IEnumerable<FoodOrderItem_Response>> GetFoodOrderItemList(FoodOrderItemSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FoodOrderId", parameters.FoodOrderId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoodOrderItem_Response>("GetFoodOrderItemList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }
    }
}
