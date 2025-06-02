using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IAssignGateNoRepository
    {
        Task<int> SaveAssignGateNo(AssignGateNo_Request parameters);
        Task<IEnumerable<AssignGateNo_Response>> GetAssignGateNoById(long RefId, string RefType, long GateDetailsId);
    }
}
