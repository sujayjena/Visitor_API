using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Visitor.Application.Models
{
    public class AssignGateNo_Request
    {
        public int Id { get; set; }

        [JsonIgnore]
        public string? Action { get; set; }

        [JsonIgnore]
        public int? RefId { get; set; }

        [JsonIgnore]
        public string? RefType { get; set; }
        public int? GateDetailsId { get; set; }
    }

    public class AssignGateNo_Response
    {
        public int Id { get; set; }
        public int? RefId { get; set; }
        public string? RefType { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
    }
}
