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
    public class ManageGroceryRepository : GenericRepository, IManageGroceryRepository
    {
        private IConfiguration _configuration;

        public ManageGroceryRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Grocery Requisition
        public async Task<int> SaveGroceryRequisition(GroceryRequisition_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveGroceryRequisition", queryParameters);
        }

        public async Task<IEnumerable<GroceryRequisitionList_Response>> GetGroceryRequisitionList(GroceryRequisition_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GroceryRequisitionList_Response>("GetGroceryRequisitionList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<GroceryRequisition_Response?> GetGroceryRequisitionById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<GroceryRequisition_Response>("GetGroceryRequisitionById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Grocery Requisition Details
        public async Task<int> SaveGroceryRequisitionDetails(GroceryRequisitionDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@GroceryRequisitionId", parameters.GroceryRequisitionId);
            queryParameters.Add("@GroceryId", parameters.GroceryId);
            queryParameters.Add("@OrderQty", parameters.OrderQty);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveGroceryRequisitionDetails", queryParameters);
        }

        public async Task<IEnumerable<GroceryRequisitionDetails_Response>> GetGroceryRequisitionDetailsList(GroceryRequisitionDetails_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StatusId", parameters.GroceryRequisitionId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GroceryRequisitionDetails_Response>("GetGroceryRequisitionDetailsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion
    }
}
