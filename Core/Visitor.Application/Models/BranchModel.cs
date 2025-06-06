﻿using Visitor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    public class BranchModel
    {
    }

    public class Branch_Request : BaseEntity
    {
        public string? BranchName { get; set; }
        public int? CompanyId { get; set; }
        public string? EmailId { get; set; }
        public string? MobileNo { get; set; }
        public string? DepartmentHead { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public int? Pincode { get; set; }
        public int? GateDetailsId { get; set; }
        public int? NoofUserAdd { get; set; }
        public string? BranchOriginalFileName { get; set; }
        [JsonIgnore]
        public string? BranchFileName { get; set; }
        public string? Branch_Base64 { get; set; }
        public bool? IsActive { get; set; }
    }
    public class BranchSearch_Request : BaseSearchEntity
    {
        public int? CompanyId { get; set; }
    }
    public class Branch_Response : BaseResponseEntity
    {
        public string? BranchName { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? EmailId { get; set; }
        public string? MobileNo { get; set; }
        public string? DepartmentHead { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public int? Pincode { get; set; }
        public int? GateDetailsId { get; set; }
        public string? GateNumber { get; set; }
        public int? NoofUserAdd { get; set; }
        public string? BranchOriginalFileName { get; set; }
        public string? BranchFileName { get; set; }
        public string? BranchURL { get; set; }
        public bool? IsActive { get; set; }
    }

    public class BranchMapping_Request : BaseEntity
    {
        [JsonIgnore]
        public string? Action { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        public int BranchId { get; set; }
    }

    public class BranchMapping_Response : BaseEntity
    {
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
