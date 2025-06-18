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
        }
        public string? WorkerName { get; set; }
        public int? ContractorId { get; set; }
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

        [JsonIgnore]
        public string? DocumentFileName { get; set; }
        public string? Document_Base64 { get; set; }

        public string? WorkerPhotoOriginalFileName { get; set; }

        [JsonIgnore]
        public string? WorkerPhotoFileName { get; set; }
        public string? WorkerPhoto_Base64 { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
        public List<AssignGateNo_Request> GateNumberList { get; set; }
    }

    public class WorkerSearch_Request : BaseSearchEntity
    {
        [DefaultValue(null)]
        public bool? IsBlackList { get; set; }
        public int? ContractorId { get; set; }
        public int? BranchId { get; set; }
    }

    public class Worker_Response : BaseResponseEntity
    {
        public Worker_Response()
        {
            GateNumberList = new List<AssignGateNo_Response>();
        }
        public string? WorkerName { get; set; }
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public int? ContractorTypeId { get; set; }
        public string? ContractorType { get; set; }
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
        public DateTime? ContractorValidity { get; set; }
        public string? PassNumber { get; set; }
        public bool? IsExpire { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
        public List<AssignGateNo_Response> GateNumberList { get; set; }
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
}
