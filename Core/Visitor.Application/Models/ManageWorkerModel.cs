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
        public string? WorkerName { get; set; }
        public int? ContractorId { get; set; }
        public int? WorkerTypeId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentNumber { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
    }

    public class WorkerSearch_Request : BaseSearchEntity
    {
        [DefaultValue(null)]
        public bool? IsBlackList { get; set; }
    }

    public class Worker_Response : BaseResponseEntity
    {
        public string? WorkerName { get; set; }
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public int? WorkerTypeId { get; set; }
        public string? WorkerType { get; set; }
        public int? ContractTypeId { get; set; }
        public string? ContractType { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }
        public bool? IsActive { get; set; }
    }
}
