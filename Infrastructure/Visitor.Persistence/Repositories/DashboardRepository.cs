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
    public class DashboardRepository : GenericRepository, IDashboardRepository
    {
        private IConfiguration _configuration;

        public DashboardRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Dashboard_TotalSummary_Result>> GetDashboard_TotalSummary(Dashboard_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Dashboard_TotalSummary_Result>("GetDashboard_TotalSummary", queryParameters);

            return result;
        }
    }
}
