using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<int> SaveFeedback(Feedback_Request parameters);
        Task<IEnumerable<Feedback_Response>> GetFeedbackList(Feedback_Search parameters);
    }
}
