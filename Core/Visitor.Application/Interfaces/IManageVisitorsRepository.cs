using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IManageVisitorsRepository
    {
        Task<int> SaveVisitors(Visitors_Request parameters);

        Task<IEnumerable<Visitors_Response>> GetVisitorsList(BaseSearchEntity parameters);

        Task<Visitors_Response?> GetVisitorsById(int Id);
    }
}
