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
    public class ManageFoodWastageController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageFoodWastageRepository _manageFoodWastageRepository;
        private readonly IFileManager _fileManager;

        public ManageFoodWastageController(IManageFoodWastageRepository manageFoodWastageRepository, IFileManager fileManager)
        {
            _manageFoodWastageRepository = manageFoodWastageRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveFoodWastage(FoodWastage_Request parameters)
        {
            int result = await _manageFoodWastageRepository.SaveFoodWastage(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record already exists";
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
        public async Task<ResponseModel> GetFoodWastageList(FoodWastage_Search parameters)
        {
            IEnumerable<FoodWastage_Response> lstRoles = await _manageFoodWastageRepository.GetFoodWastageList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodWastageById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageFoodWastageRepository.GetFoodWastageById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
