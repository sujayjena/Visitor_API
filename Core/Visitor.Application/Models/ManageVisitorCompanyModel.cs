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
    public class ManageVisitorCompanyModel
    {
    }

    public class VisitorCompany_Request : BaseEntity
    {
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public int? Pincode { get; set; }
        public string? CompanyPhone { get; set; }

        public string? GSTNo { get; set; }
        public string? GSTOriginalFileName { get; set; }
        [JsonIgnore]
        public string? GSTFileName { get; set; }
        public string? GSTFile_Base64 { get; set; }

        public string? PanCardNumber { get; set; }
        public string? PanCardOriginalFileName { get; set; }
        [JsonIgnore]
        public string? PanCardFileName { get; set; }
        public string? PanCardFile_Base64 { get; set; }
        public bool? IsActive { get; set; }
    }

    public class VisitorCompany_Response : BaseResponseEntity
    {
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? Pincode { get; set; }
        public string? CompanyPhone { get; set; }
        public string? GSTNo { get; set; }
        public string? GSTOriginalFileName { get; set; }
        public string? GSTFileName { get; set; }
        public string? GSTURL { get; set; }

        public string? PanCardNumber { get; set; }
        public string? PanCardOriginalFileName { get; set; }
        [JsonIgnore]
        public string? PanCardFileName { get; set; }
        public string? PanCardURL { get; set; }
        public bool? IsActive { get; set; }
    }

}
