using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Domain.Entities;

namespace Visitor.Application.Models
{
    public class AdminMasterModel
    {
    }

    #region Gender

    public class Gender_Request : BaseEntity
    {
        [DefaultValue("")]
        public string GenderName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Gender_Response : BaseResponseEntity
    {
        public string GenderName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Visitor Type
    public class VisitorType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string VisitorType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class VisitorType_Response : BaseResponseEntity
    {
        public string VisitorType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Visit Type
    public class VisitType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string VisitType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class VisitType_Response : BaseResponseEntity
    {
        public string? VisitType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Vehicle Type
    public class VehicleType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string VehicleType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class VehicleType_Response : BaseResponseEntity
    {
        public string VehicleType { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion

    #region Material With Visitor
    public class MaterialWithVisitor_Request : BaseEntity
    {
        [DefaultValue("")]
        public string MaterialWithVisitor { get; set; }

        public bool? IsActive { get; set; }
    }

    public class MaterialWithVisitor_Response : BaseResponseEntity
    {
        public string MaterialWithVisitor { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion
}
