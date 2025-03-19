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
            queryParameters.Add("@IsVisitor_Contractor_Vendor", parameters.IsVisitor_Contractor_Vendor);
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
            queryParameters.Add("@MP_IsApproved", parameters.MP_IsApproved);
            queryParameters.Add("@PassTypeId", parameters.PassTypeId);
            queryParameters.Add("@StartDate", parameters.StartDate);
            queryParameters.Add("@EndDate", parameters.EndDate);
            queryParameters.Add("@Duration", parameters.Duration);
            queryParameters.Add("@MeetingStatusId", parameters.MeetingStatusId);
            queryParameters.Add("@VehicleNumber", parameters.VehicleNumber);
            queryParameters.Add("@VehicleTypeId", parameters.VehicleTypeId);
            queryParameters.Add("@IsLaptop", parameters.IsLaptop);
            queryParameters.Add("@IsPendrive", parameters.IsPendrive);
            queryParameters.Add("@LaptopSerialNo", parameters.LaptopSerialNo);
            queryParameters.Add("@Others", parameters.Others);
            queryParameters.Add("@VS_IsCheckedIn", parameters.VS_IsCheckedIn);
            queryParameters.Add("@VS_IsCheckedOut", parameters.VS_IsCheckedOut);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitors", queryParameters);
        }

        public async Task<IEnumerable<Visitors_Response>> GetVisitorsList(Visitors_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@MobileNo", parameters.MobileNo);
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@PassTypeId", parameters.PassTypeId);
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@GateDetailsId_Filter", parameters.GateDetailsId_Filter);
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

        public async Task<int> VisitorsApproveNReject(Visitor_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            //queryParameters.Add("@BarcodeOriginalFileName", parameters.BarcodeOriginalFileName);
            //queryParameters.Add("@BarcodeFileName", parameters.BarcodeFileName);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("VisitorsApproveNReject", queryParameters);
        }

        public async Task<Visitors_Response?> GetVisitorDetailByMobileNumber(string MobileNumber)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@MobileNumber", MobileNumber);
            return (await ListByStoredProcedure<Visitors_Response>("GetVisitorDetailByMobileNumber", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveVisitorsGateNo(VisitorGateNo_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorsGateNo", queryParameters);
        }

        public async Task<IEnumerable<VisitorGateNo_Response>> GetVisitorsGateNoByVisitorId(long VisitorId, long GateDetailsId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitorId", VisitorId);
            queryParameters.Add("@GateDetailsId", GateDetailsId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorGateNo_Response>("GetVisitorsGateNoByVisitorId", queryParameters);

            return result;
        }

        public async Task<IEnumerable<VisitorApproveNRejectHistory_Response>> GetVisitorApproveNRejectHistoryListById(VisitorApproveNRejectHistory_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitorId", parameters.VisitorId);

            var result = await ListByStoredProcedure<VisitorApproveNRejectHistory_Response>("GetVisitorApproveNRejectHistoryListById", queryParameters);
            return result;
        }

       
    }
}
