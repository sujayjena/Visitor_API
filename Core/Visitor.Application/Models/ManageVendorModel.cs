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
    public class Vendor_Request : BaseEntity
    {
        public string? VendorCode { get; set; }
        public string? VendorName { get; set; }
        public string? LandlineNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? SpecialRemarks { get; set; }

        public string? PanCardNumber { get; set; }

        [DefaultValue("")]
        public string? PanCardOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? PanCardFileName { get; set; }

        [DefaultValue("")]
        public string? PanCard_Base64 { get; set; }

        public string? GSTNumber { get; set; }

        [DefaultValue("")]
        public string? GSTOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? GSTFileName { get; set; }

        [DefaultValue("")]
        public string? GST_Base64 { get; set; }

        public string? CustomerName { get; set; }
        public string? CustMobileNumber { get; set; }
        public string? CustEmailId { get; set; }
        public string? AddressLine1 { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public string? PinCode { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Vendor_Search : BaseSearchEntity
    {
    }

    public class Vendor_Response : BaseResponseEntity
    {
        public string? VendorCode { get; set; }
        public string? VendorName { get; set; }
        public string? LandlineNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public string? SpecialRemarks { get; set; }

        public string? PanCardNumber { get; set; }

        [DefaultValue("")]
        public string? PanCardOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? PanCardFileName { get; set; }

        [DefaultValue("")]
        public string? PanCardUrl { get; set; }

        public string? GSTNumber { get; set; }

        [DefaultValue("")]
        public string? GSTOriginalFileName { get; set; }

        [DefaultValue("")]
        public string? GSTFileName { get; set; }

        [DefaultValue("")]
        public string? GSTUrl { get; set; }

        public string? CustomerName { get; set; }
        public string? CustMobileNumber { get; set; }
        public string? CustEmailId { get; set; }
        public string? AddressLine1 { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public string? PinCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
