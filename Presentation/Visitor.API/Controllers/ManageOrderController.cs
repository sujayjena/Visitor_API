using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageOrderController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageOrderRepository _manageOrderRepository;
        private IFileManager _fileManager;

        public ManageOrderController(IManageOrderRepository manageOrderRepository, IFileManager fileManager)
        {
            _manageOrderRepository = manageOrderRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveFoodOrder(FoodOrder_Request parameters)
        {
            int result = await _manageOrderRepository.SaveFoodOrder(parameters);

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

                foreach (var items in parameters.foodItemList)
                {
                    var vFoodOrderItem = new FoodOrderItem_Request()
                    {
                        Id = items.Id,
                        FoodOrderId = result,
                        FoodItemId = items.FoodItemId,
                        Price = items.Price,
                        Quantity = items.Quantity,
                        IsActive = items.IsActive,
                    };

                    int resultFoodOrderItem = await _manageOrderRepository.SaveFoodOrderItem(vFoodOrderItem);
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodOrderList(FoodOrderSearch_Request parameters)
        {
            IEnumerable<FoodOrder_Response> lstRoles = await _manageOrderRepository.GetFoodOrderList(parameters);
            foreach (var item in lstRoles)
            {
                var vSearchRequest = new FoodOrderItemSearch_Request();
                vSearchRequest.FoodOrderId = item.Id;

                var vFoodItemList = await _manageOrderRepository.GetFoodOrderItemList(vSearchRequest);
                item.foodItemList = vFoodItemList.ToList();
            }

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodOrderById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageOrderRepository.GetFoodOrderById(Id);
                if (vResultObj != null)
                {
                    var vSearchRequest = new FoodOrderItemSearch_Request();
                    vSearchRequest.FoodOrderId = vResultObj.Id;

                    var vFoodItemList = await _manageOrderRepository.GetFoodOrderItemList(vSearchRequest);
                    vResultObj.foodItemList = vFoodItemList.ToList();
                }

                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
