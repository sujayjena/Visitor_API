using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Persistence.Repositories
{
    public class CompanyRepository : GenericRepository, ICompanyRepository
    {
        private IConfiguration _configuration;

        public CompanyRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveCompany(Company_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CompanyName", parameters.CompanyName);
            queryParameters.Add("@CompanyTypeId", parameters.CompanyTypeId);
            queryParameters.Add("@RegistrationNumber", parameters.RegistrationNumber);
            queryParameters.Add("@ContactNumber", parameters.ContactNumber);
            queryParameters.Add("@Email", parameters.Email);
            queryParameters.Add("@Website", parameters.Website);
            queryParameters.Add("@TaxNumber", parameters.TaxNumber);
            queryParameters.Add("@AddressLine1", parameters.AddressLine1);
            queryParameters.Add("@AddressLine2", parameters.AddressLine2);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@GSTNumber", parameters.GSTNumber);
            queryParameters.Add("@PANNumber", parameters.PANNumber);
            queryParameters.Add("@CompanyLogo", parameters.CompanyLogoFileName);
            queryParameters.Add("@NoofUserAdd", parameters.NoofUserAdd);
            queryParameters.Add("@NoofBranchAdd", parameters.NoofBranchAdd);
            queryParameters.Add("@LicenseStartDate", parameters.LicenseStartDate);
            queryParameters.Add("@LicenseEndDate", parameters.LicenseEndDate);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCompany", queryParameters);
        }

        public async Task<IEnumerable<Company_Response>> GetCompanyList(CompanySearch_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CompanyId", parameters.CompanyId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Company_Response>("GetCompanyList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Company_Response?> GetCompanyById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Company_Response>("GetCompanyById", queryParameters)).FirstOrDefault();
        }
    }
}
