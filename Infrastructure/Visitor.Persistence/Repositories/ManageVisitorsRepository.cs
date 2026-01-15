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
            queryParameters.Add("@VerifyOTP", parameters.VerifyOTP);
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
            queryParameters.Add("@IsVehicle", parameters.IsVehicle);
            queryParameters.Add("@VehicleNumber", parameters.VehicleNumber);
            queryParameters.Add("@VehicleTypeId", parameters.VehicleTypeId);
            queryParameters.Add("@IsLaptop", parameters.IsLaptop);
            queryParameters.Add("@IsPendrive", parameters.IsPendrive);
            queryParameters.Add("@LaptopSerialNo", parameters.LaptopSerialNo);
            queryParameters.Add("@Others", parameters.Others);
            queryParameters.Add("@VS_IsCheckedIn", parameters.VS_IsCheckedIn);
            queryParameters.Add("@VS_IsCheckedOut", parameters.VS_IsCheckedOut);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@IsCompany", parameters.IsCompany);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@IsDrivingLicense", parameters.IsDrivingLicense);
            queryParameters.Add("@IsPUC", parameters.IsPUC);
            queryParameters.Add("@IsInsurance", parameters.IsInsurance);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@CompanyIdOriginalFileName", parameters.CompanyIdOriginalFileName);
            queryParameters.Add("@CompanyIdFileName", parameters.CompanyIdFileName);
            queryParameters.Add("@IsPlanned", parameters.IsPlanned);
            queryParameters.Add("@VehiclePhotoOriginalFileName", parameters.VehiclePhotoOriginalFileName);
            queryParameters.Add("@VehiclePhotoFileName", parameters.VehiclePhotoFileName);
            queryParameters.Add("@IsMeetingOver", parameters.IsMeetingOver);
            queryParameters.Add("@CP_IsBreakfast", parameters.CP_IsBreakfast);
            queryParameters.Add("@CP_IsLunch", parameters.CP_IsLunch);
            queryParameters.Add("@CP_IsSnacks", parameters.CP_IsSnacks);
            queryParameters.Add("@CP_IsDinner", parameters.CP_IsDinner);
            queryParameters.Add("@IsAllowCanteenPrivilege", parameters.IsAllowCanteenPrivilege);
            queryParameters.Add("@IsCrew", parameters.IsCrew);
            queryParameters.Add("@NationalityId", parameters.NationalityId);
            queryParameters.Add("@IsPassport", parameters.IsPassport);
            queryParameters.Add("@PassportNo", parameters.PassportNo);
            queryParameters.Add("@PlaceOfIssue", parameters.PlaceOfIssue);
            queryParameters.Add("@PassportIssueDate", parameters.PassportIssueDate);
            queryParameters.Add("@PassportValidTill", parameters.PassportValidTill);
            queryParameters.Add("@PassportOriginalFileName", parameters.PassportOriginalFileName);
            queryParameters.Add("@PassportFileName", parameters.PassportFileName);
            queryParameters.Add("@VesselId", parameters.VesselId);
            queryParameters.Add("@IsForeigner", parameters.IsForeigner);
            queryParameters.Add("@IsVisa", parameters.IsVisa);
            queryParameters.Add("@VisaNo", parameters.VisaNo);
            queryParameters.Add("@VisaValidFrom", parameters.VisaValidFrom);
            queryParameters.Add("@VisaValidTill", parameters.VisaValidTill);
            queryParameters.Add("@VisaOriginalFileName", parameters.VisaOriginalFileName);
            queryParameters.Add("@VisaFileName", parameters.VisaFileName);
            queryParameters.Add("@IsGovOfficials", parameters.IsGovOfficials);
            queryParameters.Add("@IsActive", parameters.IsActive);
            if(parameters.IsWithoutToken ==  true)
            {
                queryParameters.Add("@UserId", 0);
            }
            else
            {
                queryParameters.Add("@UserId", SessionManager.LoggedInUserId);
            }

            return await SaveByStoredProcedure<int>("SaveVisitors", queryParameters);
        }

        public async Task<IEnumerable<VisitorsList_Response>> GetVisitorsList(Visitors_Search parameters)
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
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@IsFilterType", parameters.IsFilterType);
            queryParameters.Add("@IsCrew", parameters.IsCrew);
            queryParameters.Add("@IsForeigner", parameters.IsForeigner);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsPlanned", parameters.IsPlanned);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorsList_Response>("GetVisitorsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Visitors_Response?> GetVisitorsById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);
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

        //public async Task<int> SaveVisitorsGateNo(VisitorGateNo_Request parameters)
        //{
        //    DynamicParameters queryParameters = new DynamicParameters();
        //    queryParameters.Add("@Action", parameters.Action);
        //    queryParameters.Add("@VisitorId", parameters.VisitorId);
        //    queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
        //    queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

        //    return await SaveByStoredProcedure<int>("SaveVisitorsGateNo", queryParameters);
        //}

        //public async Task<IEnumerable<VisitorGateNo_Response>> GetVisitorsGateNoByVisitorId(long VisitorId, long GateDetailsId)
        //{
        //    DynamicParameters queryParameters = new DynamicParameters();
        //    queryParameters.Add("@VisitorId", VisitorId);
        //    queryParameters.Add("@GateDetailsId", GateDetailsId);
        //    queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

        //    var result = await ListByStoredProcedure<VisitorGateNo_Response>("GetVisitorsGateNoByVisitorId", queryParameters);

        //    return result;
        //}

        public async Task<IEnumerable<VisitorApproveNRejectHistory_Response>> GetVisitorApproveNRejectHistoryListById(VisitorApproveNRejectHistory_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitorId", parameters.VisitorId);

            var result = await ListByStoredProcedure<VisitorApproveNRejectHistory_Response>("GetVisitorApproveNRejectHistoryListById", queryParameters);
            return result;
        }

        public async Task<int> SaveVisitorLogHistory(int VisitorId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitorId", VisitorId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorLogHistory", queryParameters);
        }

        public async Task<IEnumerable<VisitorLogHistory_Response>> GetVisitorLogHistoryList(VisitorLogHistory_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorLogHistory_Response>("GetVisitorLogHistoryList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<IEnumerable<VisitorPlanned_Response>> GetVisitorPlannedList(VisitorPlanned_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitDate", parameters.VisitDate);
            //queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@IsPlanned_CheckIn_CheckOut", parameters.IsPlanned_CheckIn_CheckOut);
            queryParameters.Add("@IsPlanned", parameters.IsPlanned);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorPlanned_Response>("GetVisitorPlannedList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveVisitorCheckedInOut(VisitorCheckedInOut_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@IsCheckedIn_Out", parameters.IsCheckedIn_Out);
            queryParameters.Add("@CheckedInDate", parameters.CheckedInDate);
            queryParameters.Add("@CheckedRemark", parameters.CheckedRemark);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorCheckedInOut", queryParameters);
        }

        public async Task<IEnumerable<CheckedInOutLogHistory_Response>> GetCheckedInOutLogHistoryList(CheckedInOutLogHistory_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@IsReject", parameters.IsReject);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CheckedInOutLogHistory_Response>("GetCheckedInOutLogHistoryList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveVisitorDocumentVerification(VisitorDocumentVerification_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            //queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@IDTypeId", parameters.IDTypeId);
            queryParameters.Add("@DocumentNumber", parameters.DocumentNumber);
            queryParameters.Add("@DocumentOriginalFileName", parameters.DocumentOriginalFileName);
            queryParameters.Add("@DocumentFileName", parameters.DocumentFileName);
            queryParameters.Add("@IsDocumentStatus", parameters.IsDocumentStatus);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorDocumentVerification", queryParameters);
        }

        public async Task<IEnumerable<VisitorDocumentVerification_Response>> GetVisitorDocumentVerificationList(VisitorDocumentVerification_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorDocumentVerification_Response>("GetVisitorDocumentVerificationList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveVisitorAsset(VisitorAsset_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@AssetName", parameters.AssetName);
            queryParameters.Add("@AssetDesc", parameters.AssetDesc);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorAsset", queryParameters);
        }

        public async Task<IEnumerable<VisitorAsset_Response>> GetVisitorAssetList(VisitorAsset_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorAsset_Response>("GetVisitorAssetList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<IEnumerable<SelectListResponse>> GetVisitorMobileNoListForSelectList()
        {
            DynamicParameters queryParameters = new DynamicParameters();

            return await ListByStoredProcedure<SelectListResponse>("GetVisitorMobileNoListForSelectList", queryParameters);
        }

        public async Task<IEnumerable<PreviousVisitor_Response>> GetPreviousVisitorList(PreviousVisitor_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@IsPlanned_CheckIn_CheckOut", parameters.IsPlanned_CheckIn_CheckOut);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<PreviousVisitor_Response>("GetPreviousVisitorList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<IEnumerable<MeetingPurposeLogHistory_Response>> GetMeetingPurposeLogHistoryList(MeetingPurposeLogHistory_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VisitorId", parameters.VisitorId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MeetingPurposeLogHistory_Response>("GetMeetingPurposeLogHistoryList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> DeleteVisitorDocumentVerification(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", Id);

            return await SaveByStoredProcedure<int>("DeleteVisitorDocumentVerification", queryParameters);
        }

        public async Task<IEnumerable<Visitor_ImportDataValidation>> ImportVisitor(List<Visitor_ImportData> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlData", xmlData);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await ListByStoredProcedure<Visitor_ImportDataValidation>("ImportVisitor", queryParameters);
        }

        public async Task<IEnumerable<AutoDailyReport_Response>> AutoDailyReport(AutoDailyReport_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RefType", parameters.RefType);
            var result = await ListByStoredProcedure<AutoDailyReport_Response>("AutoDailyReport", queryParameters);

            return result;
        }
        public async Task<IEnumerable<AutoDailyReport_Worker_Response>> AutoDailyReport_Worker(string JobType = "")
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@JobType", JobType);
            var result = await ListByStoredProcedure<AutoDailyReport_Worker_Response>("AutoDailyReport_Worker", queryParameters);

            return result;
        }
        public async Task<IEnumerable<AutoDailyReport_Employee_Response>> AutoDailyReport_Employee(string JobType = "")
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@JobType", JobType);
            var result = await ListByStoredProcedure<AutoDailyReport_Employee_Response>("AutoDailyReport_Employee", queryParameters);

            return result;
        }

        public async Task<int> SaveVisitorCheckedInOut_Offline(VisitorCheckedInOut_Offline_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@IsCheckedIn_Out", parameters.IsCheckedIn_Out);
            queryParameters.Add("@CheckedInDate", parameters.CheckedInDate);
            queryParameters.Add("@CheckedOutDate", parameters.CheckedOutDate);
            queryParameters.Add("@CheckedRemark", parameters.CheckedRemark);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorCheckedInOut_Offline", queryParameters);
        }

        public async Task<int> DeleteVisitorAsset(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", Id);

            return await SaveByStoredProcedure<int>("DeleteVisitorAsset", queryParameters);
        }

        public async Task<int> SendPassportAndVisaExpiry_Notification()
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SendPassportAndVisaExpiry_Notification", queryParameters);
        }
    }
}
