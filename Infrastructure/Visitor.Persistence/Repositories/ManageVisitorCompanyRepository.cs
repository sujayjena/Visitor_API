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
    public class ManageVisitorCompanyRepository : GenericRepository, IManageVisitorCompanyRepository
    {
        private IConfiguration _configuration;

        public ManageVisitorCompanyRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveVisitorCompany(VisitorCompany_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CompanyName", parameters.CompanyName);
            queryParameters.Add("@CompanyAddress", parameters.CompanyAddress);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@CompanyPhone", parameters.CompanyPhone);
            queryParameters.Add("@GSTNo", parameters.GSTNo);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorCompany", queryParameters);
        }

        public async Task<IEnumerable<VisitorCompany_Response>> GetVisitorCompanyList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorCompany_Response>("GetVisitorCompanyList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<VisitorCompany_Response?> GetVisitorCompanyById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<VisitorCompany_Response>("GetVisitorCompanyById", queryParameters)).FirstOrDefault();
        }

    }
}
