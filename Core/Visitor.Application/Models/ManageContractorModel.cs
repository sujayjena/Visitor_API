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
    public class ManageContractorModel
    {
    }

    #region Contrator
    public class Contractor_Request : BaseEntity
    {
        public Contractor_Request()
        {
            assetList = new List<ContractorAsset_Request>();
            DocumentVerificationList = new List<VisitorDocumentVerification_Request>();
        }
        public int? ContractorTypeId { get; set; }
        public string? ContractorName { get; set; }
        public string? ContractorPerson { get; set; }

        [JsonIgnore]
        public int? NoofContractedWorkers { get; set; }
        public int? NoofContractedPO { get; set; }
        public string? MobileNo { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
        public string? Pincode { get; set; }
        public int? DocumentTypeId { get; set; }

        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentImage { get; set; }
        public string? DocumentImage_Base64 { get; set; }

        public string? ContractorPhotoOriginalFileName { get; set; }
        public string? ContractorPhoto { get; set; }
        public string? ContractorPhoto_Base64 { get; set; }

        public string? AadharCardNumber { get; set; }
        public string? AadharCardOriginalFileName { get; set; }
        public string? AadharCardFileName { get; set; }
        public string? AadharCard_Base64 { get; set; }

        public string? PanCardNumber { get; set; }
        public string? PanCardOriginalFileName { get; set; }
        public string? PanCardFileName { get; set; }
        public string? PanCard_Base64 { get; set; }

        public int? ContractorLevel { get; set; }
        public int? BranchId { get; set; }
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

        [DefaultValue(false)]
        public bool? IsCanteenPrivilege { get; set; }
        public bool? IsActive { get; set; }

        [DefaultValue(false)]
        public bool? IsBlackList { get; set; }

        public List<ContractorAsset_Request> assetList { get; set; }
        public List<VisitorDocumentVerification_Request> DocumentVerificationList { get; set; }
    }

    public class ContractorSearch_Request : BaseSearchEntity
    {
        public int? ContractorTypeId { get; set; }

        [DefaultValue(null)]
        public bool? IsBlackList { get; set; }
        public int? IsExpired { get; set; }
        public int? BranchId { get; set; }
    }

    public class Contractor_Response : BaseResponseEntity
    {
        public Contractor_Response()
        {
            assetList = new List<ContractorAsset_Response>();
        }
        public int? ContractorTypeId { get; set; }
        public string? ContractorType { get; set; }
        public string? ContractorName { get; set; }
        public string? ContractorPerson { get; set; }

        [JsonIgnore]
        public int? NoofContractedWorkers { get; set; }
        public int? NoofContractedPO { get; set; }
        public string? MobileNo { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public DateTime? ValidToDate { get; set; }
        public string? AddressLine { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public string? Pincode { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentOriginalFileName { get; set; }
        public string? DocumentImage { get; set; }
        public string? DocumentImageURL { get; set; }
        public string? ContractorPhotoOriginalFileName { get; set; }
        public string? ContractorPhoto { get; set; }
        public string? ContractorPhotoURL { get; set; }
        public string? AadharCardNumber { get; set; }
        public string? AadharCardOriginalFileName { get; set; }
        public string? AadharCardFileName { get; set; }
        public string? AadharCardURL { get; set; }
        public string? PanCardNumber { get; set; }
        public string? PanCardOriginalFileName { get; set; }
        public string? PanCardFileName { get; set; }
        public string? PanCardURL { get; set; }
        public int? ContractorLevel { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
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
        public bool? IsCanteenPrivilege { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlackList { get; set; }
        public List<ContractorAsset_Response> assetList { get; set; }
        public List<VisitorDocumentVerification_Response> DocumentVerificationList { get; set; }
    }

    public class ContractorAsset_Request : BaseEntity
    {
        [JsonIgnore]
        public int? ContractorId { get; set; }

        [DefaultValue("")]
        public string? AssetName { get; set; }

        [DefaultValue("")]
        public string? AssetDesc { get; set; }
        public decimal? Quantity { get; set; }
        public int? UOMId { get; set; }
    }

    public class ContractorAsset_Search : BaseSearchEntity
    {
        public int? ContractorId { get; set; }
    }

    public class ContractorAsset_Response : BaseResponseEntity
    {
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }

        [DefaultValue("")]
        public string? AssetName { get; set; }

        [DefaultValue("")]
        public string? AssetDesc { get; set; }
        public decimal? Quantity { get; set; }
        public int? UOMId { get; set; }
        public string? UOMName { get; set; }
    }
    #endregion

    #region Contractor Insurance
    public class ContractorInsurance_Request : BaseEntity
    {
        public int? ContractorId { get; set; }
        public string? InsuranceIssuedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Validity { get; set; }
        public int? RemainingDays { get; set; }
        public bool? InsuranceStatus { get; set; }
        public string? InsuranceOriginalFileName { get; set; }
        public string? InsuranceFileName { get; set; }
        public string? Insurance_Base64 { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ContractorInsuranceSearch_Request : BaseSearchEntity
    {
        [DefaultValue(null)]
        public bool? InsuranceStatus { get; set; }
    }

    public class ContractorInsurance_Response : BaseResponseEntity
    {
        public int? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public string? InsuranceIssuedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Validity { get; set; }
        public int? RemainingDays { get; set; }
        public bool? InsuranceStatus { get; set; }
        public string? InsuranceOriginalFileName { get; set; }
        public string? InsuranceFileName { get; set; }
        public string? InsuranceURL { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion

}
