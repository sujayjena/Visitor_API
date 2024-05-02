using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Application.Interfaces
{
    public interface IBranchRepository
    {
        Task<int> SaveBranch(Branch_Request parameters);

        Task<IEnumerable<Branch_Response>> GetBranchList(BranchSearch_Request parameters);

        Task<Branch_Response?> GetBranchById(int Id);



        Task<int> SaveBranchMapping(BranchMapping_Request parameters);

        Task<IEnumerable<BranchMapping_Response>> GetBranchMappingByEmployeeId(int EmployeeId, int BranchId);
    }
}
