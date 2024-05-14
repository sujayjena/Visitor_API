using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMasterController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IAdminMasterRepository _adminMasterRepository;

        public AdminMasterController(IAdminMasterRepository adminMasterRepository)
        {
            _adminMasterRepository = adminMasterRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Gender

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGender(Gender_Request parameters)
        {
            int result = await _adminMasterRepository.SaveGender(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGenderList(BaseSearchEntity parameters)
        {
            IEnumerable<Gender_Response> lstRoles = await _adminMasterRepository.GetGenderList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGenderById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetGenderById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Visitor Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVisitorType(VisitorType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveVisitorType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<VisitorType_Response> lstRoles = await _adminMasterRepository.GetVisitorTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetVisitorTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Visit Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVisitType(VisitType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveVisitType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<VisitType_Response> lstRoles = await _adminMasterRepository.GetVisitTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetVisitTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Vehicle Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVehicleType(VehicleType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveVehicleType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVehicleTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<VehicleType_Response> lstRoles = await _adminMasterRepository.GetVehicleTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVehicleTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetVehicleTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Material With Visitor

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMaterialWithVisitor(MaterialWithVisitor_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMaterialWithVisitor(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaterialWithVisitorList(BaseSearchEntity parameters)
        {
            IEnumerable<MaterialWithVisitor_Response> lstRoles = await _adminMasterRepository.GetMaterialWithVisitorList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaterialWithVisitorById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMaterialWithVisitorById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Meeting Location

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMeetingLocation(MeetingLocation_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMeetingLocation(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMeetingLocationList(BaseSearchEntity parameters)
        {
            IEnumerable<MeetingLocation_Response> lstRoles = await _adminMasterRepository.GetMeetingLocationList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMeetingLocationById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMeetingLocationById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Document Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDocumentType(DocumentType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveDocumentType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDocumentTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<DocumentType_Response> lstRoles = await _adminMasterRepository.GetDocumentTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDocumentTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetDocumentTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Rejection Reason

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRejectionReason(RejectionReason_Request parameters)
        {
            int result = await _adminMasterRepository.SaveRejectionReason(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRejectionReasonList(BaseSearchEntity parameters)
        {
            IEnumerable<RejectionReason_Response> lstRoles = await _adminMasterRepository.GetRejectionReasonList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRejectionReasonById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetRejectionReasonById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region UOM

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveUOM(UOM_Request parameters)
        {
            int result = await _adminMasterRepository.SaveUOM(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUOMList(BaseSearchEntity parameters)
        {
            IEnumerable<UOM_Response> lstRoles = await _adminMasterRepository.GetUOMList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUOMById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetUOMById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Canteen Name

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCanteenName(CanteenName_Request parameters)
        {
            int result = await _adminMasterRepository.SaveCanteenName(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenNameList(BaseSearchEntity parameters)
        {
            IEnumerable<CanteenName_Response> lstRoles = await _adminMasterRepository.GetCanteenNameList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenNameById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetCanteenNameById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Canteen Coupon Purpose

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCanteenCouponPurpose(CanteenCouponPurpose_Request parameters)
        {
            int result = await _adminMasterRepository.SaveCanteenCouponPurpose(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenCouponPurposeList(BaseSearchEntity parameters)
        {
            IEnumerable<CanteenCouponPurpose_Response> lstRoles = await _adminMasterRepository.GetCanteenCouponPurposeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenCouponPurposeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetCanteenCouponPurposeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Contract Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveContractType(ContractType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveContractType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContractTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<ContractType_Response> lstRoles = await _adminMasterRepository.GetContractTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContractTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetContractTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Discipline

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDiscipline(Discipline_Request parameters)
        {
            int result = await _adminMasterRepository.SaveDiscipline(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDisciplineList(BaseSearchEntity parameters)
        {
            IEnumerable<Discipline_Response> lstRoles = await _adminMasterRepository.GetDisciplineList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDisciplineById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetDisciplineById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Leave Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveLeaveType(LeaveType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveLeaveType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetLeaveTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<LeaveType_Response> lstRoles = await _adminMasterRepository.GetLeaveTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetLeaveTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetLeaveTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Rooster Group

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRoosterGroup(RoosterGroup_Request parameters)
        {
            int result = await _adminMasterRepository.SaveRoosterGroup(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRoosterGroupList(BaseSearchEntity parameters)
        {
            IEnumerable<RoosterGroup_Response> lstRoles = await _adminMasterRepository.GetRoosterGroupList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRoosterGroupById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetRoosterGroupById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Food Delivery Location

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveFoodDeliveryLocation(FoodDeliveryLocation_Request parameters)
        {
            int result = await _adminMasterRepository.SaveFoodDeliveryLocation(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodDeliveryLocationList(BaseSearchEntity parameters)
        {
            IEnumerable<FoodDeliveryLocation_Response> lstRoles = await _adminMasterRepository.GetFoodDeliveryLocationList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodDeliveryLocationById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetFoodDeliveryLocationById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Gate Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGateType(GateType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveGateType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGateTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<GateType_Response> lstRoles = await _adminMasterRepository.GetGateTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGateTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetGateTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Gate Name

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGateName(GateName_Request parameters)
        {
            int result = await _adminMasterRepository.SaveGateName(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGateNameList(BaseSearchEntity parameters)
        {
            IEnumerable<GateName_Response> lstRoles = await _adminMasterRepository.GetGateNameList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGateNameById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetGateNameById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Gate Details

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGateDetails(GateDetails_Request parameters)
        {
            int result = await _adminMasterRepository.SaveGateDetails(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGateDetailsList(BaseSearchEntity parameters)
        {
            IEnumerable<GateDetails_Response> lstRoles = await _adminMasterRepository.GetGateDetailsList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGateDetailsById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetGateDetailsById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Worker Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorkerType(WorkerType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveWorkerType(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved sucessfully";
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<WorkerType_Response> lstRoles = await _adminMasterRepository.GetWorkerTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetWorkerTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}
