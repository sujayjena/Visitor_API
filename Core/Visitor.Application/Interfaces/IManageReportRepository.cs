using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IManageReportRepository
    {
        Task<IEnumerable<CanteenUsageReport_Response>> GetCanteenUsageReport(CanteenUsageReport_Search parameters);
    }
}
