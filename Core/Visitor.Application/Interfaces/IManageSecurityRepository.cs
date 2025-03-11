using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IManageSecurityRepository
    {
        Task<int> SaveSecurityLogin(SecurityLogin_Request parameters);
        Task<IEnumerable<SecurityLogin_Response>> GetSecurityLoginList(SecurityLogin_Search parameters);
        Task<SecurityLogin_Response?> GetSecurityLoginById(int Id);

        Task<int> SaveSecurityLoginGateDetails(SecurityLoginGateDetails_Request parameters);
        Task<IEnumerable<SecurityLoginGateDetails_Response>> GetSecurityLoginGateDetailsById(int SecurityLoginId, int SecurityLoginGateDetailsId);
    }
}
