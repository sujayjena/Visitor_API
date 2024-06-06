using Visitor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Visitor.Application.Models
{
    public class UserModel
    {
    }

    public class User_Request : BaseEntity
    {
        public User_Request()
        {
            BranchList = new List<BranchMapping_Request>();
        }

        //public string UserCode { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }

        public string UserType { get; set; }

        public int? RoleId { get; set; }

        public int? ReportingTo { get; set; }

        [DefaultValue(1)]
        public int CompanyId { get; set; }

        [DefaultValue(1)]
        public int? DepartmentId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public string MobileUniqId { get; set; }

        public int BloodGroupId { get; set; }

        public int GenderId { get; set; }

        public int MaritalStatusId { get; set; }

        public string AddressLine { get; set; }

        public int? RegionId { get; set; }

        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? CityId { get; set; }

        public int? AreaId { get; set; }

        public int? Pincode { get; set; }

        public bool IsSameAsPermanent { get; set; }
        public string TemporaryAddress { get; set; }

        public int? Temporary_RegionId { get; set; }

        public int? Temporary_StateId { get; set; }

        public int? Temporary_DistrictId { get; set; }

        public int? Temporary_CityId { get; set; }

        public int? Temporary_Pincode { get; set; }

        public string EmergencyName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EmergencyRelation { get; set; }

        public string AadharNumber { get; set; }

        public string AadharImage { get; set; }

        public string AadharImage_Base64 { get; set; }

        public string? AadharOriginalFileName { get; set; }

        public string PanNumber { get; set; }

        public string PanCardImage { get; set; }

        public string PanCardImage_Base64 { get; set; }

        public string? PanCardOriginalFileName { get; set; }

        public string ProfileImage { get; set; }

        public string ProfileImage_Base64 { get; set; }

        public string? ProfileOriginalFileName { get; set; }

        public string OtherProof { get; set; }

        public string OtherProofImage { get; set; }

        public string OtherProofImage_Base64 { get; set; }

        public string? OtherProofOriginalFileName { get; set; }

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<BranchMapping_Request>? BranchList { get; set; }
        public List<UserOtherDetails_Request>? UserOtherDetailsList { get; set; }
    }

    public class User_Response : BaseResponseEntity
    {
        public User_Response()
        {
            BranchList = new List<BranchMapping_Response>();
            UserOtherDetailsList = new List<UserOtherDetails_Response>();
        }

        //public string UserCode { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }

        public string UserType { get; set; }

        public int? RoleId { get; set; }

        public string RoleName { get; set; }

        public int? ReportingTo { get; set; }

        public string ReportingToName { get; set; }

        public string ReportingToMobileNo { get; set; }

        [DefaultValue(1)]
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        [DefaultValue(1)]
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }

        public string AddressLine { get; set; }

        public int? RegionId { get; set; }

        public string RegionName { get; set; }

        public int? StateId { get; set; }

        public string StateName { get; set; }

        public int? DistrictId { get; set; }

        public string DistrictName { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public int? AreaId { get; set; }

        public string AreaName { get; set; }
        public int? Pincode { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public int BloodGroupId { get; set; }

        public string BloodGroup { get; set; }

        public int GenderId { get; set; }

        public string GenderName { get; set; }

        public int MaritalStatusId { get; set; }

        public string MaritalStatus { get; set; }

        public bool IsSameAsPermanent { get; set; }

        public string TemporaryAddress { get; set; }
        public int Temporary_RegionId { get; set; }
        public string Temporary_RegionName { get; set; }
        public int Temporary_StateId { get; set; }
        public string Temporary_StateName { get; set; }
        public int Temporary_DistrictId { get; set; }
        public string Temporary_DistrictName { get; set; }
        public int Temporary_CityId { get; set; }
        public string Temporary_CityName { get; set; }
        public string Temporary_Pincode { get; set; }
        public string EmergencyName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EmergencyRelation { get; set; }

        public string MobileUniqueId { get; set; }

        public string AadharNumber { get; set; }

        public string AadharImage { get; set; }

        public string? AadharOriginalFileName { get; set; }

        public string AadharImageURL { get; set; }

        public string PanNumber { get; set; }

        public string PanCardImage { get; set; }

        public string? PanCardOriginalFileName { get; set; }

        public string PanCardImageURL { get; set; }

        public string ProfileImage { get; set; }

        public string? ProfileOriginalFileName { get; set; }

        public string ProfileImageURL { get; set; }

        public string OtherProof { get; set; }

        public string OtherProofImage { get; set; }

        public string? OtherProofOriginalFileName { get; set; }

        public string OtherProofImageURL { get; set; }

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<BranchMapping_Response>? BranchList { get; set; }
        public List<UserOtherDetails_Response>? UserOtherDetailsList { get; set; }
    }

    public class UserOtherDetails_Request : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public string PastCompanyName { get; set; }
        public string TotalExp { get; set; }
        public string Remark { get; set; }
    }

    public class UserOtherDetails_Response : BaseEntity
    {
        public int? EmployeeId { get; set; }
        public string PastCompanyName { get; set; }
        public string TotalExp { get; set; }
        public string Remark { get; set; }
    }
}
