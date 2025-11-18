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
                            OrderQty = items.OrderQty
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
                        GroceryRequisitionId = vResultObj.Id
                    };

                    //var vGroceryRequisitionDetailsList = await _manageGroceryRepository.GetGroceryRequisitionDetailsList(vGroceryRequisitionDetails);
                    //vResultObj.GroceryRequisitionDetails = vGroceryRequisitionDetailsList.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }
       
        #endregion
    }
}
