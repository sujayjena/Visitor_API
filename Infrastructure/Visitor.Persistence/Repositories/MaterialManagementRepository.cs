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
    public class MaterialManagementRepository : GenericRepository, IMaterialManagementRepository
    {
        private IConfiguration _configuration;

        public MaterialManagementRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Material Request
        public async Task<int> SaveMaterialRequest(MaterialRequest_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@TransactionTypeId", parameters.TransactionTypeId);
            queryParameters.Add("@ContractorTypeId", parameters.ContractorTypeId);
            queryParameters.Add("@ContractorId", parameters.ContractorId);
            queryParameters.Add("@PurchaseOrderNo", parameters.PurchaseOrderNo);
            queryParameters.Add("@WorkerName", parameters.WorkerName);
            queryParameters.Add("@WorkerTypeId", parameters.WorkerTypeId);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@ValidFromDate", parameters.ValidFromDate);
            queryParameters.Add("@ValidToDate", parameters.ValidToDate);
            queryParameters.Add("@POStartDate", parameters.POStartDate);
            queryParameters.Add("@POEndDate", parameters.POEndDate);
            queryParameters.Add("@DateOfBirth", parameters.DateOfBirth);
            queryParameters.Add("@BloodGroupId", parameters.BloodGroupId);
            queryParameters.Add("@IdentificationMark", parameters.IdentificationMark);
            queryParameters.Add("@WorkerPhotoOriginalFileName", parameters.WorkerPhotoOriginalFileName);
            queryParameters.Add("@WorkerPhotoFileName", parameters.WorkerPhotoFileName);
            queryParameters.Add("@AddressLine", parameters.AddressLine);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@WorkPlaceId", parameters.WorkPlaceId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@IsDriver", parameters.IsDriver);
            queryParameters.Add("@VehicleNumber", parameters.VehicleNumber);
            queryParameters.Add("@DrivingLicenseNumber", parameters.DrivingLicenseNumber);
            queryParameters.Add("@LicenseValidFrom", parameters.LicenseValidFrom);
            queryParameters.Add("@LicenseValidTo", parameters.LicenseValidTo);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@IsBlackList", parameters.IsBlackList);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMaterialRequest", queryParameters);
        }
        public async Task<IEnumerable<MaterialRequestList_Response>> GetMaterialRequestList(MaterialRequest_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@MaterialRequestId", parameters.MaterialRequestId);
            queryParameters.Add("@TransactionTypeId", parameters.TransactionTypeId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MaterialRequestList_Response>("GetMaterialRequestList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<MaterialRequest_Response?> GetMaterialRequestById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MaterialRequest_Response>("GetMaterialRequestById", queryParameters)).FirstOrDefault();
        }
        public async Task<int> MaterialRequestApproveNReject(MaterialRequest_ApproveNReject parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@StatusId", parameters.StatusId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("MaterialRequestApproveNReject", queryParameters);
        }

        #endregion

        #region Material Request Details
        public async Task<int> SaveMaterialRequestDetails(MaterialRequestDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MaterialRequestId", parameters.MaterialRequestId);
            queryParameters.Add("@MaterialDetailsId", parameters.MaterialDetailsId);
            queryParameters.Add("@Quantity", parameters.Quantity);
            queryParameters.Add("@Rate", parameters.Rate);
            queryParameters.Add("@Amount", parameters.Amount);
            queryParameters.Add("@SerialNo", parameters.SerialNo);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMaterialRequestDetails", queryParameters);
        }

        public async Task<IEnumerable<MaterialRequestDetails_Response>> GetMaterialRequestDetailsList(MaterialRequestDetails_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@MaterialRequestId", parameters.MaterialRequestId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MaterialRequestDetails_Response>("GetMaterialRequestDetailsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion
    }
}
