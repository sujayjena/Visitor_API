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
    public class ManageWorkerModel
    {
    }

    public class Worker_Request : BaseEntity
    {
        public Worker_Request()
        {
            GateNumberList = new List<AssignGateNo_Request>();
            DocumentVerificationList = new List<VisitorDocumentVerification_Request>();
        }
        public string? WorkerName { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? WorkerTypeId { get; set; }
        public int? ContractTypeId { get; set; }
        public string? WorkerMobileNo { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string? WorkerId { get; set; }
        public DateTime? DOB { get; set; }
        public int? BloodGroupId { get; set; }
        public string? IdentificationMark { get; set; }
        public int? BranchId { get; set; }
        public int? WorkPlaceId { get; set; }
        public string? Address { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public string? Pincode { get; set; }
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentFileName { get; set; }
        public string? Document_Base64 { get; set; }

        public string? WorkerPhotoOriginalFileName { get; set; }
        public string? WorkerPhotoFileName { get; set; }
        public string? WorkerPhoto_Base64 { get; set; }

        public int? DepartmentId { get; set; }
        public int? EmployeeId { get; set; }

        [DefaultValue(false)]
        public bool? DV_IsInsurance { get; set; }
        public string? DV_InsuranceNumber { get; set; }
        public string? DV_InsuranceOriginalFileName { get; set; }
        public string? DV_InsuranceFileName { get; set; }
        public string? DV_Insurance_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? DV_IsWC { get; set; }
        public string? DV_WCNumber { get; set; }
        public string? DV_WCOriginalFileName { get; set; }
        public string? DV_WCFileName { get; set; }
        public string? DV_WC_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? DV_IsESIC { get; set; }
        public string? DV_ESICNumber { get; set; }
        public string? DV_ESICOriginalFileName { get; set; }
        public string? DV_ESICFileName { get; set; }
        public string? DV_ESIC_Base64 { get; set; }

        public int? WorkerShift { get; set; }

        [DefaultValue(false)]
        public bool? IsPoliceV { get; set; }
        public string? PoliceVOriginalFileName { get; set; }
        public string? PoliceVFileName { get; set; }
        public string? PoliceV_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? IsFitnessCert { get; set; }
        public string? FitnessCertOriginalFileName { get; set; }
        public string? FitnessCertFileName { get; set; }
        public string? FitnessCert_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? IsDriver { get; set; }
        public string? VehicleNumber { get; set; }
        public string? DrivingLicenseNo { get; set; }

        [DefaultValue(null)]
        public DateTime? LicenseValidFrom { get; set; }

        [DefaultValue(null)]
        public DateTime? LicenseValidTo { get; set; }

        [DefaultValue(false)]
        public bool? IsCanteenPrivilege { get; set; }

        [DefaultValue(false)]
        public bool? IsPrimary { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }

        public bool? IsActive { get; set; }
        public List<AssignGateNo_Request> GateNumberList { get; set; }
        public List<VisitorDocumentVerification_Request> DocumentVerificationList { get; set; }
    }

    public class WorkerSearch_Request : BaseSearchEntity
    {
        [DefaultValue(null)]
        public bool? IsBlackList { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? BranchId { get; set; }
        public int? ContractorId { get; set; }
        public int? EmployeeId { get; set; }

        [DefaultValue(0)]
        public int? StatusId { get; set; }

        [DefaultValue(0)]
        public int? GateDetailsId { get; set; }
    }

    public class Worker_Response : BaseResponseEntity
    {
        public Worker_Response()
        {
            GateNumberList = new List<AssignGateNo_Response>();
        }
        public string? WorkerName { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string? PONumber { get; set; }
        public DateTime? POStartDate { get; set; }
        public DateTime? POEndDate { get; set; }
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public int? ContractorTypeId { get; set; }
        public string? ContractorType { get; set; }
        public DateTime? ContractorStartDate { get; set; }
        public DateTime? ContractorEndDate { get; set; }
        public bool? Contractor_IsCanteenPrivilege { get; set; }
        public int? WorkerTypeId { get; set; }
        public string? WorkerType { get; set; }
        public int? ContractTypeId { get; set; }
        public string? ContractType { get; set; }
        public string? WorkerMobileNo { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string? WorkerId { get; set; }
        public DateTime? DOB { get; set; }
        public int? BloodGroupId { get; set; }
        public string? BloodGroup { get; set; }
        public string? IdentificationMark { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? WorkPlaceId { get; set; }
        public string? WorkPlace { get; set; }
        public string? Address { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? Pincode { get; set; }
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentFileName { get; set; }
        public string? DocumentURL { get; set; }
        public string? WorkerPhotoOriginalFileName { get; set; }
        public string? WorkerPhotoFileName { get; set; }
        public string? WorkerPhotoURL { get; set; }
        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
        public string? BarcodeURL { get; set; }
        public DateTime? PurchaseOrderValidity { get; set; }
        public string? PassNumber { get; set; }
        public bool? IsExpire { get; set; }

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeEmailId { get; set; }
        public string? EmployeeMobileNo { get; set; }

        public bool? DV_IsInsurance { get; set; }
        public string? DV_InsuranceNumber { get; set; }
        public string? DV_InsuranceOriginalFileName { get; set; }
        public string? DV_InsuranceFileName { get; set; }
        public string? DV_InsuranceURL { get; set; }

        public bool? DV_IsWC { get; set; }
        public string? DV_WCNumber { get; set; }
        public string? DV_WCOriginalFileName { get; set; }
        public string? DV_WCFileName { get; set; }
        public string? DV_WCURL { get; set; }

        public bool? DV_IsESIC { get; set; }
        public string? DV_ESICNumber { get; set; }
        public string? DV_ESICOriginalFileName { get; set; }
        public string? DV_ESICFileName { get; set; }
        public string? DV_ESICURL { get; set; }

        public int? WorkerShift { get; set; }
        public bool? IsPoliceV { get; set; }
        public string? PoliceVOriginalFileName { get; set; }
        public string? PoliceVFileName { get; set; }
        public string? PoliceVURL { get; set; }

        public bool? IsFitnessCert { get; set; }
        public string? FitnessCertOriginalFileName { get; set; }
        public string? FitnessCertFileName { get; set; }
        public string? FitnessCertURL { get; set; }
        public bool? IsDriver { get; set; }
        public string? VehicleNumber { get; set; }
        public string? DrivingLicenseNo { get; set; }
        public DateTime? LicenseValidFrom { get; set; }
        public DateTime? LicenseValidTo { get; set; }
        public string? ValidityPeriod { get; set; }
        public decimal? OverTime { get; set; }
        public decimal? TotalHours { get; set; }
        public string? CheckedInOutStatus { get; set; }
        public bool? IsCheckedInDisable { get; set; }
        public string? Remarks { get; set; }
        public bool? IsCanteenPrivilege { get; set; }
        public bool? IsPrimary { get; set; }
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
        public List<AssignGateNo_Response> GateNumberList { get; set; }
        public List<VisitorDocumentVerification_Response> DocumentVerificationList { get; set; }
    }

    public class WorkerPass_Request : BaseEntity
    {
        public int? WorkerId { get; set; }

        [JsonIgnore]
        public string? PassNumber { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public bool? IsActive { get; set; }
    }
    public class WorkerPassSearch_Request : BaseSearchEntity
    {
        public int? WorkerId { get; set; }
    }
    public class WorkerPass_Response
    {
        public int? WorkerId { get; set; }
        public string? PassNumber { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatorName { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class Worker_ImportData
    {
        public string? WorkerShift { get; set; }
        public string? WorkerName { get; set; }
        public string? WorkerType { get; set; }
        public string? WorkerMobileNo { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string? PONumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? BloodGroup { get; set; }
        public string? IdentificationMark { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? IsInsurance { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? IsWC { get; set; }
        public string? WCNumber { get; set; }
        public string? IsESIC { get; set; }
        public string? ESICNumber { get; set; }
        public string? IsPoliceVerification { get; set; }
        public string? IsFitnessCertificate { get; set; }
        public string? Branch { get; set; }
        public string? WorkPlace { get; set; }
        public string? Department { get; set; }
        public string? EmployeeName { get; set; }
        public string? GateNumber { get; set; }
        public string? IsDriver { get; set; }
        public string? VehicleNumber { get; set; }
        public string? DrivingLicenseNo { get; set; }
        public DateTime? LicenseValidFrom { get; set; }
        public DateTime? LicenseValidTo { get; set; }
        public string? AadharNumber { get; set; }
        public string? IsBlackList { get; set; }
        public string? IsActive { get; set; }
    }

    public class Worker_ImportDataValidation
    {
        public string? WorkerShift { get; set; }
        public string? WorkerName { get; set; }
        public string? WorkerType { get; set; }
        public string? WorkerMobileNo { get; set; }
        public string? ValidFromDate { get; set; }
        public string? ValidToDate { get; set; }
        public string? PONumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? BloodGroup { get; set; }
        public string? IdentificationMark { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? Pincode { get; set; }
        public string? IsInsurance { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? IsWC { get; set; }
        public string? WCNumber { get; set; }
        public string? IsESIC { get; set; }
        public string? ESICNumber { get; set; }
        public string? IsPoliceVerification { get; set; }
        public string? IsFitnessCertificate { get; set; }
        public string? Branch { get; set; }
        public string? WorkPlace { get; set; }
        public string? Department { get; set; }
        public string? EmployeeName { get; set; }
        public string? GateNumber { get; set; }
        public string? IsDriver { get; set; }
        public string? VehicleNumber { get; set; }
        public string? DrivingLicenseNo { get; set; }
        public string? LicenseValidFrom { get; set; }
        public string? LicenseValidTo { get; set; }
        public string? AadharNumber { get; set; }
        public string? IsBlackList { get; set; }
        public string? IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
    public class Worker_ApproveNReject
    {
        public int? Id { get; set; }
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
    }
}
