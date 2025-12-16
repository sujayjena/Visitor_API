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
    public class ManageAttendanceController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageAttendanceRepository _manageAttendanceRepository;
        private IFileManager _fileManager;

        public ManageAttendanceController(IManageAttendanceRepository manageAttendanceRepository, IFileManager fileManager)
        {
            _manageAttendanceRepository = manageAttendanceRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveManageAttendance(ManageAttendance_Request parameters)
        {
            int result = await _manageAttendanceRepository.SaveAttendanceDetails(parameters);

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
        public async Task<ResponseModel> GetManageAttendanceList(ManageAttendance_Search parameters)
        {
            IEnumerable<ManageAttendance_Response> lstUsers = await _manageAttendanceRepository.GetAttendanceDetailsList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
    }
}
