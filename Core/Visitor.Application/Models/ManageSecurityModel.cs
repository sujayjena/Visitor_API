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
    public class SecurityLogin_Search : BaseSearchEntity
    {
        public bool? IsBlackListed { get; set; }
    }

    public class SecurityLogin_Request : BaseEntity
    {
        public SecurityLogin_Request()
        {
            GateDetailsList = new List<SecurityLoginGateDetails_Request>();
        }
        public string? SecurityName { get; set; }
        public string? SecurityMobileNo { get; set; }
        public int? ContractorTypeId { get; set; }
        public int? ContractorId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentOriginalFileName { get; set; }

        [JsonIgnore]
        public string? DocumentFileName { get; set; }
        public string? Document_Base64 { get; set; }
        public string? PhotoOriginalFileName { get; set; }

        [JsonIgnore]
        public string? PhotoFileName { get; set; }
        public string? Photo_Base64 { get; set; }
        public string? Passwords { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackListed { get; set; }
        public bool? IsActive { get; set; }
        public List<SecurityLoginGateDetails_Request>? GateDetailsList { get; set; }
    }

    public class SecurityLogin_Response : BaseResponseEntity
    {
        public SecurityLogin_Response()
        {
            GateDetailsList = new List<SecurityLoginGateDetails_Response>();
        }
        public string? SecurityName { get; set; }
        public string? SecurityMobileNo { get; set; }
        public int? ContractorTypeId { get; set; }
        public string? ContractorType { get; set; }
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentFileName { get; set; }
        public string? DocumentURL { get; set; }
        public string? PhotoOriginalFileName { get; set; }
        public string? PhotoFileName { get; set; }
        public string? PhotoURL { get; set; }
        public bool? IsBlackListed { get; set; }
        public bool? IsActive { get; set; }
        public List<SecurityLoginGateDetails_Response>? GateDetailsList { get; set; }
    }

    public class SecurityLoginGateDetails_Request : BaseEntity
    {
        [JsonIgnore]
        public string? Action { get; set; }

        [JsonIgnore]
        public int? SecurityLoginId { get; set; }
        public int GateDetailsId { get; set; }
    }

    public class SecurityLoginGateDetails_Response : BaseEntity
    {
        public int? SecurityLoginId { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
    }
}
