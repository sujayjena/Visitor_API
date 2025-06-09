using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageEarlyLeaveController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageEarlyLeaveRepository _manageEarlyLeaveRepository;

        public ManageEarlyLeaveController(IManageEarlyLeaveRepository manageEarlyLeaveRepository)
        {
            _manageEarlyLeaveRepository = manageEarlyLeaveRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveEarlyLeave(EarlyLeave_Request parameters)
        {
            int result = await _manageEarlyLeaveRepository.SaveEarlyLeave(parameters);

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
        public async Task<ResponseModel> GetEarlyLeaveList(EarlyLeave_Search parameters)
        {
            IEnumerable<EarlyLeave_Response> lstRoles = await _manageEarlyLeaveRepository.GetEarlyLeaveList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEarlyLeaveById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageEarlyLeaveRepository.GetEarlyLeaveById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
