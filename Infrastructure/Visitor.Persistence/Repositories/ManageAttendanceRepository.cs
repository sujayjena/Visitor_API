using Dapper;
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
    public class ManageAttendanceRepository : GenericRepository, IManageAttendanceRepository
    {

        private IConfiguration _configuration;

        public ManageAttendanceRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }


        public async Task<int> SaveAttendanceDetails(ManageAttendance_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@AttendanceStatus", parameters.AttendanceStatus);
            queryParameters.Add("@BatteryStatus", parameters.BatteryStatus);
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@Remarks", parameters.Remarks);

            return await SaveByStoredProcedure<int>("SaveAttendanceDetails", queryParameters);
        }

        public async Task<IEnumerable<ManageAttendance_Response>> GetAttendanceDetailsList(ManageAttendance_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);

            var result = await ListByStoredProcedure<ManageAttendance_Response>("GetAttendanceDetailsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }
    }
}
