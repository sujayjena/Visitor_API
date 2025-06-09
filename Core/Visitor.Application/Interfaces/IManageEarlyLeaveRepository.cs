using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IManageEarlyLeaveRepository
    {
        Task<int> SaveEarlyLeave(EarlyLeave_Request parameters);
        Task<IEnumerable<EarlyLeave_Response>> GetEarlyLeaveList(EarlyLeave_Search parameters);
        Task<EarlyLeave_Response?> GetEarlyLeaveById(int Id);
    }
}
