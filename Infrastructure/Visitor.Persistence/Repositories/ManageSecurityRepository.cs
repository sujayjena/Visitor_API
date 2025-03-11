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
    public class ManageSecurityRepository : GenericRepository, IManageSecurityRepository
    {
        private IConfiguration _configuration;

        public ManageSecurityRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveSecurityLogin(SecurityLogin_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@SecurityName", parameters.SecurityName);
            queryParameters.Add("@SecurityMobileNo", parameters.SecurityMobileNo);
            queryParameters.Add("@ContractorTypeId", parameters.ContractorTypeId);
            queryParameters.Add("@ContractorId", parameters.ContractorId);
            queryParameters.Add("@DocumentTypeId", parameters.DocumentTypeId);
            queryParameters.Add("@DocumentOriginalFileName", parameters.DocumentOriginalFileName);
            queryParameters.Add("@DocumentFileName", parameters.DocumentFileName);
            queryParameters.Add("@PhotoOriginalFileName", parameters.PhotoOriginalFileName);
            queryParameters.Add("@PhotoFileName", parameters.PhotoFileName);
            queryParameters.Add("@Passwords", !string.IsNullOrWhiteSpace(parameters.Passwords) ? EncryptDecryptHelper.EncryptString(parameters.Passwords) : string.Empty);
            queryParameters.Add("@IsBlackListed", parameters.IsBlackListed);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveSecurityLogin", queryParameters);
        }

        public async Task<IEnumerable<SecurityLogin_Response>> GetSecurityLoginList(SecurityLogin_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsBlackListed", parameters.IsBlackListed);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<SecurityLogin_Response>("GetSecurityLoginList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<SecurityLogin_Response?> GetSecurityLoginById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<SecurityLogin_Response>("GetSecurityLoginById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveSecurityLoginGateDetails(SecurityLoginGateDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@SecurityLoginId", parameters.SecurityLoginId);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveSecurityLoginGateDetails", queryParameters);
        }

        public async Task<IEnumerable<SecurityLoginGateDetails_Response>> GetSecurityLoginGateDetailsById(int SecurityLoginId, int GateDetailsId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SecurityLoginId", SecurityLoginId);
            queryParameters.Add("@GateDetailsId", GateDetailsId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<SecurityLoginGateDetails_Response>("GetSecurityLoginGateDetailsById", queryParameters);

            return result;
        }
    }
}

