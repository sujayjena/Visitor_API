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
    public class ManageReportRepository : GenericRepository, IManageReportRepository
    {
        private IConfiguration _configuration;

        public ManageReportRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<CanteenUsageReport_Response>> GetCanteenUsageReport(CanteenUsageReport_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@IsExportType", parameters.IsExportType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CanteenUsageReport_Response>("GetCanteenUsageReport", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<IEnumerable<CanteenWastageReport_Response>> GetCanteenWastageReport(CanteenWastageReport_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@MealType", parameters.MealType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CanteenWastageReport_Response>("GetCanteenWastageReport", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }
    }
}
