using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Visitor.API.Middlewares;
using Visitor.API.CustomAttributes;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Visitor.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IBarcodeRepository _barcodeRepository;
        private IEmailHelper _emailHelper;
        private IFileManager _fileManager;

        public UserController(IUserRepository userRepository, ICompanyRepository companyRepository, IBranchRepository branchRepository, IFileManager fileManager, IBarcodeRepository barcodeRepository, IEmailHelper emailHelper)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _branchRepository = branchRepository;
            _fileManager = fileManager;
            _barcodeRepository = barcodeRepository;
            _emailHelper = emailHelper;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region User 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveUser(User_Request parameters)
        {
            #region User Restriction 

            int vCompanyNoofUserAdd = 0;
            int vBranchNoofUserAdd = 0;

            int totalCompanyRegisteredUser = 0;
            //int totalBranchRegisteredUser = 0;

            if (parameters.Id == 0)
            {
                var baseSearch = new User_Search();
                baseSearch.DepartmentId = 0;

                var vUser = await _userRepository.GetUserList(baseSearch);

                #region Company Wise User Check

                if (parameters.CompanyId > 0)
                {
                    var vCompany = await _companyRepository.GetCompanyById(parameters.CompanyId);
                    if (vCompany != null)
                    {
                        vCompanyNoofUserAdd = vCompany.NoofUserAdd ?? 0;
                    }
                }

                if (parameters.CompanyId > 0 && parameters.BranchList.Count == 0)
                {
                    //get total company user
                    totalCompanyRegisteredUser = vUser.Where(x => x.IsActive == true && x.CompanyId == parameters.CompanyId).Count();

                    // Total Company User check with register user
                    if (totalCompanyRegisteredUser >= vCompanyNoofUserAdd)
                    {
                        _response.Message = "You are not allowed to create user more then " + vCompanyNoofUserAdd + ", Please contact your administrator to access this feature!";
                        return _response;
                    }
                }

                #endregion

                #region Company and Branch Wise User Check

                List<string> strBranchList = new List<string>();

                if (parameters.CompanyId > 0 && parameters.BranchList.Count > 0)
                {
                    foreach (var vBranchitem in parameters.BranchList)
                    {
                        var vBranchMappingObj = await _branchRepository.GetBranchMappingByEmployeeId(0, Convert.ToInt32(vBranchitem.BranchId));

                        var vBranchObj = await _branchRepository.GetBranchById(Convert.ToInt32(vBranchitem.BranchId));

                        if (vBranchMappingObj.Count() >= vCompanyNoofUserAdd)
                        {
                            strBranchList.Add(vBranchObj != null ? vBranchObj.BranchName : string.Empty);
                        }
                    }

                    if (strBranchList.Count > 0)
                    {
                        string sbranchListCommaseparated = string.Join(", ", strBranchList);

                        _response.Message = "You are not allowed to create user more then " + vCompanyNoofUserAdd + " for branch " + sbranchListCommaseparated + ", Please contact your administrator to access this feature!";
                        return _response;
                    }
                }

                #endregion
            }

            #endregion

            // Aadhar Card Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.AadharImage_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.AadharImage_Base64, "\\Uploads\\Employee\\", parameters.AadharOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.AadharImage = vUploadFile;
                }
            }

            // Pan Card Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.PanCardImage_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.PanCardImage_Base64, "\\Uploads\\Employee\\", parameters.PanCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PanCardImage = vUploadFile;
                }
            }

            // Profile Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.ProfileImage_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.ProfileImage_Base64, "\\Uploads\\Employee\\", parameters.ProfileOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.ProfileImage = vUploadFile;
                }
            }

            // Other Proof Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.OtherProofImage_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.OtherProofImage_Base64, "\\Uploads\\Employee\\", parameters.OtherProofOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.OtherProofImage = vUploadFile;
                }
            }

            //auto generated password
            string autoGeneratePass = "";
            if (string.IsNullOrEmpty(parameters.Password) && parameters.Id == 0)
            {
                var resultPass = await _userRepository.GetAutoGenPassword("");
                if (!string.IsNullOrEmpty(resultPass))
                {
                    autoGeneratePass = resultPass;
                    parameters.Password = resultPass;
                }
            }

            int result = await _userRepository.SaveUser(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Employee Code already exists";
            }
            else if (result == -3)
            {
                _response.Message = "Email already exists";
            }
            else if (result == -4)
            {
                _response.Message = "Mobile already exists";
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

                #region // Add/Update Branch Mapping

                // Delete Old mapping of employee

                var vBracnMapDELETEObj = new BranchMapping_Request()
                {
                    Action = "DELETE",
                    UserId = result,
                    BranchId = 0
                };
                int resultBranchMappingDELETE = await _branchRepository.SaveBranchMapping(vBracnMapDELETEObj);


                // Add new mapping of employee
                foreach (var vBranchitem in parameters.BranchList)
                {
                    var vBracnMapObj = new BranchMapping_Request()
                    {
                        Action = "INSERT",
                        UserId = result,
                        BranchId = vBranchitem.BranchId
                    };

                    int resultBranchMapping = await _branchRepository.SaveBranchMapping(vBracnMapObj);
                }
                #endregion

                #region User Other Details
                if (result > 0)
                {
                    foreach (var items in parameters.UserOtherDetailsList)
                    {
                        var vUserOtherDetails = new UserOtherDetails_Request()
                        {
                            Id = items.Id,
                            EmployeeId = result,
                            PastCompanyName = items.PastCompanyName,
                            TotalExp = items.TotalExp,
                            Remark = items.Remark
                        };

                        int resultUserOtherDetails = await _userRepository.SaveUserOtherDetails(vUserOtherDetails);
                    }

                }
                #endregion

                #region // Add/Update Gate Number

                // Delete Old gate of employee

                var vGateDELETEObj = new EmployeeGateNo_Request()
                {
                    Action = "DELETE",
                    EmployeeId = result,
                    GateDetailsId = 0
                };
                int resultGateDELETE = await _userRepository.SaveEmployeeGateNo(vGateDELETEObj);


                // Add new gate number
                foreach (var vGateitem in parameters.GateNumberList)
                {
                    var vGateObj = new EmployeeGateNo_Request()
                    {
                        Action = "INSERT",
                        EmployeeId = result,
                        GateDetailsId = vGateitem.GateDetailsId
                    };

                    int resultGate = await _userRepository.SaveEmployeeGateNo(vGateObj);
                }

                #endregion

                #region Generate Barcode
                if (parameters.Id == 0)
                {
                    var vGenerateBarcode = _barcodeRepository.GenerateBarcode(parameters.UserCode);
                    if (vGenerateBarcode.Barcode_Unique_Id != "")
                    {
                        var vBarcode_Request = new Barcode_Request()
                        {
                            Id = 0,
                            BarcodeNo = parameters.UserCode,
                            BarcodeType = "Employee",
                            Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                            BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                            BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                            RefId = result
                        };
                        var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
                    }
                }
                #endregion

                //Send Email
                if (!string.IsNullOrEmpty(parameters.EmailId) && parameters.Id == 0)
                {
                    var vEmailEmp = await SendPassword_EmailToEmployee(autoGeneratePass, result, "Login Credential");
                }
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserList(User_Search parameters)
        {
            IEnumerable<User_Response> lstUsers = await _userRepository.GetUserList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetUserById(long Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _userRepository.GetUserById(Id);

                if (vResultObj != null)
                {
                    var vBranchMappingObj = await _branchRepository.GetBranchMappingByEmployeeId(vResultObj.Id, 0);
                    foreach (var item in vBranchMappingObj)
                    {
                        var vBranchObj = await _branchRepository.GetBranchById(Convert.ToInt32(item.BranchId));
                        var vBrMapResOnj = new BranchMapping_Response()
                        {
                            Id = item.Id,
                            UserId = vResultObj.Id,
                            BranchId = item.BranchId,
                            BranchName = vBranchObj != null ? vBranchObj.BranchName : string.Empty,
                        };

                        vResultObj.BranchList.Add(vBrMapResOnj);
                    }

                    //get user other details
                    var vUserOtherDetailsObj = await _userRepository.GetUserOtherDetailsByEmployeeId(vResultObj.Id);
                    foreach (var item in vUserOtherDetailsObj)
                    {
                        var vUserOtherDetailsResObj = new UserOtherDetails_Response()
                        {
                            Id = item.Id,
                            EmployeeId = vResultObj.Id,
                            PastCompanyName = item.PastCompanyName,
                            TotalExp = item.TotalExp,
                            Remark = item.Remark
                        };

                        vResultObj.UserOtherDetailsList.Add(vUserOtherDetailsResObj);
                    }

                    var gateNolistObj = await _userRepository.GetEmployeeGateNoByEmployeeId(vResultObj.Id, 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ChangePassword(ChangePassword_Request parameters)
        {
            int result = await _userRepository.ChangePassword(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "User data does not exist please enter correct details";
                _response.IsSuccess = false;
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
                _response.IsSuccess = false;
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
                _response.IsSuccess = false;
            }
            else
            {
                _response.Message = "Record details saved successfully";
            }

            return _response;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ForgotPassword(ForgotPassword_Request parameters)
        {
            if (parameters.EmailId == "")
            {
                _response.IsSuccess = false;
                _response.Message = "Email Id is required.";

                return _response;
            }

            //auto generated password
            string autoGeneratePass = "";
            var resultPass = await _userRepository.GetAutoGenPassword("");
            if (!string.IsNullOrEmpty(resultPass))
            {
                autoGeneratePass = resultPass;
                parameters.Passwords = resultPass;
            }

            int result = await _userRepository.ForgotPassword(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "User data does not exist please enter correct details";
                _response.IsSuccess = false;
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
                _response.IsSuccess = false;
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
                _response.IsSuccess = false;
            }
            else
            {
                _response.Message = "Recovery Password sent to your email address.";

                //Send Email
                if (!string.IsNullOrEmpty(parameters.EmailId))
                {
                    var vEmailEmp = await SendPassword_EmailToEmployee(autoGeneratePass, result, "Forgot Password");
                }
            }

            return _response;
        }

        #endregion

        #region User Other Details

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveUserOtherDetails(UserOtherDetails_Request parameters)
        {
            int result = await _userRepository.SaveUserOtherDetails(parameters);

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
        public async Task<ResponseModel> GetUserOtherDetailsById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _userRepository.GetUserOtherDetailsById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> DeleteUserOtherDetails(int Id)
        {
            int result = await _userRepository.DeleteUserOtherDetails(Id);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details deleted successfully";
            }
            return _response;
        }

        #endregion

        protected async Task<bool> SendPassword_EmailToEmployee(string passwords, int id, string emailType)
        {
            bool result = false;
            string emailTemplateContent = "", remarks = "", sSubjectDynamicContent = "";

            try
            {
                string recipientEmail = "";
                remarks = id.ToString();

                var vUserDetail = await _userRepository.GetUserById(Convert.ToInt32(id));

                if (vUserDetail != null)
                {
                    recipientEmail = vUserDetail.EmailId;

                    if(emailType == "Login Credential")
                    {
                        sSubjectDynamicContent = "Login Credential - " + vUserDetail.UserName;
                        emailTemplateContent = string.Format(@"<html><body>Dear {0},<br/><br/>Your login credential below:<br/><br/> EmailId: {1}<br/>Password: {2}<br/><br/>If you did not initiate this request, you may safely disregard this email.</body></html>", vUserDetail.UserName, vUserDetail.EmailId, passwords);

                        result = await _emailHelper.SendEmail(module: "Login Credential", subject: sSubjectDynamicContent, sendTo: "Employee", content: emailTemplateContent, recipientEmail: recipientEmail, files: null, remarks: remarks);
                    }
                    else if (emailType == "Forgot Password")
                    {
                        sSubjectDynamicContent = "Forgot Password - " + vUserDetail.UserName;
                        emailTemplateContent = string.Format(@"<html><body>Dear {0},<br/><br/>A request was made to reset your password. To proceed, please use the new password provided below:<br/><br/> Password: {1}<br/><br/>If you did not initiate this request, you may safely disregard this email.</body></html>", vUserDetail.UserName, passwords);

                        result = await _emailHelper.SendEmail(module: "Forgot Password", subject: sSubjectDynamicContent, sendTo: "Employee", content: emailTemplateContent, recipientEmail: recipientEmail, files: null, remarks: remarks);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
