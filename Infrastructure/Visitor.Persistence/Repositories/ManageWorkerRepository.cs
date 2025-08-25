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
    public class ManageWorkerRepository : GenericRepository, IManageWorkerRepository
    {
        private IConfiguration _configuration;

        public ManageWorkerRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveWorker(Worker_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@WorkerName", parameters.WorkerName);
            queryParameters.Add("@PurchaseOrderId", parameters.PurchaseOrderId);
            queryParameters.Add("@WorkerTypeId", parameters.WorkerTypeId);
            queryParameters.Add("@ContractTypeId", parameters.ContractTypeId);
            queryParameters.Add("@WorkerMobileNo", parameters.WorkerMobileNo);
            queryParameters.Add("@DocumentTypeId", parameters.DocumentTypeId);
            queryParameters.Add("@DocumentNumber", parameters.DocumentNumber);
            queryParameters.Add("@ValidFromDate", parameters.ValidFromDate);
            queryParameters.Add("@ValidToDate", parameters.ValidToDate);
            queryParameters.Add("@WorkerId", parameters.WorkerId);
            queryParameters.Add("@DOB", parameters.DOB);
            queryParameters.Add("@BloodGroupId", parameters.BloodGroupId);
            queryParameters.Add("@IdentificationMark", parameters.IdentificationMark);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@WorkPlaceId", parameters.WorkPlaceId);
            queryParameters.Add("@Address", parameters.Address);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@DocumentOriginalFileName", parameters.DocumentOriginalFileName);
            queryParameters.Add("@DocumentFileName", parameters.DocumentFileName);
            queryParameters.Add("@WorkerPhotoOriginalFileName", parameters.WorkerPhotoOriginalFileName);
            queryParameters.Add("@WorkerPhotoFileName", parameters.WorkerPhotoFileName);
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
            queryParameters.Add("@WorkerShift", parameters.WorkerShift);
            queryParameters.Add("@IsPoliceV", parameters.IsPoliceV);
            queryParameters.Add("@PoliceVOriginalFileName", parameters.PoliceVOriginalFileName);
            queryParameters.Add("@PoliceVFileName", parameters.PoliceVFileName);
            queryParameters.Add("@IsFitnessCert", parameters.IsFitnessCert);
            queryParameters.Add("@FitnessCertOriginalFileName", parameters.FitnessCertOriginalFileName);
            queryParameters.Add("@FitnessCertFileName", parameters.FitnessCertFileName);

            queryParameters.Add("@IsBlackList", parameters.IsBlackList);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorker", queryParameters);
        }

        public async Task<IEnumerable<Worker_Response>> GetWorkerList(WorkerSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsBlackList", parameters.IsBlackList);
            queryParameters.Add("@PurchaseOrderId", parameters.PurchaseOrderId);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Worker_Response>("GetWorkerList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Worker_Response?> GetWorkerById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Worker_Response>("GetWorkerById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveWorkerPass(WorkerPass_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@WorkerId", parameters.WorkerId);
            queryParameters.Add("@PassNumber", parameters.PassNumber);
            queryParameters.Add("@ValidFromDate", parameters.ValidFromDate);
            queryParameters.Add("@ValidToDate", parameters.ValidToDate);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkerPass", queryParameters);
        }

        public async Task<IEnumerable<WorkerPass_Response>> GetWorkerPassList(WorkerPassSearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@WorkerId", parameters.WorkerId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkerPass_Response>("GetWorkerPassList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }
    }
}
