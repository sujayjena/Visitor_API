using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.Persistence.Repositories
{
    public class ManageContractorRepository : GenericRepository, IManageContractorRepository
    {
        private IConfiguration _configuration;

        public ManageContractorRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Contractor
        public async Task<int> SaveContractor(Contractor_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ContractorTypeId", parameters.ContractorTypeId);
            queryParameters.Add("@ContractorName", parameters.ContractorName);
            queryParameters.Add("@ContractorPerson", parameters.ContractorPerson);
            queryParameters.Add("@NoofContractedWorkers", parameters.NoofContractedWorkers);
            queryParameters.Add("@NoofContractedPO", parameters.NoofContractedPO);
            queryParameters.Add("@MobileNo", parameters.MobileNo);
            queryParameters.Add("@ValidFromDate", parameters.ValidFromDate);
            queryParameters.Add("@ValidToDate", parameters.ValidToDate);
            queryParameters.Add("@AddressLine", parameters.AddressLine);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@DocumentTypeId", parameters.DocumentTypeId);
            queryParameters.Add("@DocumentOriginalFileName", parameters.DocumentOriginalFileName);
            queryParameters.Add("@DocumentImage", parameters.DocumentImage);
            queryParameters.Add("@ContractorPhotoOriginalFileName", parameters.ContractorPhotoOriginalFileName);
            queryParameters.Add("@ContractorPhoto", parameters.ContractorPhoto);
            queryParameters.Add("@AadharCardNumber", parameters.AadharCardNumber);
            queryParameters.Add("@AadharCardOriginalFileName", parameters.AadharCardOriginalFileName);
            queryParameters.Add("@AadharCardFileName", parameters.AadharCardFileName);
            queryParameters.Add("@PanCardNumber", parameters.PanCardNumber);
            queryParameters.Add("@PanCardOriginalFileName", parameters.PanCardOriginalFileName);
            queryParameters.Add("@PanCardFileName", parameters.PanCardFileName);
            queryParameters.Add("@ContractorLevel", parameters.ContractorLevel);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@DV_IsInsurance", parameters.DV_IsInsurance);
            queryParameters.Add("@DV_InsuranceNumber", parameters.DV_InsuranceNumber);
            queryParameters.Add("@DV_InsuranceOriginalFileName", parameters.DV_InsuranceOriginalFileName);
            queryParameters.Add("@DV_InsuranceFileName", parameters.DV_InsuranceFileName);
            queryParameters.Add("@DV_IsWC", parameters.DV_IsWC);
            queryParameters.Add("@DV_WCNumber", parameters.DV_WCNumber);
            queryParameters.Add("@DV_WCOriginalFileName", parameters.DV_WCOriginalFileName);
            queryParameters.Add("@DV_WCFileName", parameters.DV_WCFileName);
            queryParameters.Add("@DV_IsESIC", parameters.DV_IsESIC);
            queryParameters.Add("@DV_ESICNumber", parameters.DV_ESICNumber);
            queryParameters.Add("@DV_ESICOriginalFileName", parameters.DV_ESICOriginalFileName);
            queryParameters.Add("@DV_ESICFileName", parameters.DV_ESICFileName);

            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsBlackList", parameters.IsBlackList);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveContractor", queryParameters);
        }

        public async Task<IEnumerable<Contractor_Response>> GetContractorList(ContractorSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ContractorTypeId", parameters.ContractorTypeId);
            queryParameters.Add("@IsExpired", parameters.IsExpired);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsBlackList", parameters.IsBlackList);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Contractor_Response>("GetContractorList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Contractor_Response?> GetContractorById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Contractor_Response>("GetContractorById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region Contractor Insurance
        public async Task<int> SaveContractorInsurance(ContractorInsurance_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ContractorId", parameters.ContractorId);
            queryParameters.Add("@InsuranceIssuedBy", parameters.InsuranceIssuedBy);
            queryParameters.Add("@FromDate", parameters.FromDate);
            queryParameters.Add("@ToDate", parameters.ToDate);
            queryParameters.Add("@Validity", parameters.Validity);
            queryParameters.Add("@RemainingDays", parameters.RemainingDays);
            queryParameters.Add("@InsuranceStatus", parameters.InsuranceStatus);
            queryParameters.Add("@InsuranceOriginalFileName", parameters.InsuranceOriginalFileName);
            queryParameters.Add("@InsuranceFileName", parameters.InsuranceFileName);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveContractorInsurance", queryParameters);
        }

        public async Task<IEnumerable<ContractorInsurance_Response>> GetContractorInsuranceList(ContractorInsuranceSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@InsuranceStatus", parameters.InsuranceStatus);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ContractorInsurance_Response>("GetContractorInsuranceList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<ContractorInsurance_Response?> GetContractorInsuranceById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<ContractorInsurance_Response>("GetContractorInsuranceById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region Contractor Asset
        public async Task<int> SaveContractorAsset(ContractorAsset_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ContractorId", parameters.ContractorId);
            queryParameters.Add("@AssetName", parameters.AssetName);
            queryParameters.Add("@AssetDesc", parameters.AssetDesc);
            queryParameters.Add("@Quantity", parameters.Quantity);
            queryParameters.Add("@UOMId", parameters.UOMId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveContractorAsset", queryParameters);
        }

        public async Task<IEnumerable<ContractorAsset_Response>> GetContractorAssetList(ContractorAsset_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ContractorId", parameters.ContractorId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ContractorAsset_Response>("GetContractorAssetList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> DeleteContractorAsset(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", Id);

            return await SaveByStoredProcedure<int>("DeleteContractorAsset", queryParameters);
        }
        #endregion
    }
}
