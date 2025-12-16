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

        #region Employee
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


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEmployeeEarlyLeave_CheckedInOut_List(EmployeeEarlyLeave_CheckedInOut_Search parameters)
        {
            IEnumerable<EmployeeEarlyLeave_CheckedInOut_Response> lstVisitorss = await _manageEarlyLeaveRepository.GetEmployeeEarlyLeave_CheckedInOut_List(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        #endregion

        #region Worker
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorkerEarlyLeave(WorkerEarlyLeave_Request parameters)
        {
            int result = await _manageEarlyLeaveRepository.SaveWorkerEarlyLeave(parameters);

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
        public async Task<ResponseModel> GetWorkerEarlyLeaveList(WorkerEarlyLeave_Search parameters)
        {
            IEnumerable<WorkerEarlyLeave_Response> lstRoles = await _manageEarlyLeaveRepository.GetWorkerEarlyLeaveList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerEarlyLeaveById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageEarlyLeaveRepository.GetWorkerEarlyLeaveById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerEarlyLeave_CheckedInOut_List(WorkerEarlyLeave_CheckedInOut_Search parameters)
        {
            IEnumerable<WorkerEarlyLeave_CheckedInOut_Response> lstVisitorss = await _manageEarlyLeaveRepository.GetWorkerEarlyLeave_CheckedInOut_List(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
        #endregion

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> EarlyLeaveApproveNReject(EarlyLeave_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                int resultExpenseDetails = await _manageEarlyLeaveRepository.EarlyLeaveApproveNReject(parameters);

                if (resultExpenseDetails == (int)SaveOperationEnums.NoRecordExists)
                {
                    _response.Message = "No record exists";
                }
                else if (resultExpenseDetails == (int)SaveOperationEnums.ReocrdExists)
                {
                    _response.Message = "Record already exists";
                }
                else if (resultExpenseDetails == (int)SaveOperationEnums.NoResult)
                {
                    _response.Message = "Something went wrong, please try again";
                }
                else
                {
                    if (parameters.StatusId == 2)
                    {
                        _response.Message = "Record Approved successfully";
                    }
                    else if (parameters.StatusId == 3)
                    {
                        _response.Message = "Record Rejected successfully";
                    }
                }
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEarlyLeaveApproveNRejectHistoryListById(EarlyLeaveApproveNRejectHistory_Search parameters)
        {
            IEnumerable<EarlyLeaveApproveNRejectHistory_Response> lstObj = await _manageEarlyLeaveRepository.GetEarlyLeaveApproveNRejectHistoryListById(parameters);
            _response.Data = lstObj.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
    }
}
