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
    public class CanteenTransactionRepository : GenericRepository, ICanteenTransactionRepository
    {
        private IConfiguration _configuration;

        public CanteenTransactionRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveCanteenTransaction(CanteenTransaction_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@FoodItemId", parameters.FoodItemId);
            queryParameters.Add("@TokenNo", parameters.TokenNo);
            queryParameters.Add("@IsPaid", parameters.IsPaid);
            queryParameters.Add("@CTDate", parameters.CTDate);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCanteenTransaction", queryParameters);
        }

        public async Task<IEnumerable<CanteenTransaction_Response>> GetCanteenTransactionList(CanteenTransaction_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CanteenTransaction_Response>("GetCanteenTransactionList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveCanteenTransactionToken(CanteenTransactionToken_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MealType", parameters.MealType);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCanteenTransactionToken", queryParameters);
        }

        public async Task<CanteenTransactionToken_Response?> GetCanteenTransactionTokenById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<CanteenTransactionToken_Response>("GetCanteenTransactionTokenById", queryParameters)).FirstOrDefault();
        }
    }
}
