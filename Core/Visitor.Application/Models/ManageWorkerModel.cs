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
        public DateTime? ContractorValidity { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
        public List<AssignGateNo_Response> GateNumberList { get; set; }
    }
}
