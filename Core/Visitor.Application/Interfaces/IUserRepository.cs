using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Application.Interfaces
{
    public interface IUserRepository  
    {
        #region User 

        Task<int> SaveUser(User_Request parameters);

        Task<IEnumerable<User_Response>> GetUserList(User_Search parameters);

        Task<User_Response?> GetUserById(long Id);

        Task<int> ChangePassword(ChangePassword_Request parameters);

        Task<int> ForgotPassword(ForgotPassword_Request parameters);

        Task<string?> GetAutoGenPassword(string AutoPassword);

        Task<IEnumerable<User_ImportDataValidation>> ImportUser(List<User_ImportData> parameters);

        #endregion

        #region User Other Details
        Task<int> SaveUserOtherDetails(UserOtherDetails_Request parameters);

        Task<IEnumerable<UserOtherDetails_Response>> GetUserOtherDetailsByEmployeeId(int EmployeeId);

        Task<UserOtherDetails_Response?> GetUserOtherDetailsById(int Id);

        Task<int> DeleteUserOtherDetails(int Id);

        #endregion

        #region Employee Gate Number
        Task<int> SaveEmployeeGateNo(EmployeeGateNo_Request parameters);
        Task<IEnumerable<EmployeeGateNo_Response>> GetEmployeeGateNoByEmployeeId(long EmployeeId, long GateDetailsId);
        #endregion
    }
}
