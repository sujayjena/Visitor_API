using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Visitor.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Department

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDepartment(Department_Request parameters)
        {
            int result = await _profileRepository.SaveDepartment(parameters);

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
        public async Task<ResponseModel> GetDepartmentList(Department_Search parameters)
        {
            IEnumerable<Department_Response> lstRoles = await _profileRepository.GetDepartmentList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDepartmentById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _profileRepository.GetDepartmentById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Role 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRole(Role_Request parameters)
        {

            var fdgf = SessionManager.LoggedInUserId;

            int result = await _profileRepository.SaveRole(parameters);

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
        public async Task<ResponseModel> GetRoleList(Role_Search parameters)
        {
            IEnumerable<Role_Response> lstRoles = await _profileRepository.GetRoleList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRoleById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _profileRepository.GetRoleById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region RoleHierarchy 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRoleHierarchy(RoleHierarchy_Request parameters)
        {

            var fdgf = SessionManager.LoggedInUserId;

            int result = await _profileRepository.SaveRoleHierarchy(parameters);

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
        public async Task<ResponseModel> GetRoleHierarchyList(RoleHierarchy_Search parameters)
        {
            IEnumerable<RoleHierarchy_Response> lstRoleHierarchys = await _profileRepository.GetRoleHierarchyList(parameters);
            _response.Data = lstRoleHierarchys.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRoleHierarchyById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _profileRepository.GetRoleHierarchyById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region Reporting To 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetReportingToEmpListForSelectList(ReportingToEmpListParameters parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _profileRepository.GetReportingToEmployeeForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEmployeesListByReportingTo(int EmployeeId)
        {
            IEnumerable<EmployeesListByReportingTo_Response> lstResponse = await _profileRepository.GetEmployeesListByReportingTo(EmployeeId);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        #endregion
    }
}
