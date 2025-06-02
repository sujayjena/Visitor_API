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
    public class AssignGateNoRepository : GenericRepository, IAssignGateNoRepository
    {
        private IConfiguration _configuration;

        public AssignGateNoRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveAssignGateNo(AssignGateNo_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@GateDetailsId", parameters.GateDetailsId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveAssignGateNo", queryParameters);
        }

        public async Task<IEnumerable<AssignGateNo_Response>> GetAssignGateNoById(long RefId, string RefType, long GateDetailsId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RefId", RefId);
            queryParameters.Add("@RefType", RefType);
            queryParameters.Add("@GateDetailsId", GateDetailsId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<AssignGateNo_Response>("GetAssignGateNoById", queryParameters);

            return result;
        }
    }
}
