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
    public class ManagePurchaseOrderRepository : GenericRepository, IManagePurchaseOrderRepository
    {
        private IConfiguration _configuration;

        public ManagePurchaseOrderRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SavePurchaseOrder(PurchaseOrder_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ContractorId", parameters.ContractorId);
            queryParameters.Add("@PONumber", parameters.PONumber);
            queryParameters.Add("@POName", parameters.POName);
            queryParameters.Add("@ValidFromDate", parameters.ValidFromDate);
            queryParameters.Add("@ValidToDate", parameters.ValidToDate);
            queryParameters.Add("@DocumentOriginalFileName", parameters.DocumentOriginalFileName);
            queryParameters.Add("@DocumentFileName", parameters.DocumentFileName);
            queryParameters.Add("@NoofPOWorker", parameters.NoofPOWorker);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SavePurchaseOrder", queryParameters);
        }

        public async Task<IEnumerable<PurchaseOrder_Response>> GetPurchaseOrderList(PurchaseOrderSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@ContractorId", parameters.ContractorId);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<PurchaseOrder_Response>("GetPurchaseOrderList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<PurchaseOrder_Response?> GetPurchaseOrderById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<PurchaseOrder_Response>("GetPurchaseOrderById", queryParameters)).FirstOrDefault();
        }
    }
}
