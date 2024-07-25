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
    public class ManageRFIDRepository : GenericRepository, IManageRFIDRepository
    {
        private IConfiguration _configuration;

        public ManageRFIDRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region RFID
        public async Task<int> SaveRFID(RFID_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RFID", parameters.RFID);
            queryParameters.Add("@BalanceAmt", parameters.BalanceAmt);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveRFID", queryParameters);
        }

        public async Task<IEnumerable<RFID_Response>> GetRFIDList(RFID_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<RFID_Response>("GetRFIDList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<RFID_Response?> GetRFIDById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<RFID_Response>("GetRFIDById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region RFID Topup
        public async Task<int> SaveRFIDTopup(RFIDTopupDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RFIDId", parameters.RFIDId);
            queryParameters.Add("@MenuItemId", parameters.MenuItemId);
            queryParameters.Add("@Amount", parameters.Amount);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveRFIDTopup", queryParameters);
        }

        public async Task<IEnumerable<RFIDTopup_Response>> GetRFIDTopupList(RFIDTopup_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RFIDId", parameters.RFIDId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<RFIDTopup_Response>("GetRFIDTopupList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion
    }
}
