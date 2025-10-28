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
    public class AdminMasterRepository : GenericRepository, IAdminMasterRepository
    {
        private IConfiguration _configuration;

        public AdminMasterRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Gender
        public async Task<int> SaveGender(Gender_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@GenderName", parameters.GenderName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveGender", queryParameters);
        }

        public async Task<IEnumerable<Gender_Response>> GetGenderList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Gender_Response>("GetGenderList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Gender_Response?> GetGenderById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Gender_Response>("GetGenderById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Visitor Type
        public async Task<int> SaveVisitorType(VisitorType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VisitorType", parameters.VisitorType.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitorType", queryParameters);
        }

        public async Task<IEnumerable<VisitorType_Response>> GetVisitorTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitorType_Response>("GetVisitorTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<VisitorType_Response?> GetVisitorTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<VisitorType_Response>("GetVisitorTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Visit Type
        public async Task<int> SaveVisitType(VisitType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VisitType", parameters.VisitType.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVisitType", queryParameters);
        }

        public async Task<IEnumerable<VisitType_Response>> GetVisitTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VisitType_Response>("GetVisitTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<VisitType_Response?> GetVisitTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<VisitType_Response>("GetVisitTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Vehicle Type
        public async Task<int> SaveVehicleType(VehicleType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VehicleType", parameters.VehicleType.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveVehicleType", queryParameters);
        }

        public async Task<IEnumerable<VehicleType_Response>> GetVehicleTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<VehicleType_Response>("GetVehicleTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<VehicleType_Response?> GetVehicleTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<VehicleType_Response>("GetVehicleTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Material With Visitor
        public async Task<int> SaveMaterialWithVisitor(MaterialWithVisitor_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MaterialWithVisitor", parameters.MaterialWithVisitor.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMaterialWithVisitor", queryParameters);
        }

        public async Task<IEnumerable<MaterialWithVisitor_Response>> GetMaterialWithVisitorList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MaterialWithVisitor_Response>("GetMaterialWithVisitorList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MaterialWithVisitor_Response?> GetMaterialWithVisitorById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MaterialWithVisitor_Response>("GetMaterialWithVisitorById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Meeting Location
        public async Task<int> SaveMeetingLocation(MeetingLocation_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MeetingLocation", parameters.MeetingLocation.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMeetingLocation", queryParameters);
        }

        public async Task<IEnumerable<MeetingLocation_Response>> GetMeetingLocationList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MeetingLocation_Response>("GetMeetingLocationList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MeetingLocation_Response?> GetMeetingLocationById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MeetingLocation_Response>("GetMeetingLocationById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Document Type
        public async Task<int> SaveDocumentType(DocumentType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DocumentId", parameters.DocumentId);
            queryParameters.Add("@DocumentType", parameters.DocumentType.SanitizeValue());
            queryParameters.Add("@Purpose", parameters.Purpose);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveDocumentType", queryParameters);
        }

        public async Task<IEnumerable<DocumentType_Response>> GetDocumentTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<DocumentType_Response>("GetDocumentTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<DocumentType_Response?> GetDocumentTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<DocumentType_Response>("GetDocumentTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Rejection Reason
        public async Task<int> SaveRejectionReason(RejectionReason_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RejectionReason", parameters.RejectionReason.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveRejectionReason", queryParameters);
        }

        public async Task<IEnumerable<RejectionReason_Response>> GetRejectionReasonList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<RejectionReason_Response>("GetRejectionReasonList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<RejectionReason_Response?> GetRejectionReasonById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<RejectionReason_Response>("GetRejectionReasonById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region UOM
        public async Task<int> SaveUOM(UOM_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@UOMName", parameters.UOMName.SanitizeValue());
            queryParameters.Add("@UOMDesc", parameters.UOMDesc.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveUOM", queryParameters);
        }

        public async Task<IEnumerable<UOM_Response>> GetUOMList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<UOM_Response>("GetUOMList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<UOM_Response?> GetUOMById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<UOM_Response>("GetUOMById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Canteen Name
        public async Task<int> SaveCanteenName(CanteenName_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CanteenName", parameters.CanteenName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCanteenName", queryParameters);
        }

        public async Task<IEnumerable<CanteenName_Response>> GetCanteenNameList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CanteenName_Response>("GetCanteenNameList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<CanteenName_Response?> GetCanteenNameById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<CanteenName_Response>("GetCanteenNameById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Canteen Coupon Purpose
        public async Task<int> SaveCanteenCouponPurpose(CanteenCouponPurpose_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CanteenId", parameters.CanteenId);
            queryParameters.Add("@CouponCode", parameters.CouponCode);
            queryParameters.Add("@CouponPurpose", parameters.CouponPurpose);
            queryParameters.Add("@ValidFrom", parameters.ValidFrom);
            queryParameters.Add("@ValidTo", parameters.ValidTo);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCanteenCouponPurpose", queryParameters);
        }

        public async Task<IEnumerable<CanteenCouponPurpose_Response>> GetCanteenCouponPurposeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CanteenCouponPurpose_Response>("GetCanteenCouponPurposeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<CanteenCouponPurpose_Response?> GetCanteenCouponPurposeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<CanteenCouponPurpose_Response>("GetCanteenCouponPurposeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Contract Type
        public async Task<int> SaveContractType(ContractType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ContractType", parameters.ContractType.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveContractType", queryParameters);
        }

        public async Task<IEnumerable<ContractType_Response>> GetContractTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ContractType_Response>("GetContractTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<ContractType_Response?> GetContractTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<ContractType_Response>("GetContractTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Discipline
        public async Task<int> SaveDiscipline(Discipline_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@Discipline", parameters.Discipline.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveDiscipline", queryParameters);
        }

        public async Task<IEnumerable<Discipline_Response>> GetDisciplineList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Discipline_Response>("GetDisciplineList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Discipline_Response?> GetDisciplineById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Discipline_Response>("GetDisciplineById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Leave Type
        public async Task<int> SaveLeaveType(LeaveType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@LeaveType", parameters.LeaveType.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveLeaveType", queryParameters);
        }

        public async Task<IEnumerable<LeaveType_Response>> GetLeaveTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<LeaveType_Response>("GetLeaveTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<LeaveType_Response?> GetLeaveTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<LeaveType_Response>("GetLeaveTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Rooster Group
        public async Task<int> SaveRoosterGroup(RoosterGroup_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RoosterGroup", parameters.RoosterGroup.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveRoosterGroup", queryParameters);
        }

        public async Task<IEnumerable<RoosterGroup_Response>> GetRoosterGroupList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<RoosterGroup_Response>("GetRoosterGroupList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<RoosterGroup_Response?> GetRoosterGroupById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<RoosterGroup_Response>("GetRoosterGroupById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Food Delivery Location
        public async Task<int> SaveFoodDeliveryLocation(FoodDeliveryLocation_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@FoodDeliveryLocation", parameters.FoodDeliveryLocation);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFoodDeliveryLocation", queryParameters);
        }

        public async Task<IEnumerable<FoodDeliveryLocation_Response>> GetFoodDeliveryLocationList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoodDeliveryLocation_Response>("GetFoodDeliveryLocationList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<FoodDeliveryLocation_Response?> GetFoodDeliveryLocationById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<FoodDeliveryLocation_Response>("GetFoodDeliveryLocationById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Gate Type
        public async Task<int> SaveGateType(GateType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@GateType", parameters.GateType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveGateType", queryParameters);
        }

        public async Task<IEnumerable<GateType_Response>> GetGateTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GateType_Response>("GetGateTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<GateType_Response?> GetGateTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<GateType_Response>("GetGateTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Gate Name
        public async Task<int> SaveGateName(GateName_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@GateName", parameters.GateName);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveGateName", queryParameters);
        }

        public async Task<IEnumerable<GateName_Response>> GetGateNameList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GateName_Response>("GetGateNameList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<GateName_Response?> GetGateNameById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<GateName_Response>("GetGateNameById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Gate Details
        public async Task<int> SaveGateDetails(GateDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@GateNumber", parameters.GateNumber);
            queryParameters.Add("@GateNameId", parameters.GateNameId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveGateDetails", queryParameters);
        }

        public async Task<IEnumerable<GateDetails_Response>> GetGateDetailsList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<GateDetails_Response>("GetGateDetailsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<GateDetails_Response?> GetGateDetailsById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<GateDetails_Response>("GetGateDetailsById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Worker Type
        public async Task<int> SaveWorkerType(WorkerType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@WorkerType", parameters.WorkerType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkerType", queryParameters);
        }

        public async Task<IEnumerable<WorkerType_Response>> GetWorkerTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkerType_Response>("GetWorkerTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<WorkerType_Response?> GetWorkerTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<WorkerType_Response>("GetWorkerTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Worker Status
        public async Task<int> SaveWorkerStatus(WorkerStatus_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@WorkerStatus", parameters.WorkerStatus);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkerStatus", queryParameters);
        }

        public async Task<IEnumerable<WorkerStatus_Response>> GetWorkerStatusList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkerStatus_Response>("GetWorkerStatusList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<WorkerStatus_Response?> GetWorkerStatusById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<WorkerStatus_Response>("GetWorkerStatusById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Meeting Type
        public async Task<int> SaveMeetingType(MeetingType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MeetingType", parameters.MeetingType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMeetingType", queryParameters);
        }

        public async Task<IEnumerable<MeetingType_Response>> GetMeetingTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MeetingType_Response>("GetMeetingTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MeetingType_Response?> GetMeetingTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MeetingType_Response>("GetMeetingTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Qualification
        public async Task<int> SaveQualification(Qualification_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@Qualification", parameters.Qualification);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveQualification", queryParameters);
        }

        public async Task<IEnumerable<Qualification_Response>> GetQualificationList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Qualification_Response>("GetQualificationList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Qualification_Response?> GetQualificationById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Qualification_Response>("GetQualificationById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Item Details
        public async Task<int> SaveItemDetails(ItemDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ItemCode", parameters.ItemCode);
            queryParameters.Add("@ItemName", parameters.ItemName);
            queryParameters.Add("@ItemDesc", parameters.ItemDesc);
            queryParameters.Add("@UOMId", parameters.UOMId);
            queryParameters.Add("@ItemRate", parameters.ItemRate);
            queryParameters.Add("@Serial", parameters.Serial);
            queryParameters.Add("@Batch", parameters.Batch);
            queryParameters.Add("@CompanyId", parameters.CompanyId);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveItemDetails", queryParameters);
        }

        public async Task<IEnumerable<ItemDetails_Response>> GetItemDetailsList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ItemDetails_Response>("GetItemDetailsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<ItemDetails_Response?> GetItemDetailsById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<ItemDetails_Response>("GetItemDetailsById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region ID Type
        public async Task<int> SaveIDType(IDType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@IDType", parameters.IDType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveIDType", queryParameters);
        }

        public async Task<IEnumerable<IDType_Response>> GetIDTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<IDType_Response>("GetIDTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<IDType_Response?> GetIDTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<IDType_Response>("GetIDTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Contractor Type
        public async Task<int> SaveContractorType(ContractorType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ContractorType", parameters.ContractorType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveContractorType", queryParameters);
        }

        public async Task<IEnumerable<ContractorType_Response>> GetContractorTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ContractorType_Response>("GetContractorTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<ContractorType_Response?> GetContractorTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<ContractorType_Response>("GetContractorTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Template Type
        public async Task<int> SaveTemplateType(TemplateType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@TemplateType", parameters.TemplateType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveTemplateType", queryParameters);
        }

        public async Task<IEnumerable<TemplateType_Response>> GetTemplateTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<TemplateType_Response>("GetTemplateTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<TemplateType_Response?> GetTemplateTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<TemplateType_Response>("GetTemplateTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Template Text
        public async Task<int> SaveTemplateText(TemplateText_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@TemplateTypeId", parameters.TemplateTypeId);
            queryParameters.Add("@TemplateText", parameters.TemplateText);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveTemplateText", queryParameters);
        }

        public async Task<IEnumerable<TemplateText_Response>> GetTemplateTextList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<TemplateText_Response>("GetTemplateTextList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<TemplateText_Response?> GetTemplateTextById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<TemplateText_Response>("GetTemplateTextById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Canteen Feedback
        public async Task<int> SaveCanteenFeedback(CanteenFeedback_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CanteenId", parameters.CanteenId);
            queryParameters.Add("@CanteenFeedback", parameters.CanteenFeedback);
            queryParameters.Add("@Rating", parameters.Rating);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCanteenFeedback", queryParameters);
        }

        public async Task<IEnumerable<CanteenFeedback_Response>> GetCanteenFeedbackList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CanteenFeedback_Response>("GetCanteenFeedbackList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<CanteenFeedback_Response?> GetCanteenFeedbackById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<CanteenFeedback_Response>("GetCanteenFeedbackById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Attendance
        public async Task<int> SaveAttendance(AttendanceDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DayName", parameters.DayName);
            queryParameters.Add("@ColorSelection", parameters.ColorSelection);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveAttendance", queryParameters);
        }

        public async Task<IEnumerable<Attendance_Response>> GetAttendanceList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Attendance_Response>("GetAttendanceList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Attendance_Response?> GetAttendanceById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Attendance_Response>("GetAttendanceById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Marital Status
        public async Task<int> SaveMaritalStatus(MaritalStatus_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MaritalStatus", parameters.MaritalStatus);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMaritalStatus", queryParameters);
        }

        public async Task<IEnumerable<MaritalStatus_Response>> GetMaritalStatusList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MaritalStatus_Response>("GetMaritalStatusList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MaritalStatus_Response?> GetMaritalStatusById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MaritalStatus_Response>("GetMaritalStatusById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Blood Group
        public async Task<int> SaveBloodGroup(BloodGroup_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@BloodGroup", parameters.BloodGroup);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveBloodGroup", queryParameters);
        }

        public async Task<IEnumerable<BloodGroup_Response>> GetBloodGroupList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<BloodGroup_Response>("GetBloodGroupList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<BloodGroup_Response?> GetBloodGroupById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<BloodGroup_Response>("GetBloodGroupById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Work Shift
        public async Task<int> SaveWorkShift(WorkShift_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ShiftName", parameters.ShiftName);
            queryParameters.Add("@ShiftIn", parameters.ShiftIn);
            queryParameters.Add("@ShiftOut", parameters.ShiftOut);
            queryParameters.Add("@TotalHours", parameters.TotalHours);
            queryParameters.Add("@ShiftOverLay", parameters.ShiftOverLay);
            queryParameters.Add("@CompanyId", parameters.CompanyId);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkShift", queryParameters);
        }

        public async Task<IEnumerable<WorkShift_Response>> GetWorkShiftList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkShift_Response>("GetWorkShiftList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<WorkShift_Response?> GetWorkShiftById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<WorkShift_Response>("GetWorkShiftById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveWorkShiftDays(WorkShiftDays_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@WorkShiftId", parameters.WorkShiftId);
            queryParameters.Add("@DaysId", parameters.DaysId);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkShiftDays", queryParameters);
        }

        public async Task<IEnumerable<WorkShiftDays_Response>> GetWorkShiftDaysList(WorkShiftDays_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@WorkShiftId", parameters.WorkShiftId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkShiftDays_Response>("GetWorkShiftDaysList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion

        #region Pass Type
        public async Task<int> SavePassType(PassType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@PassType", parameters.PassType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SavePassType", queryParameters);
        }

        public async Task<IEnumerable<PassType_Response>> GetPassTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<PassType_Response>("GetPassTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<PassType_Response?> GetPassTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<PassType_Response>("GetPassTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Item Type
        public async Task<int> SaveItemType(ItemType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@ItemType", parameters.ItemType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveItemType", queryParameters);
        }

        public async Task<IEnumerable<ItemType_Response>> GetItemTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ItemType_Response>("GetItemTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<ItemType_Response?> GetItemTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<ItemType_Response>("GetItemTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Days
        public async Task<int> SaveDays(Days_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@DaysName", parameters.DaysName);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveDays", queryParameters);
        }

        public async Task<IEnumerable<Days_Response>> GetDaysList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Days_Response>("GetDaysList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Days_Response?> GetDaysById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Days_Response>("GetDaysById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Meal Type
        public async Task<int> SaveMealType(MealType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MealType", parameters.MealType);
            queryParameters.Add("@StartTime", parameters.StartTime);
            queryParameters.Add("@EndTime", parameters.EndTime);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMealType", queryParameters);
        }

        public async Task<IEnumerable<MealType_Response>> GetMealTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MealType_Response>("GetMealTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MealType_Response?> GetMealTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MealType_Response>("GetMealTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Food Item
        public async Task<int> SaveFoodItem(FoodItem_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@CanteenId", parameters.CanteenId);
            queryParameters.Add("@MenuItemId", parameters.MenuItemId);
            queryParameters.Add("@FoodItemDesc", parameters.FoodItemDesc);
            queryParameters.Add("@IsVeg", parameters.IsVeg);
            queryParameters.Add("@IsSubsidized", parameters.IsSubsidized);
            queryParameters.Add("@SellingPrice", parameters.SellingPrice);
            queryParameters.Add("@SubsidizedPrice", parameters.SubsidizedPrice);
            queryParameters.Add("@IsDiscounted", parameters.IsDiscounted);
            queryParameters.Add("@DiscountPrice", parameters.DiscountPrice);
            queryParameters.Add("@IsEmployee", parameters.IsEmployee);
            queryParameters.Add("@FoodItemOriginalFileName", parameters.FoodItemOriginalFileName);
            queryParameters.Add("@FoodItemImage", parameters.FoodItemImage);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFoodItem", queryParameters);
        }

        public async Task<IEnumerable<FoodItem_Response>> GetFoodItemList(FoodItem_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CanteenId", parameters.CanteenId);
            queryParameters.Add("@IsVeg", parameters.IsVeg);
            queryParameters.Add("@IsSubsidized", parameters.IsSubsidized);
            queryParameters.Add("@IsEmployee", parameters.IsEmployee);
            queryParameters.Add("@MealTypeId", parameters.MealTypeId);
            queryParameters.Add("@DaysId", parameters.DaysId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoodItem_Response>("GetFoodItemList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<FoodItem_Response?> GetFoodItemById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<FoodItem_Response>("GetFoodItemById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveFoodItemDays(FoodItemDays_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@FoodItemId", parameters.FoodItemId);
            queryParameters.Add("@DaysId", parameters.DaysId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFoodItemDays", queryParameters);
        }

        public async Task<IEnumerable<FoodItemDays_Response>> GetFoodItemDaysList(FoodItemDays_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FoodItemId", parameters.FoodItemId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoodItemDays_Response>("GetFoodItemDaysList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<int> SaveFoodItemMealType(FoodItemMealType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Action", parameters.Action);
            queryParameters.Add("@FoodItemId", parameters.FoodItemId);
            queryParameters.Add("@MealTypeId", parameters.MealTypeId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveFoodItemMealType", queryParameters);
        }

        public async Task<IEnumerable<FoodItemMealType_Response>> GetFoodItemMealTypeList(FoodItemDays_Search_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FoodItemId", parameters.FoodItemId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoodItemMealType_Response>("GetFoodItemMealTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        #endregion

        #region MenuItem
        public async Task<int> SaveMenuItem(MenuItem_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MenuItemName", parameters.MenuItemName.SanitizeValue());
            queryParameters.Add("@IsVeg", parameters.IsVeg);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMenuItem", queryParameters);
        }

        public async Task<IEnumerable<MenuItem_Response>> GetMenuItemList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MenuItem_Response>("GetMenuItemList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MenuItem_Response?> GetMenuItemById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MenuItem_Response>("GetMenuItemById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Canteen Item Details
        public async Task<int> SaveCanteenItemDetails(CanteenItemDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RawMaterial", parameters.RawMaterial.SanitizeValue());
            queryParameters.Add("@UOMId", parameters.UOMId);
            queryParameters.Add("@MinQty", parameters.MinQty);
            queryParameters.Add("@AvailableQty", parameters.AvailableQty);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveCanteenItemDetails", queryParameters);
        }

        public async Task<IEnumerable<CanteenItemDetails_Response>> GetCanteenItemDetailsList(CanteenItemDetails_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<CanteenItemDetails_Response>("GetCanteenItemDetailsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<CanteenItemDetails_Response?> GetCanteenItemDetailsById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<CanteenItemDetails_Response>("GetCanteenItemDetailsById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Material Details
        public async Task<int> SaveMaterialDetails(MaterialDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@MaterialCode", parameters.MaterialCode.SanitizeValue());
            queryParameters.Add("@MaterialName", parameters.MaterialName.SanitizeValue());
            queryParameters.Add("@CurrentStock", parameters.CurrentStock);
            queryParameters.Add("@Quantity", parameters.Quantity);
            queryParameters.Add("@UOMId", parameters.UOMId);
            queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveMaterialDetails", queryParameters);
        }

        public async Task<IEnumerable<MaterialDetails_Response>> GetMaterialDetailsList(MaterialDetails_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<MaterialDetails_Response>("GetMaterialDetailsList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<MaterialDetails_Response?> GetMaterialDetailsById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<MaterialDetails_Response>("GetMaterialDetailsById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region User Type
        public async Task<int> SaveUserType(UserType_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@UserType", parameters.UserType);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveUserType", queryParameters);
        }

        public async Task<IEnumerable<UserType_Response>> GetUserTypeList(BaseSearchEntity parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<UserType_Response>("GetUserTypeList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<UserType_Response?> GetUserTypeById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<UserType_Response>("GetUserTypeById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Work Place
        public async Task<int> SaveWorkPlace(WorkPlace_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@WorkPlace", parameters.WorkPlace);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveWorkPlace", queryParameters);
        }

        public async Task<IEnumerable<WorkPlace_Response>> GetWorkPlaceList(WorkPlace_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<WorkPlace_Response>("GetWorkPlaceList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<WorkPlace_Response?> GetWorkPlaceById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<WorkPlace_Response>("GetWorkPlaceById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region Grocery
        public async Task<int> SaveGrocery(Grocery_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@GroceryName", parameters.GroceryName.SanitizeValue());
            queryParameters.Add("@GroceryDesc", parameters.GroceryDesc);
            queryParameters.Add("@UOMId", parameters.UOMId);
            queryParameters.Add("@MinQty", parameters.MinQty);
            queryParameters.Add("@AvailableQty", parameters.AvailableQty);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveGrocery", queryParameters);
        }

        public async Task<IEnumerable<Grocery_Response>> GetGroceryList(Grocery_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<Grocery_Response>("GetGroceryList", queryParameters);
            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<Grocery_Response?> GetGroceryById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<Grocery_Response>("GetGroceryById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<Grocery_ImportDataValidation>> ImportGrocery(List<Grocery_ImportData> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlData", xmlData);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await ListByStoredProcedure<Grocery_ImportDataValidation>("ImportGrocery", queryParameters);
        }

        #endregion
    }
}
