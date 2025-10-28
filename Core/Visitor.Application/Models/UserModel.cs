using Visitor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Visitor.Persistence.Repositories;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Reflection;

namespace Visitor.Application.Models
{
    public class UserModel
    {
    }
    public class User_Search : BaseSearchEntity
    {
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int UserTypeId { get; set; }
        public int IsFilterType { get; set; }
        public int RoleId { get; set; }
        public int EmployeeId { get; set; }
    }

    public class User_Request : BaseEntity
    {
        public User_Request()
        {
            BranchList = new List<BranchMapping_Request>();
            UserOtherDetailsList = new List<UserOtherDetails_Request>();
            GateNumberList = new List<AssignGateNo_Request>();
        }

        public string? UserCode { get; set; }

        public string? UserName { get; set; }

        public string? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? Password { get; set; }

        public int? UserTypeId { get; set; }

        public int? RoleId { get; set; }

        public int? ReportingTo { get; set; }

        [DefaultValue(1)]
        public int CompanyId { get; set; }

        [DefaultValue(1)]
        public int DepartmentId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public string? MobileUniqId { get; set; }

        public int? BloodGroupId { get; set; }

        public int? GenderId { get; set; }

        public int? MaritalStatusId { get; set; }

        public string? AddressLine { get; set; }

        public int? CountryId { get; set; }

        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? CityId { get; set; }

        public int? AreaId { get; set; }

        public string? Pincode { get; set; }

        public bool? IsSameAsPermanent { get; set; }
        public string? TemporaryAddress { get; set; }

        public int? Temporary_CountryId { get; set; }

        public int? Temporary_StateId { get; set; }

        public int? Temporary_DistrictId { get; set; }

        public int? Temporary_CityId { get; set; }

        public string? Temporary_Pincode { get; set; }

        public string? EmergencyName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? EmergencyRelation { get; set; }

        public string? AadharNumber { get; set; }

        public string? AadharImage { get; set; }

        public string? AadharImage_Base64 { get; set; }

        public string? AadharOriginalFileName { get; set; }

        public string? PanNumber { get; set; }

        public string? PanCardImage { get; set; }

        public string? PanCardImage_Base64 { get; set; }

        public string? PanCardOriginalFileName { get; set; }

        public string? ProfileImage { get; set; }

        public string? ProfileImage_Base64 { get; set; }

        public string? ProfileOriginalFileName { get; set; }

        public string? OtherProof { get; set; }

        public string? OtherProofImage { get; set; }

        public string? OtherProofImage_Base64 { get; set; }

        public string? OtherProofOriginalFileName { get; set; }

        public int? BranchId { get; set; }

        [DefaultValue(false)]
        public bool? IsHOD { get; set; }

        [DefaultValue(false)]
        public bool? IsManager { get; set; }

        [DefaultValue(false)]
        public bool? IsApprover { get; set; }

        [DefaultValue(null)]
        public DateTime? LastWorkingDate { get; set; }

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<BranchMapping_Request>? BranchList { get; set; }
        public List<UserOtherDetails_Request>? UserOtherDetailsList { get; set; }
        public List<AssignGateNo_Request> GateNumberList { get; set; }
    }

    public class User_Response : BaseResponseEntity
    {
        public User_Response()
        {
            BranchList = new List<BranchMapping_Response>();
            UserOtherDetailsList = new List<UserOtherDetails_Response>();
            GateNumberList = new List<AssignGateNo_Response>();
        }

        public string? UserCode { get; set; }

        public string? UserName { get; set; }

        public string? MobileNumber { get; set; }

        public string? EmailId { get; set; }

        public string? Password { get; set; }

        public int? UserTypeId { get; set; }
        public string? UserType { get; set; }

        public int? RoleId { get; set; }

        public string? RoleName { get; set; }

        public int? ReportingTo { get; set; }

        public string? ReportingToName { get; set; }

        public string? ReportingToMobileNo { get; set; }

        [DefaultValue(1)]
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        [DefaultValue(1)]
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public string? AddressLine { get; set; }

        public int? CountryId { get; set; }

        public string? CountryName { get; set; }

        public int? StateId { get; set; }

        public string? StateName { get; set; }

        public int? DistrictId { get; set; }

        public string? DistrictName { get; set; }

        public int? CityId { get; set; }

        public string? CityName { get; set; }

        public int? AreaId { get; set; }

        public string? AreaName { get; set; }
        public string? Pincode { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public DateTime? Validity { get; set; }

        public int? BloodGroupId { get; set; }

        public string? BloodGroup { get; set; }

        public int? GenderId { get; set; }

        public string? GenderName { get; set; }

        public int? MaritalStatusId { get; set; }

        public string? MaritalStatus { get; set; }

        public bool? IsSameAsPermanent { get; set; }

        public string? TemporaryAddress { get; set; }
        public int? Temporary_CountryId { get; set; }
        public string? Temporary_CountryName { get; set; }
        public int? Temporary_StateId { get; set; }
        public string? Temporary_StateName { get; set; }
        public int? Temporary_DistrictId { get; set; }
        public string? Temporary_DistrictName { get; set; }
        public int? Temporary_CityId { get; set; }
        public string? Temporary_CityName { get; set; }
        public string? Temporary_Pincode { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? EmergencyRelation { get; set; }

        public string? MobileUniqueId { get; set; }

        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
        public string? BarcodeURL { get; set; }

        public string? AadharNumber { get; set; }

        public string? AadharImage { get; set; }

        public string? AadharOriginalFileName { get; set; }

        public string? AadharImageURL { get; set; }

        public string? PanNumber { get; set; }

        public string? PanCardImage { get; set; }

        public string? PanCardOriginalFileName { get; set; }

        public string? PanCardImageURL { get; set; }

        public string? ProfileImage { get; set; }

        public string? ProfileOriginalFileName { get; set; }

        public string? ProfileImageURL { get; set; }

        public string? OtherProof { get; set; }

        public string? OtherProofImage { get; set; }

        public string? OtherProofOriginalFileName { get; set; }

        public string? OtherProofImageURL { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        public bool? IsHOD { get; set; }

        public bool? IsManager { get; set; }

        public bool? IsApprover { get; set; }

        public DateTime? LastWorkingDate { get; set; }

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<BranchMapping_Response>? BranchList { get; set; }
        public List<UserOtherDetails_Response>? UserOtherDetailsList { get; set; }
        public List<AssignGateNo_Response> GateNumberList { get; set; }
    }

    public class UserOffline_Search : BaseSearchEntity
    {
    }

    public class UserOffline_Response
    {
        public int? Id { get; set; }
        public string? UserCode { get; set; }
        public string? UserName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
    }

    public class UserOtherDetails_Request : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public string? PastCompanyName { get; set; }
        public string? TotalExp { get; set; }
        public string? Remark { get; set; }
    }

    public class UserOtherDetails_Response : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public string? PastCompanyName { get; set; }
        public string? TotalExp { get; set; }
        public string? Remark { get; set; }
    }

    public class ChangePassword_Request
    {
        [DefaultValue("")]
        public string? NewPassword { get; set; }

        [DefaultValue("")]
        public string? ConfirmPassword { get; set; }
    }

    public class ForgotPassword_Request
    {
        [DefaultValue("")]
        public string? EmailId { get; set; }

        [JsonIgnore]
        public string? Passwords { get; set; }
    }

    //public class EmployeeGateNo_Request
    //{
    //    public int Id { get; set; }

    //    [JsonIgnore]
    //    public string? Action { get; set; }

    //    [JsonIgnore]
    //    public int? EmployeeId { get; set; }
    //    public int? GateDetailsId { get; set; }
    //}

    //public class EmployeeGateNo_Response
    //{
    //    public int Id { get; set; }
    //    public int? EmployeeId { get; set; }
    //    public int? GateDetailsId { get; set; }
    //    public string? GateNumber { get; set; }
    //}

    public class ImportRequest
    {
        public IFormFile FileUpload { get; set; }
    }

    #region Import and Download

    public class User_ImportRequest
    {
        public IFormFile FileUpload { get; set; }
    }

    public class User_ImportData
    {
        public string? UserCode { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }
        public string? GateNumber { get; set; }
        public string? Role { get; set; }
        public string? ReportingTo { get; set; }
        public string? Department { get; set; }
        public string? Company { get; set; }
        public string? Branch { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? BloodGroup { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? EmergencyRelation { get; set; }
        public string? PastCompanyName { get; set; }
        public string? TotalExp { get; set; }
        public string? Remark { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? IsSameAsPermanent { get; set; }
        public string? Temporary_Address { get; set; }
        public string? Temporary_Country { get; set; }
        public string? Temporary_State { get; set; }
        public string? Temporary_Province { get; set; }
        public string? Temporary_Pincode { get; set; }
        public string? AadharNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? OtherProof { get; set; }
        public string? IsHOD { get; set; }
        public string? IsManager { get; set; }
        public string? IsApprover { get; set; }
        public string? IsMobileUser { get; set; }
        public string? IsWebUser { get; set; }
        public string? IsActive { get; set; }
    }

    public class User_ImportDataValidation
    {
        public string? UserCode { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        //public string? Password { get; set; }
        public string? GateNumber { get; set; }
        public string? Role { get; set; }
        public string? ReportingTo { get; set; }
        public string? Department { get; set; }
        public string? Company { get; set; }
        public string? Branch { get; set; }
        public string? DateOfBirth { get; set; }
        public string? DateOfJoining { get; set; }
        public string? BloodGroup { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? EmergencyRelation { get; set; }
        public string? PastCompanyName { get; set; }
        public string? TotalExp { get; set; }
        public string? Remark { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? IsSameAsPermanent { get; set; }
        public string? Temporary_Address { get; set; }
        public string? Temporary_Country { get; set; }
        public string? Temporary_State { get; set; }
        public string? Temporary_Province { get; set; }
        public string? Temporary_Pincode { get; set; }
        public string? AadharNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? OtherProof { get; set; }
        public string? IsHOD { get; set; }
        public string? IsManager { get; set; }
        public string? IsApprover { get; set; }
        public string? IsMobileUser { get; set; }
        public string? IsWebUser { get; set; }
        public string? IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }

    #endregion
}
