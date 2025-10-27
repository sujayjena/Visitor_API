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
    public class ManageVendorRepository : GenericRepository, IManageVendorRepository
    {
        private IConfiguration _configuration;

        public ManageVendorRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveVendor(Vendor_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VendorCode", parameters.VendorCode);
            queryParameters.Add("@VendorName", parameters.VendorName);
            queryParameters.Add("@LandlineNumber", parameters.LandlineNumber);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@EmailId", parameters.EmailId);
            queryParameters.Add("@SpecialRemarks", parameters.SpecialRemarks);
            queryParameters.Add("@PanCardNumber", parameters.PanCardNumber);
            queryParameters.Add("@PanCardOriginalFileName", parameters.PanCardOriginalFileName);
            queryParameters.Add("@PanCardFileName", parameters.PanCardFileName);
            queryParameters.Add("@GSTNumber", parameters.GSTNumber);
            queryParameters.Add("@GSTOriginalFileName", parameters.GSTOriginalFileName);
            queryParameters.Add("@GSTFileName", parameters.GSTFileName);
            queryParameters.Add("@CustomerName", parameters.CustomerName);
            queryParameters.Add("@CustMobileNumber", parameters.CustMobileNumber);
            queryParameters.Add("@CustEmailId", parameters.CustEmailId);
            queryParameters.Add("@AddressLine1", parameters.AddressLine1);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@PinCode", parameters.PinCode);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVendor", queryParameters);
        }

        public async Task<IEnumerable<Vendor_Response>> GetVendorList(Vendor_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Vendor_Response>("GetVendorList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Vendor_Response?> GetVendorById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Vendor_Response>("GetVendorById", queryParameters)).FirstOrDefault();
        }
    }
}
