using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    public class ManageVisitorsModel
    {
    }

    public class Visitors_Search : BaseSearchEntity
    {
        [DefaultValue("")]
        public string? MobileNo { get; set; }
        public int? VisitorId { get; set; }
        public int? GateDetailsId { get; set; }
        public int? PassTypeId { get; set; }

        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
        public int? StatusId { get; set; }

        [DefaultValue(null)]
        public bool? IsCheckedIn { get; set; }

        [DefaultValue(null)]
        public bool? IsCheckedOut { get; set; }

        [DefaultValue("")]
        public string? GateDetailsId_Filter { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }

        [DefaultValue("")]
        public string? IsFilterType { get; set; }

        [DefaultValue(null)]
        public bool? IsCrew { get; set; }

        [DefaultValue(null)]
        public bool? IsForeigner { get; set; }

        [DefaultValue(null)]
        public bool? IsGovOfficials { get; set; }

        [DefaultValue(false)]
        public bool? IsLiveReport { get; set; }

        public bool? IsPlanned { get; set; }
    }

    public class Visitors_Request : BaseEntity
    {
        public Visitors_Request()
        {
            GateNumberList = new List<AssignGateNo_Request>();
            DocumentVerificationList = new List<VisitorDocumentVerification_Request>();
            AssetList = new List<VisitorAsset_Request>();
        }

        public DateTime? VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public int? IsVisitor_Contractor_Vendor { get; set; }
        public int? VisitTypeId { get; set; }

        public string? VisitorName { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VerifyOTP { get; set; }
        public string? VisitorEmailId { get; set; }
        public int? GenderId { get; set; }


        public int? VisitorCompanyId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public string? Pincode { get; set; }
        public string? AddressLine { get; set; }
        public int? IDTypeId { get; set; }

        public string? VisitorPhotoOriginalFileName { get; set; }
        public string? VisitorPhoto { get; set; }
        public string? VisitorPhoto_Base64 { get; set; }

        public int? MeetingTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? EmployeeId { get; set; }
        public string? Purpose { get; set; }

        [DefaultValue(false)]
        public bool? MP_IsApproved { get; set; }

        public int? PassTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Duration { get; set; }
        public int? MeetingStatusId { get; set; }

        [DefaultValue(false)]
        public bool? IsVehicle { get; set; }
        public string? VehicleNumber { get; set; }
        public int? VehicleTypeId { get; set; }

        [DefaultValue(false)]
        public bool? IsLaptop { get; set; }

        [DefaultValue(false)]
        public bool? IsPendrive { get; set; }

        public string? LaptopSerialNo { get; set; }
        public string? Others { get; set; }

        [DefaultValue(false)]
        public bool? VS_IsCheckedIn { get; set; }

        [DefaultValue(false)]
        public bool? VS_IsCheckedOut { get; set; }
        public int? StatusId { get; set; }

        [DefaultValue(false)]
        public bool? IsCompany { get; set; }
        public int? BranchId { get; set; }

        [DefaultValue(false)]
        public bool? IsDrivingLicense { get; set; }

        [DefaultValue(false)]
        public bool? IsPUC { get; set; }

        [DefaultValue(false)]
        public bool? IsInsurance { get; set; }
        public string? Remarks { get; set; }
        public string? CompanyIdOriginalFileName { get; set; }
        public string? CompanyIdFileName { get; set; }
        public string? CompanyId_Base64 { get; set; }
        public bool? IsPlanned { get; set; }
        public string? VehiclePhotoOriginalFileName { get; set; }
        public string? VehiclePhotoFileName { get; set; }
        public string? VehiclePhoto_Base64 { get; set; }
        public bool? IsMeetingOver { get; set; }

        [DefaultValue(false)]
        public bool? IsWithoutToken { get; set; }

        [DefaultValue(false)]
        public bool? CP_IsBreakfast { get; set; }

        [DefaultValue(false)]
        public bool? CP_IsLunch { get; set; }

        [DefaultValue(false)]
        public bool? CP_IsSnacks { get; set; }

        [DefaultValue(false)]
        public bool? CP_IsDinner { get; set; }

        [DefaultValue(false)]
        public bool? IsAllowCanteenPrivilege { get; set; }

        [DefaultValue(false)]
        public bool? IsCrew { get; set; }
        public int? NationalityId { get; set; }

        [DefaultValue(false)]
        public bool? IsPassport { get; set; }
        public string? PassportNo { get; set; }
        public string? PlaceOfIssue { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportValidTill { get; set; }

        [DefaultValue("")]
        public string? PassportOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? PassportFileName { get; set; }

        [DefaultValue("")]
        public string? Passport_Base64 { get; set; }
        public int? VesselId { get; set; }
        public DateTime? VesselStartDate { get; set; }
        public DateTime? VesselEndDate { get; set; }

        [DefaultValue(false)]
        public bool? IsForeigner { get; set; }

        [DefaultValue(false)]
        public bool? IsVisa { get; set; }
        public string? VisaNo { get; set; }
        public DateTime? VisaValidFrom { get; set; }
        public DateTime? VisaValidTill { get; set; }

        [DefaultValue("")]
        public string? VisaOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? VisaFileName { get; set; }

        [DefaultValue("")]
        public string? Visa_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? IsGovOfficials { get; set; }
        public bool? IsActive { get; set; }
        public List<AssignGateNo_Request> GateNumberList { get; set; }
        public List<VisitorDocumentVerification_Request> DocumentVerificationList { get; set; }
        public List<VisitorAsset_Request> AssetList { get; set; }
    }
    public class VisitorAsset_Search : BaseSearchEntity
    {
        public int? VisitorId { get; set; }
    }
    public class VisitorsList_Response : BaseResponseEntity
    {
        public string? VisitNumber { get; set; }
        public DateTime? VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public int? IsVisitor_Contractor_Vendor { get; set; }
        public int? VisitTypeId { get; set; }
        public string? VisitType { get; set; }


        public string? VisitorName { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VerifyOTP { get; set; }
        public string? VisitorEmailId { get; set; }
        public int? GenderId { get; set; }
        public string? GenderName { get; set; }

        public int? VisitorCompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public string? Pincode { get; set; }
        public string? AddressLine { get; set; }
        public int? IDTypeId { get; set; }
        public string? IDType { get; set; }
        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
        public string? BarcodeURL { get; set; }
        public string? VisitorPhotoOriginalFileName { get; set; }
        public string? VisitorPhoto { get; set; }
        public string? VisitorPhotoURL { get; set; }
        public int? MeetingTypeId { get; set; }
        public string? MeetingType { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Employee_MobileNumber { get; set; }
        public string? Employee_EmailId { get; set; }
        public string? Purpose { get; set; }
        public bool? MP_IsApproved { get; set; }
        public int? PassTypeId { get; set; }
        public string? PassType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Duration { get; set; }
        public int? MeetingStatusId { get; set; }
        public string? MeetingStatus { get; set; }
        public bool? IsVehicle { get; set; }
        public string? VehicleNumber { get; set; }
        public int? VehicleTypeId { get; set; }
        public string? VehicleType { get; set; }
        public bool? IsLaptop { get; set; }
        public bool? IsPendrive { get; set; }
        public string? LaptopSerialNo { get; set; }
        public string? Others { get; set; }
        public bool? VS_IsCheckedIn { get; set; }
        public DateTime? CheckedInClosedDate { get; set; }
        public bool? VS_IsCheckedOut { get; set; }
        public DateTime? CheckedOutClosedDate { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsCompany { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public bool? IsDrivingLicense { get; set; }
        public bool? IsPUC { get; set; }
        public bool? IsInsurance { get; set; }
        public string? Remarks { get; set; }
        public string? CompanyIdOriginalFileName { get; set; }
        public string? CompanyIdFileName { get; set; }
        public string? CompanyIdURL { get; set; }
        public bool? IsPlanned { get; set; }
        public string? VehiclePhotoOriginalFileName { get; set; }
        public string? VehiclePhotoFileName { get; set; }
        public string? VehiclePhotoURL { get; set; }
        public bool? IsApprovedReject { get; set; }
        public string? ApprovedReject_Remarks { get; set; }
        public bool? IsMeetingOver { get; set; }
        public DateTime? IsMeetingOverDateAndTime { get; set; }
        public bool? IsMeetingOverDisabled { get; set; }
        public bool? IsCheckedInDisable { get; set; }
        public bool? CP_IsBreakfast { get; set; }
        public bool? CP_IsLunch { get; set; }
        public bool? CP_IsSnacks { get; set; }
        public bool? CP_IsDinner { get; set; }
        public bool? IsAllowCanteenPrivilege { get; set; }
        public bool? IsCrew { get; set; }
        public int? NationalityId { get; set; }
        public string? NationalityName { get; set; }
        public bool? IsPassport { get; set; }
        public string? PassportNo { get; set; }
        public string? PlaceOfIssue { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportValidTill { get; set; }
        public int? VesselId { get; set; }
        public string? VesselName { get; set; }
        public DateTime? VesselStartDate { get; set; }
        public DateTime? VesselEndDate { get; set; }
        public bool? IsForeigner { get; set; }
        public bool? IsVisa { get; set; }
        public string? VisaNo { get; set; }
        public DateTime? VisaValidFrom { get; set; }
        public DateTime? VisaValidTill { get; set; }
        public bool? IsGovOfficials { get; set; }
        public bool? IsActive { get; set; }

        public List<AssignGateNo_Response> GateNumberList { get; set; }
        public List<VisitorDocumentVerification_Response> DocumentVerificationList { get; set; }
        public List<VisitorAsset_Response> AssetList { get; set; }
    }
    public class Visitors_Response : BaseResponseEntity
    {
        public string? VisitNumber { get; set; }
        public DateTime? VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public int? IsVisitor_Contractor_Vendor { get; set; }
        public int? VisitTypeId { get; set; }
        public string? VisitType { get; set; }

        public string? VisitorName { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VerifyOTP { get; set; }
        public string? VisitorEmailId { get; set; }
        public int? GenderId { get; set; }
        public string? GenderName { get; set; }

        public int? VisitorCompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public string? Pincode { get; set; }
        public string? AddressLine { get; set; }
        public int? IDTypeId { get; set; }
        public string? IDType { get; set; }
        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
        public string? BarcodeURL { get; set; }
        public string? VisitorPhotoOriginalFileName { get; set; }
        public string? VisitorPhoto { get; set; }
        public string? VisitorPhotoURL { get; set; }
        public int? MeetingTypeId { get; set; }
        public string? MeetingType { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Employee_MobileNumber { get; set; }
        public string? Employee_EmailId { get; set; }
        public string? Purpose { get; set; }
        public bool? MP_IsApproved { get; set; }
        public int? PassTypeId { get; set; }
        public string? PassType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Duration { get; set; }
        public int? MeetingStatusId { get; set; }
        public string? MeetingStatus { get; set; }
        public bool? IsVehicle { get; set; }
        public string? VehicleNumber { get; set; }
        public int? VehicleTypeId { get; set; }
        public string? VehicleType { get; set; }
        public bool? IsLaptop { get; set; }
        public bool? IsPendrive { get; set; }
        public string? LaptopSerialNo { get; set; }
        public string? Others { get; set; }
        public bool? VS_IsCheckedIn { get; set; }
        public DateTime? CheckedInClosedDate { get; set; }
        public bool? VS_IsCheckedOut { get; set; }
        public DateTime? CheckedOutClosedDate { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public bool? IsCompany { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public bool? IsDrivingLicense { get; set; }
        public bool? IsPUC { get; set; }
        public bool? IsInsurance { get; set; }
        public string? Remarks { get; set; }
        public string? CompanyIdOriginalFileName { get; set; }
        public string? CompanyIdFileName { get; set; }
        public string? CompanyIdURL { get; set; }
        public bool? IsPlanned { get; set; }
        public string? VehiclePhotoOriginalFileName { get; set; }
        public string? VehiclePhotoFileName { get; set; }
        public string? VehiclePhotoURL { get; set; }
        public bool? IsApprovedReject { get; set; }
        public string? ApprovedReject_Remarks { get; set; }
        public bool? IsMeetingOver { get; set; }
        public DateTime? IsMeetingOverDateAndTime { get; set; }
        public bool? IsMeetingOverDisabled { get; set; }
        public bool? IsCheckedInDisable { get; set; }
        public bool? CP_IsBreakfast { get; set; }
        public bool? CP_IsLunch { get; set; }
        public bool? CP_IsSnacks { get; set; }
        public bool? CP_IsDinner { get; set; }
        public bool? IsAllowCanteenPrivilege { get; set; }
        public bool? IsCrew { get; set; }
        public int? NationalityId { get; set; }
        public string? NationalityName { get; set; }
        public bool? IsPassport { get; set; }
        public string? PassportNo { get; set; }
        public string? PlaceOfIssue { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportValidTill { get; set; }
        public string? PassportOriginalFileName { get; set; }
        public string? PassportFileName { get; set; }
        public string? PassportURL { get; set; }
        public int? VesselId { get; set; }
        public string? VesselName { get; set; }
        public DateTime? VesselStartDate { get; set; }
        public DateTime? VesselEndDate { get; set; }
        public bool? IsForeigner { get; set; }
        public bool? IsVisa { get; set; }
        public string? VisaNo { get; set; }
        public DateTime? VisaValidFrom { get; set; }
        public DateTime? VisaValidTill { get; set; }
        public string? VisaOriginalFileName { get; set; }
        public string? VisaFileName { get; set; }
        public string? VisaURL { get; set; }
        public bool? IsGovOfficials { get; set; }
        public bool? IsActive { get; set; }

        public List<AssignGateNo_Response> GateNumberList { get; set; }
        public List<VisitorDocumentVerification_Response> DocumentVerificationList { get; set; }
        public List<VisitorAsset_Response> AssetList { get; set; }
    }
    public class VisitorDocumentVerification_Request : BaseEntity
    {
        [JsonIgnore]
        public int? RefId { get; set; }

        [JsonIgnore]
        [DefaultValue("Visitor")]
        public string? RefType { get; set; }

        //[JsonIgnore]
        //public int? VisitorId { get; set; }

        public int? IDTypeId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? DocumentOriginalFileName { get; set; }

        public string? DocumentFileName { get; set; }
        public string? DocumentFile_Base64 { get; set; }
        public int? IsDocumentStatus { get; set; }
    }

    public class VisitorDocumentVerification_Search : BaseSearchEntity
    {
        public int? RefId { get; set; }

        [DefaultValue("Visitor")]
        public string? RefType { get; set; }
        //public int? VisitorId { get; set; }
    }

    public class VisitorDocumentVerification_Response : BaseEntity
    {
        public int? RefId { get; set; }
        public string? RefType { get; set; }
        //public int? VisitorId { get; set; }
        public int? IDTypeId { get; set; }
        public string? IDType { get; set; }
        public string? DocumentNumber { get; set; }
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentFileName { get; set; }
        public string? DocumentURL { get; set; }
        public int? IsDocumentStatus { get; set; }
    }

    public class VisitorAsset_Request : BaseEntity
    {
        [JsonIgnore]
        public int? VisitorId { get; set; }
        public string? AssetName { get; set; }
        public string? AssetDesc { get; set; }
    }
    public class VisitorAsset_Response : BaseEntity
    {
        public int? VisitorId { get; set; }
        public string? AssetName { get; set; }
        public string? AssetDesc { get; set; }
    }

    //public class VisitorGateNo_Request
    //{
    //    public int Id { get; set; }

    //    [JsonIgnore]
    //    public string? Action { get; set; }

    //    [JsonIgnore]
    //    public int? VisitorId { get; set; }
    //    public int? GateDetailsId { get; set; }
    //}

    //public class VisitorGateNo_Response
    //{
    //    public int Id { get; set; }
    //    public int? VisitorId { get; set; }
    //    public int? GateDetailsId { get; set; }
    //    public string? GateNumber { get; set; }
    //}

    public class VisitorApproveNRejectHistory_Search : BaseSearchEntity
    {
        public int VisitorId { get; set; }
    }

    public class VisitorApproveNRejectHistory_Response
    {
        public int? Id { get; set; }
        public string? Remarks { get; set; }
        public string? ApproveOrReject { get; set; }
        public string? CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class Visitor_ApproveNReject
    {
        public int? Id { get; set; }
        public int? StatusId { get; set; }

        [JsonIgnore]
        public string? BarcodeOriginalFileName { get; set; }

        [JsonIgnore]
        public string? BarcodeFileName { get; set; }
        public string? Remarks { get; set; }
    }

    public class VisitorLogHistory_Search : BaseSearchEntity
    {
        public int? VisitorId { get; set; }
    }

    public class VisitorLogHistory_Response
    {
        public int Id { get; set; }
        public int? VisitorId { get; set; }
        public string? VisitorName { get; set; }
        public string? GateNumber { get; set; }
        public string? CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class VisitorPlanned_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? VisitDate { get; set; }

        //[DefaultValue(null)]
        //public DateTime? ToDate { get; set; }

        [DefaultValue(0)]
        public int? GateDetailsId { get; set; }
        public int? IsPlanned_CheckIn_CheckOut { get; set; }

        [DefaultValue(null)]
        public bool? IsPlanned { get; set; }

        [DefaultValue(null)]
        public bool? IsCrew { get; set; }

        [DefaultValue(null)]
        public bool? IsForeigner { get; set; }
    }

    public class VisitorPlanned_Response : BaseResponseEntity
    {
        public int? VisitorId { get; set; }
        public string? VisitNumber { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VisitorEmailId { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
        public string? AssignGateNumber { get; set; }
        public int? PassTypeId { get; set; }
        public string? PassType { get; set; }
        public int? VisitTypeId { get; set; }
        public string? VisitType { get; set; }
        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
        public string? BarcodeURL { get; set; }
        public string? VisitorPhotoOriginalFileName { get; set; }
        public string? VisitorPhoto { get; set; }
        public string? VisitorPhotoURL { get; set; }

        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? CheckedInOutStatus { get; set; }
        public DateTime? CheckedInDate { get; set; }
        public string? Validity { get; set; }
        public bool? IsCheckedInDisable { get; set; }
        public bool? IsActive { get; set; }
    }

    public class VisitorCheckedInOut_Request : BaseEntity
    {
        public int? RefId { get; set; }
        public string? RefType { get; set; }
        public int? GateDetailsId { get; set; }
        public int? IsCheckedIn_Out { get; set; }

        [DefaultValue(null)]
        public DateTime? CheckedInDate { get; set; }
        public string? CheckedRemark { get; set; }
    }

    public class VisitorCheckedInOut_Offline_Request : BaseEntity
    {
        public int? RefId { get; set; }
        public string? RefType { get; set; }
        public int? GateDetailsId { get; set; }
        public int? IsCheckedIn_Out { get; set; }

        [DefaultValue(null)]
        public DateTime? CheckedInDate { get; set; }

        [DefaultValue(null)]
        public DateTime? CheckedOutDate { get; set; }
        public string? CheckedRemark { get; set; }
    }

    public class CheckedInOutLogHistory_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
        public int? RefId { get; set; }

        [DefaultValue("Employee")]
        public string? RefType { get; set; }
        public int? GateDetailsId { get; set; }

        [DefaultValue(null)]
        public bool? IsReject { get; set; }

        [DefaultValue(null)]
        public bool? IsLiveReport { get; set; }
    }

    public class CheckedInOutLogHistory_Response
    {
        public int Id { get; set; }
        public int? RefId { get; set; }
        public string? RefType { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
        public string? CheckedStatus { get; set; }
        public string? CheckedRemark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public string? CreatorName { get; set; }

    }

    public class VisitorOTPVerify
    {
        public string? VisitNumber { get; set; }
        public string? MobileNo { get; set; }
    }

    public class PreviousVisitor_Search : BaseSearchEntity
    {
        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }

        [DefaultValue(0)]
        public int? GateDetailsId { get; set; }
        public int? IsPlanned_CheckIn_CheckOut { get; set; }
    }

    public class PreviousVisitor_Response : BaseResponseEntity
    {
        public int? VisitorId { get; set; }
        public string? VisitNumber { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VisitorEmailId { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
        public string? AssignGateNumber { get; set; }
        public int? PassTypeId { get; set; }
        public string? PassType { get; set; }
        public int? VisitTypeId { get; set; }
        public string? VisitType { get; set; }
        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
        public string? BarcodeURL { get; set; }
        public string? VisitorPhotoOriginalFileName { get; set; }
        public string? VisitorPhoto { get; set; }
        public string? VisitorPhotoURL { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime? CheckedInDate { get; set; }
        public string? Validity { get; set; }
        public bool? IsActive { get; set; }
    }
    public class MeetingPurposeLogHistory_Search : BaseSearchEntity
    {
        public int? VisitorId { get; set; }
    }

    public class MeetingPurposeLogHistory_Response
    {
        public int Id { get; set; }
        public int? VisitorId { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public bool? IsMeetingOver { get; set; }
        public string? CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class Visitor_ImportData
    {
        public string? PassType { get; set; }
        public DateTime? VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorEmailId { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? Address { get; set; }
        public string? IsCompany { get; set; }
        public string? VisitorCompany { get; set; }
        public string? VisitPurpose { get; set; }
        public string? Branch { get; set; }
        public string? Department { get; set; }
        public string? GateNumber { get; set; }
        public string? EmployeeName { get; set; }
        public string? IsVehicle { get; set; }
        public string? VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        public string? IsDrivingLicense { get; set; }
        public string? IsPUC { get; set; }
        public string? IsInsurance { get; set; }
        public string? Remarks { get; set; }
        public string? IsCrew { get; set; }
        public string? Nationality { get; set; }
        public string? IsPassport { get; set; }
        public string? PassportNo { get; set; }
        public string? PlaceOfIssue { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportValidTill { get; set; }
        public string? Vessel { get; set; }
        public string? IsForeigner { get; set; }
        public string? IsVisa { get; set; }
        public string? VisaNo { get; set; }
        public DateTime? VisaValidFrom { get; set; }
        public DateTime? VisaValidTill { get; set; }
    }

    public class Visitor_ImportDataValidation
    {
        public string? PassType { get; set; }
        public string? VisitStartDate { get; set; }
        public string? VisitEndDate { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorEmailId { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? Address { get; set; }
        public string? IsCompany { get; set; }
        public string? VisitorCompany { get; set; }
        public string? VisitPurpose { get; set; }
        public string? Branch { get; set; }
        public string? Department { get; set; }
        public string? GateNumber { get; set; }
        public string? EmployeeName { get; set; }
        public string? IsVehicle { get; set; }
        public string? VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        public string? IsDrivingLicense { get; set; }
        public string? IsPUC { get; set; }
        public string? IsInsurance { get; set; }
        public string? Remarks { get; set; }
        public string? IsCrew { get; set; }
        public string? Nationality { get; set; }
        public string? IsPassport { get; set; }
        public string? PassportNo { get; set; }
        public string? PlaceOfIssue { get; set; }
        public string? PassportIssueDate { get; set; }
        public string? PassportValidTill { get; set; }
        public string? Vessel { get; set; }
        public string? IsForeigner { get; set; }
        public string? IsVisa { get; set; }
        public string? VisaNo { get; set; }
        public string? VisaValidFrom { get; set; }
        public string? VisaValidTill { get; set; }
        public string ValidationMessage { get; set; }
    }

    public class AutoDailyReport_Search
    {
        [DefaultValue("Visitor")]
        public string? RefType { get; set; }
    }
    public class AutoDailyReport_Response
    {
        public int Id { get; set; }
        public string? VisitNumber { get; set; }
        public string? VisitDate { get; set; }
        public string? VisitorName { get; set; }
        public string? PassType { get; set; }
        public string? VisitorCompany { get; set; }
        public string? VisitorMobileNo { get; set; }
        public DateTime? Validity { get; set; }
        public string? GateNumber { get; set; }
        public string? HostDepartment { get; set; }
        public int? EmployeeId { get; set; }
        public string? HostName { get; set; }
        public string? VehicleNumber { get; set; }
        public string? VisitType { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string? BranchName { get; set; }
        public string? Remarks { get; set; }
    }

    public class AutoDailyReport_Worker_Response
    {
        public int Id { get; set; }
        public string? VisitDate { get; set; }
        public string? GateNumber { get; set; }
        public string? ContractorName { get; set; }
        public string? WorkerName { get; set; }
        public string? WorkerId { get; set; }
        public string? WorkerTypeId { get; set; }
        public string? WorkerType { get; set; }
        public string? WorkerMobileNo { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string? BranchName { get; set; }
    }

    public class AutoDailyReport_Employee_Response
    {
        public int Id { get; set; }
        public string? VisitDate { get; set; }
        public string? GateNumber { get; set; }
        public string? EmployeeName { get; set; }
        public string? ContactNumber { get; set; }
        public string? EmployeeCode { get; set; }
        public string? DepartmentName { get; set; }
        public string? RoleName { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string? BranchName { get; set; }
    }

    public class BarcodeRegenerate_Request
    {
        [DefaultValue("Visitor")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }
    }
}
