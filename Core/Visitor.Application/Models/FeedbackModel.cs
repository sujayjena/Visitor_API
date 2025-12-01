using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    public class Feedback_Search : BaseSearchEntity
    {
        [DefaultValue("Employee")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }
    }

    public class Feedback_Request : BaseEntity
    {
        [DefaultValue("Employee")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }
        public DateTime? CTDate { get; set; }
        public string? MealType { get; set; }
        public int? FeedbackValue { get; set; }
    }

    public class Feedback_Response : BaseResponseEntity
    {
        public string? RefType { get; set; }
        public int? RefId { get; set; }
        public string? RefName { get; set; }
        public DateTime? CTDate { get; set; }
        public string? MealType { get; set; }
        public int? FeedbackValue { get; set; }
    }
}
