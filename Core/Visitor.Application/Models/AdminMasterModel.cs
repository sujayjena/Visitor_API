﻿using System;
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

        [DefaultValue("")]
        public string UOMDesc { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UOM_Response : BaseResponseEntity
    {
        public string UOMName { get; set; }
        public string UOMDesc { get; set; }

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

    #region Meeting Type
    public class MeetingType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string MeetingType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class MeetingType_Response : BaseResponseEntity
    {
        public string MeetingType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Item Details
    public class ItemDetails_Request : BaseEntity
    {
        [DefaultValue("")]
        public string ItemCode { get; set; }

        [DefaultValue("")]
        public string ItemName { get; set; }

        [DefaultValue("")]
        public string ItemDesc { get; set; }

        public int UOMId { get; set; }

        public decimal ItemRate { get; set; }

        [DefaultValue("")]
        public string Serial { get; set; }

        [DefaultValue("")]
        public string Batch { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ItemDetails_Response : BaseResponseEntity
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public int UOMId { get; set; }
        public string UOMName { get; set; }
        public string ItemRate { get; set; }
        public string Serial { get; set; }
        public string Batch { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region ID Type
    public class IDType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string IDType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class IDType_Response : BaseResponseEntity
    {
        public string IDType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Contractor Type
    public class ContractorType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string ContractorType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ContractorType_Response : BaseResponseEntity
    {
        public string ContractorType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Template Type
    public class TemplateType_Request : BaseEntity
    {
        [DefaultValue("")]
        public string TemplateType { get; set; }

        public bool? IsActive { get; set; }
    }

    public class TemplateType_Response : BaseResponseEntity
    {
        public string TemplateType { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Template Text
    public class TemplateText_Request : BaseEntity
    {
        public int TemplateTypeId { get; set; }

        [DefaultValue("")]
        public string TemplateText { get; set; }

        public bool? IsActive { get; set; }
    }

    public class TemplateText_Response : BaseResponseEntity
    {
        public int TemplateTypeId { get; set; }
        public string TemplateType { get; set; }
        public string TemplateText { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Canteen Feedback
    public class CanteenFeedback_Request : BaseEntity
    {
        public int CanteenId { get; set; }

        [DefaultValue("")]
        public string CanteenFeedback { get; set; }

        public int Rating { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CanteenFeedback_Response : BaseResponseEntity
    {
        public int CanteenId { get; set; }
        public string CanteenName { get; set; }
        public string CanteenFeedback { get; set; }
        public int Rating { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Attendance
    public class Attendance_Request
    {
        public Attendance_Request()
        {
            attendanceDetails = new List<AttendanceDetails_Request>();
        }

        public List<AttendanceDetails_Request> attendanceDetails { get; set; }
    }

    public class AttendanceDetails_Request : BaseEntity
    {
        [DefaultValue("")]
        public string DayName { get; set; }

        [DefaultValue("")]
        public string ColorSelection { get; set; }
        public bool? IsActive { get; set; }
    }

    public class Attendance_Response : BaseResponseEntity
    {
        public string DayName { get; set; }
        public string ColorSelection { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Marital Status
    public class MaritalStatus_Request : BaseEntity
    {
        [DefaultValue("")]
        public string MaritalStatus { get; set; }

        public bool? IsActive { get; set; }
    }

    public class MaritalStatus_Response : BaseResponseEntity
    {
        public string MaritalStatus { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Blood Group
    public class BloodGroup_Request : BaseEntity
    {
        [DefaultValue("")]
        public string BloodGroup { get; set; }

        public bool? IsActive { get; set; }
    }

    public class BloodGroup_Response : BaseResponseEntity
    {
        public string BloodGroup { get; set; }

        public bool? IsActive { get; set; }
    }
    #endregion
}
