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
        public string? MobileNo { get; set; }
        public int? VisitorId { get; set; }
        public int? GateDetailsId { get; set; }
        public int? PassTypeId { get; set; }

        [DefaultValue(null)]
        public DateTime? FromDate { get; set; }

        [DefaultValue(null)]
        public DateTime? ToDate { get; set; }
        public int? StatusId { get; set; }

        [DefaultValue("")]
        public string? GateDetailsId_Filter { get; set; }
    }

    public class Visitors_Request : BaseEntity
    {
        public Visitors_Request()
        {
            GateNumberList = new List<VisitorGateNo_Request>();
        }

        public DateTime? VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public int? IsVisitor_Contractor_Vendor { get; set; }
        public int? VisitTypeId { get; set; }
       
        public string? VisitorName { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VisitorEmailId { get; set; }
        public int? GenderId { get; set; }
        
       
        public int? VisitorCompanyId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public int? Pincode { get; set; }
        public string? AddressLine { get; set; }
        public int? IDTypeId { get; set; }

        public string? VisitorPhotoOriginalFileName { get; set; }
        [JsonIgnore]
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
        public bool? IsActive { get; set; }
        public List<VisitorGateNo_Request> GateNumberList { get; set; }
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
        public int? Pincode { get; set; }
        public string? AddressLine { get; set; }
        public int? IDTypeId { get; set; }
        public string? IDType { get; set; }
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
        public bool? IsActive { get; set; }

        public List<VisitorGateNo_Response> GateNumberList { get; set; }
    }

    public class VisitorGateNo_Request
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string? Action { get; set; }

        [JsonIgnore]
        public int? VisitorId { get; set; }
        public int? GateDetailsId { get; set; }
    }

    public class VisitorGateNo_Response
    {
        public int Id { get; set; }
        public int? VisitorId { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
    }
}
