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
    public class ManageFoodWastageRepository : GenericRepository, IManageFoodWastageRepository
    {
        private IConfiguration _configuration;

        public ManageFoodWastageRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveFoodWastage(FoodWastage_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@FWDate", parameters.FWDate);
            queryParameters.Add("@MealType", parameters.MealType);
            queryParameters.Add("@ItemName", parameters.ItemName);
            queryParameters.Add("@Quantity", parameters.Quantity);
            queryParameters.Add("@UOMId", parameters.UOMId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFoodWastage", queryParameters);
        }

        public async Task<IEnumerable<FoodWastage_Response>> GetFoodWastageList(FoodWastage_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@MealType", parameters.MealType);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoodWastage_Response>("GetFoodWastageList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<FoodWastage_Response?> GetFoodWastageById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<FoodWastage_Response>("GetFoodWastageById", queryParameters)).FirstOrDefault();
        }
    }
}
