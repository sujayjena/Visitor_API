﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.Persistence.Repositories
{
    public class ManageEarlyLeaveRepository : GenericRepository, IManageEarlyLeaveRepository
    {
        private IConfiguration _configuration;

        public ManageEarlyLeaveRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveEarlyLeave(EarlyLeave_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@LeaveTypeId", parameters.LeaveTypeId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@Reason", parameters.Reason);
            queryParameters.Add("@LeaveDesc", parameters.LeaveDesc);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveEarlyLeave", queryParameters);
        }

        public async Task<IEnumerable<EarlyLeave_Response>> GetEarlyLeaveList(EarlyLeave_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<EarlyLeave_Response>("GetEarlyLeaveList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<EarlyLeave_Response?> GetEarlyLeaveById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<EarlyLeave_Response>("GetEarlyLeaveById", queryParameters)).FirstOrDefault();
        }
    }
}
