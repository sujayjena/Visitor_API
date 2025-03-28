﻿using Visitor.Application.Models;
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

        #endregion

        #region User Other Details
        Task<int> SaveUserOtherDetails(UserOtherDetails_Request parameters);

        Task<IEnumerable<UserOtherDetails_Response>> GetUserOtherDetailsByEmployeeId(int EmployeeId);

        Task<UserOtherDetails_Response?> GetUserOtherDetailsById(int Id);

        Task<int> DeleteUserOtherDetails(int Id);

        #endregion
    }
}
