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
    }
}
