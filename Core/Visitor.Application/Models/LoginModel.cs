using Visitor.Application.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Visitor.Application.Models
{
    public class LoginByMobileNumberRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [JsonIgnore]
        public string? MobileUniqueId { get; set; }

        [DefaultValue("W")]
        public string IsWebOrMobileUser { get; set; }

        public bool Remember { get; set; }
    }

    public class MobileAppLoginRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile Unique Id is required")]
        [MaxLength(ValidationConstants.MobileUniqueId_MaxLength, ErrorMessage = ValidationConstants.MobileUniqueId_MaxLength_Msg)]
        public string MobileUniqueId { get; set; }

        [DefaultValue("M")]
        public string IsWebOrMobileUser { get; set; }
        public bool Remember { get; set; }
    }

    public class LoginOTPRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNumber { get; set; }
    }

    public class LoginByOTPRequestModel : LoginOTPRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.OTP_Required_Msg)]
        [RegularExpression(ValidationConstants.OTP_RegExp, ErrorMessage = ValidationConstants.OTP_RegExp_Msg)]
        [MinLength(ValidationConstants.OTP_MinLength, ErrorMessage = ValidationConstants.OTP_Range_Msg)]
        [MaxLength(ValidationConstants.OTP_MaxLength, ErrorMessage = ValidationConstants.OTP_Range_Msg)]
        public string OTP { get; set; }
    }


    // Login Response
    public class UsersLoginSessionData
    {
        public long? UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string UserType { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsWebUser { get; set; }
        public bool IsMobileUser { get; set; }
        public bool IsActive { get; set; }
    }

    public class SessionDataCustomer
    {
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string UserType { get; set; }
        public string Token { get; set; }
    }

    public class SessionDataEmployee
    {
        public long? UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string UserType { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BranchId { get; set; }
        public bool IsWebUser { get; set; }
        public bool IsMobileUser { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public string ProfileImage { get; set; }
        public string? ProfileOriginalFileName { get; set; }
        public string ProfileImageURL { get; set; }
        public List<RoleMaster_Employee_Permission_Response> UserRoleList { get; set; }
    }
}
