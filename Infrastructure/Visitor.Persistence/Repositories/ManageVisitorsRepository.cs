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
    public class ManageVisitorsRepository : GenericRepository, IManageVisitorsRepository
    {
        private IConfiguration _configuration;

        public ManageVisitorsRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveVisitors(Visitors_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VisitStartDate", parameters.VisitStartDate);
            queryParameters.Add("@VisitEndDate", parameters.VisitEndDate);
            queryParameters.Add("@VisitTypeId", parameters.VisitTypeId);
            queryParameters.Add("@VisitorName", parameters.VisitorName);
            queryParameters.Add("@VisitorMobileNo", parameters.VisitorMobileNo);
            queryParameters.Add("@VisitorEmailId", parameters.VisitorEmailId);
            queryParameters.Add("@GenderId", parameters.GenderId);
            queryParameters.Add("@VisitorCompanyId", parameters.VisitorCompanyId);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@AddressLine", parameters.AddressLine);
            queryParameters.Add("@IDTypeId", parameters.IDTypeId);
            queryParameters.Add("@VisitorPhotoOriginalFileName", parameters.VisitorPhotoOriginalFileName);
            queryParameters.Add("@VisitorPhoto", parameters.VisitorPhoto);
            queryParameters.Add("@MeetingTypeId", parameters.MeetingTypeId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@Purpose", parameters.Purpose);
            queryParameters.Add("@PassTypeId", parameters.PassTypeId);
            queryParameters.Add("@StartDate", parameters.StartDate);
            queryParameters.Add("@EndDate", parameters.EndDate);
            queryParameters.Add("@Duration", parameters.Duration);
            queryParameters.Add("@MeetingStatusId", parameters.MeetingStatusId);
            queryParameters.Add("@VehicleNumber", parameters.VehicleNumber);
            queryParameters.Add("@VehicleTypeId", parameters.VehicleTypeId);
            queryParameters.Add("@IsLaptop", parameters.IsLaptop);
            queryParameters.Add("@IsPendrive", parameters.IsPendrive);
            queryParameters.Add("@Others", parameters.Others);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitors", queryParameters);
        }

        public async Task<IEnumerable<Visitors_Response>> GetVisitorsList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Visitors_Response>("GetVisitorsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Visitors_Response?> GetVisitorsById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Visitors_Response>("GetVisitorsById", queryParameters)).FirstOrDefault();
        }

    }
}
