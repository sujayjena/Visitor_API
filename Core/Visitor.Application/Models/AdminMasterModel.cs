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

    #region Meeting Location
    public class MeetingLocation_Request : BaseEntity
    {
        [DefaultValue("")]
        public string MeetingLocation { get; set; }

        public bool? IsActive { get; set; }
    }

    public class MeetingLocation_Response : BaseResponseEntity
    {
        public string MeetingLocation { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Document Type
    public class DocumentType_Request : BaseEntity
    {
        public int DocumentId { get; set; }

        [DefaultValue("")]
        public string DocumentType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class DocumentType_Response : BaseResponseEntity
    {
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Rejection Reason
    public class RejectionReason_Request : BaseEntity
    {
        [DefaultValue("")]
        public string RejectionReason { get; set; }

        public bool? IsActive { get; set; }
    }

    public class RejectionReason_Response : BaseResponseEntity
    {
        public string RejectionReason { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region UOM
    public class UOM_Request : BaseEntity
    {
        [DefaultValue("")]
        public string UOMName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UOM_Response : BaseResponseEntity
    {
        public string UOMName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Canteen Name
    public class CanteenName_Request : BaseEntity
    {
        [DefaultValue("")]
        public string CanteenName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CanteenName_Response : BaseResponseEntity
    {
        public string CanteenName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Canteen Coupon Purpose
    public class CanteenCouponPurpose_Request : BaseEntity
    {
        public int CanteenId { get; set; }

        [DefaultValue("")]
        public string CouponCode { get; set; }

        [DefaultValue("")]
        public string CouponPurpose { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CanteenCouponPurpose_Response : BaseResponseEntity
    {
        public int CanteenId { get; set; }
        public string CanteenName { get; set; }
        public string CouponCode { get; set; }
        public string CouponPurpose { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Contract Type
    public class ContractType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string ContractType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ContractType_Response : BaseResponseEntity
    {
        public string ContractType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Discipline
    public class Discipline_Request : BaseEntity
    {
        [DefaultValue("")]
        public string Discipline { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Discipline_Response : BaseResponseEntity
    {
        public string Discipline { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Leave Type
    public class LeaveType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string LeaveType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class LeaveType_Response : BaseResponseEntity
    {
        public string LeaveType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Rooster Group
    public class RoosterGroup_Request : BaseEntity
    {
        [DefaultValue("")]
        public string RoosterGroup { get; set; }

        public bool? IsActive { get; set; }
    }

    public class RoosterGroup_Response : BaseResponseEntity
    {
        public string RoosterGroup { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Food Delivery Location
    public class FoodDeliveryLocation_Request : BaseEntity
    {
        [DefaultValue("")]
        public string FoodDeliveryLocation { get; set; }

        public bool? IsActive { get; set; }
    }

    public class FoodDeliveryLocation_Response : BaseResponseEntity
    {
        public string FoodDeliveryLocation { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Gate Type
    public class GateType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string GateType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class GateType_Response : BaseResponseEntity
    {
        public string GateType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Gate Name
    public class GateName_Request : BaseEntity
    {
        [DefaultValue("")]
        public string GateName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class GateName_Response : BaseResponseEntity
    {
        public string GateName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Gate Details
    public class GateDetails_Request : BaseEntity
    {
        public int GateNameId { get; set; }
        public int GateTypeId { get; set; }

        [DefaultValue("")]
        public string Remarks { get; set; }

        public bool? IsActive { get; set; }
    }

    public class GateDetails_Response : BaseResponseEntity
    {
        public int GateNameId { get; set; }
        public string GateName { get; set; }
        public int GateTypeId { get; set; }
        public string GateType { get; set; }
       
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion

    #region Worker Type
    public class WorkerType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string WorkerType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class WorkerType_Response : BaseResponseEntity
    {
        public string WorkerType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion
}
