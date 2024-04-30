using Visitor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [DefaultValue(1)]
        public int CompanyId { get; set; }

        [DefaultValue(1)]
        public int? DepartmentId { get; set; }

        //public string UserCode { get; set; }

        public string UserName { get; set; }

        public string MobileNumber { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }

        public string UserType { get; set; }

        public int? RoleId { get; set; }

        public int? ReportingTo { get; set; }

        public string AddressLine { get; set; }

        public int? RegionId { get; set; }

        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? CityId { get; set; }

        public int? AreaId { get; set; }

        public int? Pincode { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public string EmergencyContactNumber { get; set; }

        public string BloodGroup { get; set; }

        public string MobileUniqId { get; set; }

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

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<BranchMapping_Request>? BranchList { get; set; }
    }

    public class User_Response : BaseResponseEntity
    {
        public User_Response()
        {
            BranchList = new List<BranchMapping_Response>();
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

        public string EmergencyContactNumber { get; set; }

        public string BloodGroup { get; set; }

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

        public bool? IsMobileUser { get; set; }

        public bool? IsWebUser { get; set; }

        public bool? IsActive { get; set; }

        public List<BranchMapping_Response>? BranchList { get; set; }
    }
}
