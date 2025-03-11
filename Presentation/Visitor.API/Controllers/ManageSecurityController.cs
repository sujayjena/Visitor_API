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
    public class ManageSecurityController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageSecurityRepository _manageSecurityRepository;
        private readonly IAdminMasterRepository _adminMasterRepository;
        private IFileManager _fileManager;

        public ManageSecurityController(IManageSecurityRepository manageSecurityRepository, IFileManager fileManager, IAdminMasterRepository adminMasterRepository)
        {
            _manageSecurityRepository = manageSecurityRepository;
            _fileManager = fileManager;
            _adminMasterRepository = adminMasterRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Security Login 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveSecurityLogin(SecurityLogin_Request parameters)
        {
            // Photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Photo_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Photo_Base64, "\\Uploads\\Employee\\", parameters.PhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PhotoFileName = vUploadFile;
                }
            }

            // Documnet Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Document_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Document_Base64, "\\Uploads\\Employee\\", parameters.DocumentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DocumentFileName = vUploadFile;
                }
            }

            int result = await _manageSecurityRepository.SaveSecurityLogin(parameters);

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

                #region // Add/Update Gate Deatils

                // Delete Old Gate Deatils

                var vGateDELETEObj = new SecurityLoginGateDetails_Request()
                {
                    Action = "DELETE",
                    SecurityLoginId = result,
                    GateDetailsId = 0
                };
                int resultGateDetailsDELETE = await _manageSecurityRepository.SaveSecurityLoginGateDetails(vGateDELETEObj);


                // Add new Gate Deatils
                foreach (var vitem in parameters.GateDetailsList)
                {
                    var vGateDObj = new SecurityLoginGateDetails_Request()
                    {
                        Action = "INSERT",
                        SecurityLoginId = result,
                        GateDetailsId = vitem.GateDetailsId
                    };

                    int resultGateD = await _manageSecurityRepository.SaveSecurityLoginGateDetails(vGateDObj);
                }

                #endregion
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSecurityLoginList(SecurityLogin_Search parameters)
        {
            IEnumerable<SecurityLogin_Response> lstUsers = await _manageSecurityRepository.GetSecurityLoginList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSecurityLoginById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageSecurityRepository.GetSecurityLoginById(Id);

                if (vResultObj != null)
                {
                    var vGateDetailsObj = await _manageSecurityRepository.GetSecurityLoginGateDetailsById(vResultObj.Id, 0);

                    foreach (var item in vGateDetailsObj)
                    {
                        var vGateObj = await _adminMasterRepository.GetGateDetailsById(Convert.ToInt32(item.GateDetailsId));
                        var vGateResOnj = new SecurityLoginGateDetails_Response()
                        {
                            Id = item.Id,
                            SecurityLoginId = vResultObj.Id,
                            GateDetailsId = item.GateDetailsId,
                            GateNumber = vGateObj != null ? vGateObj.GateNumber : string.Empty,
                        };

                        vResultObj.GateDetailsList.Add(vGateResOnj);
                    }
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}