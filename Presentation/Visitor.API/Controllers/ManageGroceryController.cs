using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Enums;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageGroceryController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageGroceryRepository _manageGroceryRepository;

        public ManageGroceryController(IManageGroceryRepository manageGroceryRepository)
        {
            _manageGroceryRepository = manageGroceryRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Grocery Requisition
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGroceryRequisition(GroceryRequisition_Request parameters)
        {
            int result = await _manageGroceryRepository.SaveGroceryRequisition(parameters);

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
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
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

                #region Grocery Requisition Details
                if (result > 0)
                {
                    foreach (var items in parameters.GroceryRequisitionDetails)
                    {
                        var vGroceryRequisitionDetails = new GroceryRequisitionDetails_Request()
                        {
                            Id = items.Id,
                            GroceryRequisitionId = result,
                            GroceryId = items.GroceryId,
                            OrderQty = items.OrderQty,
                            ReceivedQty = 0,
                            IsOK = 0,
                        };

                        int resultUserOtherDetails = await _manageGroceryRepository.SaveGroceryRequisitionDetails(vGroceryRequisitionDetails);
                    }
                }
                #endregion
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryRequisitionList(GroceryRequisition_Search parameters)
        {
            IEnumerable<GroceryRequisitionList_Response> lstRoles = await _manageGroceryRepository.GetGroceryRequisitionList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryRequisitionById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageGroceryRepository.GetGroceryRequisitionById(Id);
                if (vResultObj != null)
                {
                    var vGroceryRequisitionDetails = new GroceryRequisitionDetails_Search()
                    {
                        GroceryRequisitionId = vResultObj.Id,
                        IsOk = 0,
                    };

                    var vGroceryRequisitionDetailsList = await _manageGroceryRepository.GetGroceryRequisitionDetailsList(vGroceryRequisitionDetails);
                    vResultObj.GroceryRequisitionDetails = vGroceryRequisitionDetailsList.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryRequisition_ApproveNRejectHistoryListById(GroceryRequisition_ApproveNRejectHistory_Search parameters)
        {
            IEnumerable<GroceryRequisition_ApproveNRejectHistory_Response> lst = await _manageGroceryRepository.GetGroceryRequisition_ApproveNRejectHistoryListById(parameters);
            _response.Data = lst.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
        #endregion

        #region Grocery Inwarding
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGroceryRequisitionDetails(GroceryRequisitionDetails_Request parameters)
        {
            int result = await _manageGroceryRepository.SaveGroceryRequisitionDetails(parameters);

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
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
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
        public async Task<ResponseModel> GetGroceryRequisitionDetailsList(GroceryRequisitionDetails_Search parameters)
        {
            IEnumerable<GroceryRequisitionDetails_Response> lstRoles = await _manageGroceryRepository.GetGroceryRequisitionDetailsList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
        #endregion

        #region Grocery Outwarding
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGroceryOutwarding(GroceryOutwarding_Request parameters)
        {
            int result = await _manageGroceryRepository.SaveGroceryOutwarding(parameters);

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
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
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
        public async Task<ResponseModel> GetGroceryOutwardingList(GroceryOutwarding_Search parameters)
        {
            IEnumerable<GroceryOutwarding_Response> lstRoles = await _manageGroceryRepository.GetGroceryOutwardingList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        #endregion
    }
}
