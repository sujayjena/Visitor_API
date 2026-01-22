using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Dashboard_TotalSummary_Result>> GetDashboard_TotalSummary(Dashboard_Search_Request parameters);
        Task<IEnumerable<Dashboard_TokenCountSummary_Result>> GetDashboard_TokenCountSummary(Dashboard_TokenCountSummary_Search parameters);
        Task<IEnumerable<Dashboard_Security_TotalSummary_Result>> GetDashboard_Security_TotalSummary(Dashboard_Security_Search_Request parameters);
        Task<IEnumerable<Dashboard_TokenCountSummary_Result>> GetDashboard_CanteenWastageSummary(Dashboard_CanteenWastageSummary_Search parameters);
    }
}
