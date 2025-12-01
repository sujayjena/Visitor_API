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
    public class FeedbackRepository : GenericRepository, IFeedbackRepository
    {
        private IConfiguration _configuration;

        public FeedbackRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveFeedback(Feedback_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RefType", parameters.RefType);
            queryParameters.Add("@RefId", parameters.RefId);
            queryParameters.Add("@CTDate", parameters.CTDate);
            queryParameters.Add("@MealType", parameters.MealType);
            queryParameters.Add("@FeedbackValue", parameters.FeedbackValue);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFeedback", queryParameters);
        }

        public async Task<IEnumerable<Feedback_Response>> GetFeedbackList(Feedback_Search parameters)
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

            var result = await ListByStoredProcedure<Feedback_Response>("GetFeedbackList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

    }
}
