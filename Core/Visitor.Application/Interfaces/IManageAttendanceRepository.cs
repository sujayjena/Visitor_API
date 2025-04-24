using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IManageAttendanceRepository
    {
        Task<int> SaveAttendanceDetails(ManageAttendance_Request parameters);
        Task<IEnumerable<ManageAttendance_Response>> GetAttendanceDetailsList(ManageAttendance_Search parameters);
    }
}
