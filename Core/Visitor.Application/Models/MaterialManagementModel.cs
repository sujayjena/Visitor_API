using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    #region Material Request
    public class MaterialRequest_Request : BaseEntity
    {
        public MaterialRequest_Request()
        {
            MaterialRequestDetails = new List<MaterialRequestDetails_Request>();
            GateNumberList = new List<AssignGateNo_Request>();
            DocumentVerificationList = new List<VisitorDocumentVerification_Request>();
        }

        public int? TransactionTypeId { get; set; }
        public int? ContractorTypeId { get; set; }
        public int? ContractorId { get; set; }
        public string? PurchaseOrderNo { get; set; }
        public string? WorkerName { get; set; }
        public int? WorkerTypeId { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public DateTime? POStartDate { get; set; }
        public DateTime? POEndDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? BloodGroupId { get; set; }
        public string? IdentificationMark { get; set; }

        [DefaultValue("")]
        public string? WorkerPhotoOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? WorkerPhotoFileName { get; set; }

        [DefaultValue("")]
        public string? WorkerPhoto_Base64 { get; set; }
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public string? Pincode { get; set; }
        public int? BranchId { get; set; }
        public int? WorkPlaceId { get; set; }
        public int? DepartmentId { get; set; }
        public int? EmployeeId { get; set; }
        public bool? IsDriver { get; set; }
        public int? VehicleNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime? LicenseValidFrom { get; set; }
        public DateTime? LicenseValidTo { get; set; }

        [DefaultValue(1)]
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
        public List<MaterialRequestDetails_Request> MaterialRequestDetails { get; set; }
        public List<AssignGateNo_Request> GateNumberList { get; set; }
        public List<VisitorDocumentVerification_Request> DocumentVerificationList { get; set; }
    }
    public class MaterialRequest_Search : BaseSearchEntity
    {
        public int? StatusId { get; set; }
        public int? MaterialRequestId { get; set; }
        public int? TransactionTypeId { get; set; }
    }
    public class MaterialRequestList_Response : BaseResponseEntity
    {
        public string? RequisitionCode { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Remarks { get; set; }
        public int? Approver1_Id { get; set; }
        public string? Approver1_Name { get; set; }
        public int? Approver2_Id { get; set; }
        public string? Approver2_Name { get; set; }
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
    }
    public class MaterialRequest_Response : BaseResponseEntity
    {
        public MaterialRequest_Response()
        {
            MaterialRequestDetails = new List<MaterialRequestDetails_Response>();
            GateNumberList = new List<AssignGateNo_Response>();
            DocumentVerificationList = new List<VisitorDocumentVerification_Response>();
        }
        public int? VisitorId { get; set; }
        public string? VisitNumber { get; set; }
        public string? RequisitionCode { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? ContractorTypeId { get; set; }
        public string? ContractorType { get; set; }
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public string? WorkerName { get; set; }
        public int? WorkerTypeId { get; set; }
        public string? WorkerType { get; set; }
        public string? MobileNumber { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public DateTime? POStartDate { get; set; }
        public DateTime? POEndDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? BloodGroupId { get; set; }
        public string? BloodGroup { get; set; }
        public string? IdentificationMark { get; set; }
        public string? WorkerPhotoOriginalFileName { get; set; }
        public string? WorkerPhotoFileName { get; set; }
        public string? WorkerPhotoURL { get; set; }
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? Pincode { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? WorkPlaceId { get; set; }
        public string? WorkPlace { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? IsDriver { get; set; }
        public string? VehicleNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime? LicenseValidFrom { get; set; }
        public DateTime? LicenseValidTo { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Remarks { get; set; }
        public int? Approver1_Id { get; set; }
        public string? Approver1_Name { get; set; }
        public int? Approver2_Id { get; set; }
        public string? Approver2_Name { get; set; }
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
        public List<MaterialRequestDetails_Response> MaterialRequestDetails { get; set; }
        public List<AssignGateNo_Response> GateNumberList { get; set; }
        public List<VisitorDocumentVerification_Response> DocumentVerificationList { get; set; }
    }
    public class MaterialRequest_ApproveNReject
    {
        public int? Id { get; set; }
        public int? StatusId { get; set; }
        public string? Remarks { get; set; }
    }
    #endregion

    #region Material Request details
    public class MaterialRequestDetails_Request : BaseEntity
    {
        public int? MaterialRequestId { get; set; }
        public int? MaterialDetailsId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public string? SerialNo { get; set; }
    }
    public class MaterialRequestDetails_Search : BaseSearchEntity
    {
        public int? MaterialRequestId { get; set; }
    }
    public class MaterialRequestDetails_Response : BaseEntity
    {
        public int? MaterialRequestId { get; set; }
        public string? RequisitionCode { get; set; }
        public int? MaterialDetailsId { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public string? SerialNo { get; set; }
    }

    #endregion
}
