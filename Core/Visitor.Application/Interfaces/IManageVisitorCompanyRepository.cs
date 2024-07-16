using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IManageVisitorCompanyRepository
    {
        Task<int> SaveVisitorCompany(VisitorCompany_Request parameters);

        Task<IEnumerable<VisitorCompany_Response>> GetVisitorCompanyList(BaseSearchEntity parameters);

        Task<VisitorCompany_Response?> GetVisitorCompanyById(int Id);
    }
}
