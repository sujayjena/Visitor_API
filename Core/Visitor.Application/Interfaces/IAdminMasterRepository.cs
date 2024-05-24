using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IAdminMasterRepository
    {
        #region Gender
        Task<int> SaveGender(Gender_Request parameters);

        Task<IEnumerable<Gender_Response>> GetGenderList(BaseSearchEntity parameters);

        Task<Gender_Response?> GetGenderById(int Id);
        #endregion

        #region Visitor Type
        Task<int> SaveVisitorType(VisitorType_Request parameters);

        Task<IEnumerable<VisitorType_Response>> GetVisitorTypeList(BaseSearchEntity parameters);

        Task<VisitorType_Response?> GetVisitorTypeById(int Id);
        #endregion

        #region Visit Type
        Task<int> SaveVisitType(VisitType_Request parameters);

        Task<IEnumerable<VisitType_Response>> GetVisitTypeList(BaseSearchEntity parameters);

        Task<VisitType_Response?> GetVisitTypeById(int Id);
        #endregion

        #region Vehicle Type
        Task<int> SaveVehicleType(VehicleType_Request parameters);

        Task<IEnumerable<VehicleType_Response>> GetVehicleTypeList(BaseSearchEntity parameters);

        Task<VehicleType_Response?> GetVehicleTypeById(int Id);
        #endregion

        #region Material With Visitor
        Task<int> SaveMaterialWithVisitor(MaterialWithVisitor_Request parameters);

        Task<IEnumerable<MaterialWithVisitor_Response>> GetMaterialWithVisitorList(BaseSearchEntity parameters);

        Task<MaterialWithVisitor_Response?> GetMaterialWithVisitorById(int Id);
        #endregion

        #region Meeting Location
        Task<int> SaveMeetingLocation(MeetingLocation_Request parameters);

        Task<IEnumerable<MeetingLocation_Response>> GetMeetingLocationList(BaseSearchEntity parameters);

        Task<MeetingLocation_Response?> GetMeetingLocationById(int Id);
        #endregion

        #region Document Type
        Task<int> SaveDocumentType(DocumentType_Request parameters);

        Task<IEnumerable<DocumentType_Response>> GetDocumentTypeList(BaseSearchEntity parameters);

        Task<DocumentType_Response?> GetDocumentTypeById(int Id);
        #endregion

        #region Rejection Reason
        Task<int> SaveRejectionReason(RejectionReason_Request parameters);

        Task<IEnumerable<RejectionReason_Response>> GetRejectionReasonList(BaseSearchEntity parameters);

        Task<RejectionReason_Response?> GetRejectionReasonById(int Id);
        #endregion

        #region UOM
        Task<int> SaveUOM(UOM_Request parameters);

        Task<IEnumerable<UOM_Response>> GetUOMList(BaseSearchEntity parameters);

        Task<UOM_Response?> GetUOMById(int Id);
        #endregion

        #region Canteen Name
        Task<int> SaveCanteenName(CanteenName_Request parameters);

        Task<IEnumerable<CanteenName_Response>> GetCanteenNameList(BaseSearchEntity parameters);

        Task<CanteenName_Response?> GetCanteenNameById(int Id);
        #endregion

        #region Canteen Coupon Purpose
        Task<int> SaveCanteenCouponPurpose(CanteenCouponPurpose_Request parameters);

        Task<IEnumerable<CanteenCouponPurpose_Response>> GetCanteenCouponPurposeList(BaseSearchEntity parameters);

        Task<CanteenCouponPurpose_Response?> GetCanteenCouponPurposeById(int Id);
        #endregion

        #region Contract Type
        Task<int> SaveContractType(ContractType_Request parameters);

        Task<IEnumerable<ContractType_Response>> GetContractTypeList(BaseSearchEntity parameters);

        Task<ContractType_Response?> GetContractTypeById(int Id);
        #endregion

        #region Discipline
        Task<int> SaveDiscipline(Discipline_Request parameters);

        Task<IEnumerable<Discipline_Response>> GetDisciplineList(BaseSearchEntity parameters);

        Task<Discipline_Response?> GetDisciplineById(int Id);
        #endregion

        #region Leave Type
        Task<int> SaveLeaveType(LeaveType_Request parameters);

        Task<IEnumerable<LeaveType_Response>> GetLeaveTypeList(BaseSearchEntity parameters);

        Task<LeaveType_Response?> GetLeaveTypeById(int Id);
        #endregion

        #region Rooster Group
        Task<int> SaveRoosterGroup(RoosterGroup_Request parameters);

        Task<IEnumerable<RoosterGroup_Response>> GetRoosterGroupList(BaseSearchEntity parameters);

        Task<RoosterGroup_Response?> GetRoosterGroupById(int Id);
        #endregion

        #region Food Delivery Location
        Task<int> SaveFoodDeliveryLocation(FoodDeliveryLocation_Request parameters);

        Task<IEnumerable<FoodDeliveryLocation_Response>> GetFoodDeliveryLocationList(BaseSearchEntity parameters);

        Task<FoodDeliveryLocation_Response?> GetFoodDeliveryLocationById(int Id);
        #endregion

        #region Gate Type
        Task<int> SaveGateType(GateType_Request parameters);

        Task<IEnumerable<GateType_Response>> GetGateTypeList(BaseSearchEntity parameters);

        Task<GateType_Response?> GetGateTypeById(int Id);
        #endregion

        #region Gate Name
        Task<int> SaveGateName(GateName_Request parameters);

        Task<IEnumerable<GateName_Response>> GetGateNameList(BaseSearchEntity parameters);

        Task<GateName_Response?> GetGateNameById(int Id);
        #endregion

        #region Gate Details
        Task<int> SaveGateDetails(GateDetails_Request parameters);

        Task<IEnumerable<GateDetails_Response>> GetGateDetailsList(BaseSearchEntity parameters);

        Task<GateDetails_Response?> GetGateDetailsById(int Id);
        #endregion

        #region Worker Type
        Task<int> SaveWorkerType(WorkerType_Request parameters);

        Task<IEnumerable<WorkerType_Response>> GetWorkerTypeList(BaseSearchEntity parameters);

        Task<WorkerType_Response?> GetWorkerTypeById(int Id);
        #endregion

        #region Meeting Type
        Task<int> SaveMeetingType(MeetingType_Request parameters);

        Task<IEnumerable<MeetingType_Response>> GetMeetingTypeList(BaseSearchEntity parameters);

        Task<MeetingType_Response?> GetMeetingTypeById(int Id);
        #endregion

        #region Item Details
        Task<int> SaveItemDetails(ItemDetails_Request parameters);

        Task<IEnumerable<ItemDetails_Response>> GetItemDetailsList(BaseSearchEntity parameters);

        Task<ItemDetails_Response?> GetItemDetailsById(int Id);
        #endregion

        #region ID Type
        Task<int> SaveIDType(IDType_Request parameters);

        Task<IEnumerable<IDType_Response>> GetIDTypeList(BaseSearchEntity parameters);

        Task<IDType_Response?> GetIDTypeById(int Id);
        #endregion

        #region Contractor Type
        Task<int> SaveContractorType(ContractorType_Request parameters);

        Task<IEnumerable<ContractorType_Response>> GetContractorTypeList(BaseSearchEntity parameters);

        Task<ContractorType_Response?> GetContractorTypeById(int Id);
        #endregion

        #region Template Type
        Task<int> SaveTemplateType(TemplateType_Request parameters);

        Task<IEnumerable<TemplateType_Response>> GetTemplateTypeList(BaseSearchEntity parameters);

        Task<TemplateType_Response?> GetTemplateTypeById(int Id);
        #endregion

        #region Template Text
        Task<int> SaveTemplateText(TemplateText_Request parameters);

        Task<IEnumerable<TemplateText_Response>> GetTemplateTextList(BaseSearchEntity parameters);

        Task<TemplateText_Response?> GetTemplateTextById(int Id);
        #endregion

        #region Canteen Feedback
        Task<int> SaveCanteenFeedback(CanteenFeedback_Request parameters);

        Task<IEnumerable<CanteenFeedback_Response>> GetCanteenFeedbackList(BaseSearchEntity parameters);

        Task<CanteenFeedback_Response?> GetCanteenFeedbackById(int Id);
        #endregion

        #region Attendance
        Task<int> SaveAttendance(AttendanceDetails_Request parameters);

        Task<IEnumerable<Attendance_Response>> GetAttendanceList(BaseSearchEntity parameters);

        Task<Attendance_Response?> GetAttendanceById(int Id);
        #endregion
    }
}
