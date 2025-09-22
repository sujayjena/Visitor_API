using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.API.CustomAttributes;
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
        private readonly IFileManager _fileManager;

        public AdminMasterController(IAdminMasterRepository adminMasterRepository, IFileManager fileManager)
        {
            _adminMasterRepository = adminMasterRepository;
            _fileManager = fileManager;

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
                if(parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }

        [AllowAnonymous]
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
                _response.Message = "Record details saved successfully";
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }

        [AllowAnonymous]
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }

        [AllowAnonymous]
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                _response.Message = "Record details saved successfully";
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
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

        #region Worker Status

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorkerStatus(WorkerStatus_Request parameters)
        {
            int result = await _adminMasterRepository.SaveWorkerStatus(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerStatusList(BaseSearchEntity parameters)
        {
            IEnumerable<WorkerStatus_Response> lstRoles = await _adminMasterRepository.GetWorkerStatusList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerStatusById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetWorkerStatusById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Meeting Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMeetingType(MeetingType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMeetingType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMeetingTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<MeetingType_Response> lstRoles = await _adminMasterRepository.GetMeetingTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMeetingTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMeetingTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Qualification

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveQualification(Qualification_Request parameters)
        {
            int result = await _adminMasterRepository.SaveQualification(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetQualificationList(BaseSearchEntity parameters)
        {
            IEnumerable<Qualification_Response> lstRoles = await _adminMasterRepository.GetQualificationList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetQualificationById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetQualificationById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Item Details

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveItemDetails(ItemDetails_Request parameters)
        {
            int result = await _adminMasterRepository.SaveItemDetails(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetItemDetailsList(BaseSearchEntity parameters)
        {
            IEnumerable<ItemDetails_Response> lstRoles = await _adminMasterRepository.GetItemDetailsList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetItemDetailsById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetItemDetailsById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region ID Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveIDType(IDType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveIDType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> GetIDTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<IDType_Response> lstRoles = await _adminMasterRepository.GetIDTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetIDTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetIDTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Contractor Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveContractorType(ContractorType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveContractorType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContractorTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<ContractorType_Response> lstRoles = await _adminMasterRepository.GetContractorTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContractorTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetContractorTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Template Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveTemplateType(TemplateType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveTemplateType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTemplateTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<TemplateType_Response> lstRoles = await _adminMasterRepository.GetTemplateTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTemplateTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetTemplateTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Template Text

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveTemplateText(TemplateText_Request parameters)
        {
            int result = await _adminMasterRepository.SaveTemplateText(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTemplateTextList(BaseSearchEntity parameters)
        {
            IEnumerable<TemplateText_Response> lstRoles = await _adminMasterRepository.GetTemplateTextList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTemplateTextById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetTemplateTextById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Canteen Feedback

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCanteenFeedback(CanteenFeedback_Request parameters)
        {
            int result = await _adminMasterRepository.SaveCanteenFeedback(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenFeedbackList(BaseSearchEntity parameters)
        {
            IEnumerable<CanteenFeedback_Response> lstRoles = await _adminMasterRepository.GetCanteenFeedbackList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenFeedbackById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetCanteenFeedbackById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Attendance

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveAttendance(Attendance_Request parameters)
        {
            int result = 0;
            foreach (var item in parameters.attendanceDetails)
            {
                var vAttendanceDetails_Request = new AttendanceDetails_Request()
                {
                    Id = item.Id,
                    DayName = item.DayName,
                    ColorSelection = item.ColorSelection,
                    IsActive = item.IsActive,
                };

                result = await _adminMasterRepository.SaveAttendance(vAttendanceDetails_Request);
            }

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
                _response.Message = "Record details saved successfully";
            }

            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetAttendanceList(BaseSearchEntity parameters)
        {
            IEnumerable<Attendance_Response> lstRoles = await _adminMasterRepository.GetAttendanceList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetAttendanceById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetAttendanceById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Marital Status

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMaritalStatus(MaritalStatus_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMaritalStatus(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaritalStatusList(BaseSearchEntity parameters)
        {
            IEnumerable<MaritalStatus_Response> lstRoles = await _adminMasterRepository.GetMaritalStatusList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaritalStatusById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMaritalStatusById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Blood Group

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBloodGroup(BloodGroup_Request parameters)
        {
            int result = await _adminMasterRepository.SaveBloodGroup(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBloodGroupList(BaseSearchEntity parameters)
        {
            IEnumerable<BloodGroup_Response> lstRoles = await _adminMasterRepository.GetBloodGroupList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBloodGroupById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetBloodGroupById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Work Shift

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorkShift(WorkShift_Request parameters)
        {
            int result = await _adminMasterRepository.SaveWorkShift(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }

                // Delete Old days
                var vDaysDELETEObj = new WorkShiftDays_Request()
                {
                    Action = "DELETE",
                    WorkShiftId = result,
                    DaysId = 0,
                };
                int resultDaysDELETE = await _adminMasterRepository.SaveWorkShiftDays(vDaysDELETEObj);


                // Add new days
                foreach (var items in parameters.daysList)
                {
                    var vWorkShiftDays = new WorkShiftDays_Request()
                    {
                        Action = "INSERT",
                        WorkShiftId = result,
                        DaysId = items.DaysId,
                    };

                    int resultMealTypeDays = await _adminMasterRepository.SaveWorkShiftDays(vWorkShiftDays);
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkShiftList(BaseSearchEntity parameters)
        {
            IEnumerable<WorkShift_Response> lstRoles = await _adminMasterRepository.GetWorkShiftList(parameters);
            foreach(var item in lstRoles)
            {
                var vSearchRequest = new WorkShiftDays_Search_Request();
                vSearchRequest.WorkShiftId = item.Id;

                var vDayList = await _adminMasterRepository.GetWorkShiftDaysList(vSearchRequest);
                item.daysList = vDayList.ToList();
            }

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkShiftById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetWorkShiftById(Id);
                if(vResultObj != null)
                {
                    var vSearchRequest = new WorkShiftDays_Search_Request();
                    vSearchRequest.WorkShiftId = vResultObj.Id;

                    var vDayList = await _adminMasterRepository.GetWorkShiftDaysList(vSearchRequest);
                    vResultObj.daysList = vDayList.ToList();
                }

                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Pass Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePassType(PassType_Request parameters)
        {
            int result = await _adminMasterRepository.SavePassType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPassTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<PassType_Response> lstRoles = await _adminMasterRepository.GetPassTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPassTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetPassTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Item Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveItemType(ItemType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveItemType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetItemTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<ItemType_Response> lstRoles = await _adminMasterRepository.GetItemTypeList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetItemTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetItemTypeById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Days

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDays(Days_Request parameters)
        {
            int result = await _adminMasterRepository.SaveDays(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDaysList(BaseSearchEntity parameters)
        {
            IEnumerable<Days_Response> lstRoles = await _adminMasterRepository.GetDaysList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDaysById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetDaysById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Meal Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMealType(MealType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMealType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMealTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<MealType_Response> lstRoles = await _adminMasterRepository.GetMealTypeList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMealTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMealTypeById(Id);
               
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Food Item

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveFoodItem(FoodItem_Request parameters)
        {
            // image Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.FoodItemImage_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.FoodItemImage_Base64, "\\Uploads\\FoodItem\\", parameters.FoodItemOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.FoodItemImage = vUploadFile;
                }
            }

            int result = await _adminMasterRepository.SaveFoodItem(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }

                // Delete Old Meal Type
                var vMealTypeDELETEObj = new FoodItemMealType_Request()
                {
                    Action = "DELETE",
                    FoodItemId = result,
                    MealTypeId = 0,
                    IsActive = null,
                };
                int resultMealTypeDELETE = await _adminMasterRepository.SaveFoodItemMealType(vMealTypeDELETEObj);

                // Add new days
                foreach (var items in parameters.mealTypeList)
                {
                    var vFoodItemMealType = new FoodItemMealType_Request()
                    {
                        Action = "INSERT",
                        FoodItemId = result,
                        MealTypeId = items.MealTypeId,
                        IsActive = items.IsActive,
                    };

                    int resultFoodItemMealType = await _adminMasterRepository.SaveFoodItemMealType(vFoodItemMealType);
                }

                // Delete Old days
                var vDaysDELETEObj = new FoodItemDays_Request()
                {
                    Action = "DELETE",
                    FoodItemId = result,
                    DaysId = 0,
                    IsActive = null,
                };
                int resultDaysDELETE = await _adminMasterRepository.SaveFoodItemDays(vDaysDELETEObj);

                // Add new days
                foreach (var items in parameters.daysList)
                {
                    var vFoodItemDays = new FoodItemDays_Request()
                    {
                        Action = "INSERT",
                        FoodItemId = result,
                        DaysId = items.DaysId,
                        IsActive = items.IsActive,
                    };

                    int resultFoodItemDays = await _adminMasterRepository.SaveFoodItemDays(vFoodItemDays);
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodItemList(FoodItem_Search_Request parameters)
        {
            IEnumerable<FoodItem_Response> lstRoles = await _adminMasterRepository.GetFoodItemList(parameters);
            foreach (var item in lstRoles)
            {
                var vSearchRequest = new FoodItemDays_Search_Request();
                vSearchRequest.FoodItemId = item.Id;

                var vDayList = await _adminMasterRepository.GetFoodItemDaysList(vSearchRequest);
                item.daysList = vDayList.ToList();

                var vMealTypeList = await _adminMasterRepository.GetFoodItemMealTypeList(vSearchRequest);
                item.mealTypeList = vMealTypeList.ToList();
            }

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodItemById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetFoodItemById(Id);
                if (vResultObj != null)
                {
                    var vSearchRequest = new FoodItemDays_Search_Request();
                    vSearchRequest.FoodItemId = vResultObj.Id;

                    var vDayList = await _adminMasterRepository.GetFoodItemDaysList(vSearchRequest);
                    vResultObj.daysList = vDayList.ToList();

                    var vMealTypeList = await _adminMasterRepository.GetFoodItemMealTypeList(vSearchRequest);
                    vResultObj.mealTypeList = vMealTypeList.ToList();
                }

                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region MenuItem

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMenuItem(MenuItem_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMenuItem(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMenuItemList(BaseSearchEntity parameters)
        {
            IEnumerable<MenuItem_Response> lstRoles = await _adminMasterRepository.GetMenuItemList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMenuItemById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMenuItemById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Canteen Item Details

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCanteenItemDetails(CanteenItemDetails_Request parameters)
        {
            int result = await _adminMasterRepository.SaveCanteenItemDetails(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenItemDetailsList(CanteenItemDetails_Search parameters)
        {
            IEnumerable<CanteenItemDetails_Response> lstRoles = await _adminMasterRepository.GetCanteenItemDetailsList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenItemDetailsById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetCanteenItemDetailsById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Material Details

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMaterialDetails(MaterialDetails_Request parameters)
        {
            int result = await _adminMasterRepository.SaveMaterialDetails(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaterialDetailsList(MaterialDetails_Search parameters)
        {
            IEnumerable<MaterialDetails_Response> lstRoles = await _adminMasterRepository.GetMaterialDetailsList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaterialDetailsById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetMaterialDetailsById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region User Type

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveUserType(UserType_Request parameters)
        {
            int result = await _adminMasterRepository.SaveUserType(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserTypeList(BaseSearchEntity parameters)
        {
            IEnumerable<UserType_Response> lstRoles = await _adminMasterRepository.GetUserTypeList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserTypeById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetUserTypeById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Work Place

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorkPlace(WorkPlace_Request parameters)
        {
            int result = await _adminMasterRepository.SaveWorkPlace(parameters);

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
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkPlaceList(WorkPlace_Search parameters)
        {
            IEnumerable<WorkPlace_Response> lstRoles = await _adminMasterRepository.GetWorkPlaceList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkPlaceById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _adminMasterRepository.GetWorkPlaceById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}
