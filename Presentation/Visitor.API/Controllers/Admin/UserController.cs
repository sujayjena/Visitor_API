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
using OfficeOpenXml.Style;
using OfficeOpenXml;

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
                _response.Message = "Mobile Number already exists";
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

                    if (emailType == "Login Credential")
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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> DownloadUserTemplate()
        {
            byte[]? formatFile = await Task.Run(() => _fileManager.GetFormatFileFromPath("Template_User.xlsx"));

            if (formatFile != null)
            {
                _response.Data = formatFile;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportUser([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;

            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            List<string[]> data = new List<string[]>();
            List<User_ImportData> lstUser_ImportData = new List<User_ImportData>();
            IEnumerable<User_ImportDataValidation> lstUser_ImportDataValidation;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file";
                return _response;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                request.FileUpload.CopyTo(stream);
                using ExcelPackage package = new ExcelPackage(stream);
                currentSheet = package.Workbook.Worksheets;
                workSheet = currentSheet.First();
                noOfCol = workSheet.Dimension.End.Column;
                noOfRow = workSheet.Dimension.End.Row;

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "UserCode", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "UserName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "UserType", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "MobileNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "EmailId", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 6].Value.ToString(), "Password", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 7].Value.ToString(), "GateNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 8].Value.ToString(), "Role", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 9].Value.ToString(), "ReportingTo", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 10].Value.ToString(), "Department", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 11].Value.ToString(), "Company", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 12].Value.ToString(), "DateOfBirth", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 13].Value.ToString(), "DateOfJoining", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 14].Value.ToString(), "BloodGroup", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 15].Value.ToString(), "Gender", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 16].Value.ToString(), "MeritalStatus", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 17].Value.ToString(), "EmergencyName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 18].Value.ToString(), "EmergencyContactNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 19].Value.ToString(), "EmergencyRelation", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 20].Value.ToString(), "PastCompanyName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 21].Value.ToString(), "TotalExp", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 22].Value.ToString(), "Remark", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 23].Value.ToString(), "Address", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 24].Value.ToString(), "Country", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 25].Value.ToString(), "State", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 26].Value.ToString(), "Province", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 27].Value.ToString(), "Pincode", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 28].Value.ToString(), "IsSameAsPermanent", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 29].Value.ToString(), "Temporary_Address", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 30].Value.ToString(), "Temporary_Country", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 31].Value.ToString(), "Temporary_State", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 32].Value.ToString(), "Temporary_Province", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 33].Value.ToString(), "Temporary_Pincode", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 34].Value.ToString(), "AadharNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 35].Value.ToString(), "PanNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 36].Value.ToString(), "OtherProof", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 37].Value.ToString(), "IsHOD", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 38].Value.ToString(), "IsManager", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 39].Value.ToString(), "IsApprover", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 40].Value.ToString(), "IsMobileUser", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 41].Value.ToString(), "IsWebUser", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 42].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    if (!string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 2].Value?.ToString()) && !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 3].Value?.ToString()))
                    {
                        lstUser_ImportData.Add(new User_ImportData()
                        {
                            UserCode = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                            UserName = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                            UserType = workSheet.Cells[rowIterator, 3].Value?.ToString(),

                            MobileNumber = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                            EmailId = workSheet.Cells[rowIterator, 5].Value?.ToString(),
                            Password = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 6].Value?.ToString()) ? EncryptDecryptHelper.EncryptString(workSheet.Cells[rowIterator, 6].Value?.ToString()) : string.Empty,
                            GateNumber = workSheet.Cells[rowIterator, 7].Value?.ToString(),
                            Role = workSheet.Cells[rowIterator, 8].Value?.ToString(),
                            ReportingTo = workSheet.Cells[rowIterator, 9].Value?.ToString(),
                            Department = workSheet.Cells[rowIterator, 10].Value?.ToString(),
                            Company = workSheet.Cells[rowIterator, 11].Value?.ToString(),
                            DateOfBirth = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 12].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 12].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            DateOfJoining = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 13].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 13].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            BloodGroup = workSheet.Cells[rowIterator, 14].Value?.ToString(),
                            Gender = workSheet.Cells[rowIterator, 15].Value?.ToString(),
                            MeritalStatus = workSheet.Cells[rowIterator, 16].Value?.ToString(),
                            EmergencyName = workSheet.Cells[rowIterator, 17].Value?.ToString(),
                            EmergencyContactNumber = workSheet.Cells[rowIterator, 18].Value?.ToString(),
                            EmergencyRelation = workSheet.Cells[rowIterator, 19].Value?.ToString(),
                            PastCompanyName = workSheet.Cells[rowIterator, 20].Value?.ToString(),
                            TotalExp = workSheet.Cells[rowIterator, 21].Value?.ToString(),
                            Remark = workSheet.Cells[rowIterator, 22].Value?.ToString(),

                            Address = workSheet.Cells[rowIterator, 23].Value?.ToString(),
                            Country = workSheet.Cells[rowIterator, 24].Value?.ToString(),
                            State = workSheet.Cells[rowIterator, 25].Value?.ToString(),
                            Province = workSheet.Cells[rowIterator, 26].Value?.ToString(),
                            Pincode = workSheet.Cells[rowIterator, 27].Value?.ToString(),

                            IsSameAsPermanent = workSheet.Cells[rowIterator, 28].Value?.ToString(),

                            Temporary_Address = workSheet.Cells[rowIterator, 29].Value?.ToString(),
                            Temporary_Country = workSheet.Cells[rowIterator, 30].Value?.ToString(),
                            Temporary_State = workSheet.Cells[rowIterator, 31].Value?.ToString(),
                            Temporary_Province = workSheet.Cells[rowIterator, 32].Value?.ToString(),
                            Temporary_Pincode = workSheet.Cells[rowIterator, 33].Value?.ToString(),

                            AadharNumber = workSheet.Cells[rowIterator, 34].Value?.ToString(),
                            PanNumber = workSheet.Cells[rowIterator, 35].Value?.ToString(),
                            OtherProof = workSheet.Cells[rowIterator, 36].Value?.ToString(),
                            IsHOD = workSheet.Cells[rowIterator, 37].Value?.ToString(),
                            IsManager = workSheet.Cells[rowIterator, 38].Value?.ToString(),
                            IsApprover = workSheet.Cells[rowIterator, 39].Value?.ToString(),

                            IsMobileUser = workSheet.Cells[rowIterator, 40].Value?.ToString(),
                            IsWebUser = workSheet.Cells[rowIterator, 41].Value?.ToString(),
                            IsActive = workSheet.Cells[rowIterator, 42].Value?.ToString()
                        });
                    }
                }
            }

            if (lstUser_ImportData.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstUser_ImportDataValidation = await _userRepository.ImportUser(lstUser_ImportData);

            _response.IsSuccess = true;
            _response.Message = "Record imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstUser_ImportDataValidation.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidImportDataFile(lstUser_ImportDataValidation);

            }

            #endregion

            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportUserData(bool IsActive = true)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new User_Search();
            request.IsActive = IsActive;

            IEnumerable<User_Response> lstSizeObj = await _userRepository.GetUserList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Employee");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "User Code";
                    WorkSheet1.Cells[1, 2].Value = "User Name";
                    WorkSheet1.Cells[1, 3].Value = "User Type";
                    WorkSheet1.Cells[1, 4].Value = "Mobile";
                    WorkSheet1.Cells[1, 5].Value = "EmailId";
                    WorkSheet1.Cells[1, 6].Value = "Gate Number";
                    WorkSheet1.Cells[1, 7].Value = "Role";
                    WorkSheet1.Cells[1, 8].Value = "ReportingTo";
                    WorkSheet1.Cells[1, 9].Value = "Department";
                    WorkSheet1.Cells[1, 10].Value = "Company";
                    WorkSheet1.Cells[1, 11].Value = "DateOfBirth";
                    WorkSheet1.Cells[1, 12].Value = "Date Of Joining";
                    WorkSheet1.Cells[1, 13].Value = "Blood Group";
                    WorkSheet1.Cells[1, 14].Value = "Gender";
                    WorkSheet1.Cells[1, 15].Value = "Merital Status";

                    WorkSheet1.Cells[1, 16].Value = "Emergency Name";
                    WorkSheet1.Cells[1, 17].Value = "Emergency Contact Number";
                    WorkSheet1.Cells[1, 18].Value = "Emergency Relation";
                    //WorkSheet1.Cells[1, 19].Value = "Past Company Name";
                    //WorkSheet1.Cells[1, 20].Value = "Total Exp";
                    //WorkSheet1.Cells[1, 21].Value = "Remark";

                    WorkSheet1.Cells[1, 19].Value = "Address";
                    WorkSheet1.Cells[1, 20].Value = "Country";
                    WorkSheet1.Cells[1, 21].Value = "State";
                    WorkSheet1.Cells[1, 22].Value = "Province";
                    WorkSheet1.Cells[1, 23].Value = "Pincode";

                    WorkSheet1.Cells[1, 24].Value = "IsSameAsPermanent";
                    WorkSheet1.Cells[1, 25].Value = "Temporary_Address";
                    WorkSheet1.Cells[1, 26].Value = "Temporary_Country";
                    WorkSheet1.Cells[1, 27].Value = "Temporary_State";
                    WorkSheet1.Cells[1, 28].Value = "Temporary_Province";
                    WorkSheet1.Cells[1, 29].Value = "Temporary_Pincode";

                    WorkSheet1.Cells[1, 30].Value = "Aadhar Number";
                    WorkSheet1.Cells[1, 31].Value = "Pan Number";

                    WorkSheet1.Cells[1, 32].Value = "OtherProof";
                    WorkSheet1.Cells[1, 33].Value = "IsHOD";
                    WorkSheet1.Cells[1, 34].Value = "IsManager";
                    WorkSheet1.Cells[1, 35].Value = "IsApprover";

                    WorkSheet1.Cells[1, 36].Value = "IsMobileUser";
                    WorkSheet1.Cells[1, 37].Value = "IsWebUser";
                    WorkSheet1.Cells[1, 38].Value = "IsActive";

                    recordIndex = 2;

                    foreach (var items in lstSizeObj)
                    {
                        var vResultObj = await _userRepository.GetUserById(items.Id);

                        string strGateNumberList = string.Empty;
                        var vSecurityGateDetail = await _userRepository.GetEmployeeGateNoByEmployeeId(EmployeeId: Convert.ToInt32(items.Id), GateDetailsId: 0);
                        if (vSecurityGateDetail.ToList().Count > 0)
                        {
                            strGateNumberList = string.Join(",", vSecurityGateDetail.ToList().OrderBy(x => x.GateDetailsId).Select(x => x.GateDetailsId));
                        }

                        WorkSheet1.Cells[recordIndex, 1].Value = items.UserCode;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.UserName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.UserType;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.EmailId;
                        WorkSheet1.Cells[recordIndex, 6].Value = strGateNumberList;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.RoleName;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.ReportingToName;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.DepartmentName;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.CompanyName;

                        WorkSheet1.Cells[recordIndex, 11].Value = items.DateOfBirth.HasValue ? items.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.DateOfJoining.HasValue ? items.DateOfJoining.Value.ToString("dd/MM/yyyy") : string.Empty;

                        WorkSheet1.Cells[recordIndex, 13].Value = items.BloodGroup;

                        WorkSheet1.Cells[recordIndex, 14].Value = items.GenderName;
                        WorkSheet1.Cells[recordIndex, 15].Value = vResultObj != null ? vResultObj.MaritalStatus : string.Empty;
                        WorkSheet1.Cells[recordIndex, 16].Value = items.EmergencyName;
                        WorkSheet1.Cells[recordIndex, 17].Value = items.EmergencyContactNumber;
                        WorkSheet1.Cells[recordIndex, 18].Value = items.EmergencyRelation;
                        //WorkSheet1.Cells[recordIndex, 16].Value = items.PastCompanyName;
                        //WorkSheet1.Cells[recordIndex, 16].Value = items.TotalExp;
                        //WorkSheet1.Cells[recordIndex, 16].Value = items.Remark;

                        WorkSheet1.Cells[recordIndex, 19].Value = items.AddressLine;
                        WorkSheet1.Cells[recordIndex, 20].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 21].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 22].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 23].Value = items.Pincode;

                        WorkSheet1.Cells[recordIndex, 24].Value = items.IsSameAsPermanent;
                        WorkSheet1.Cells[recordIndex, 25].Value = items.TemporaryAddress;
                        WorkSheet1.Cells[recordIndex, 26].Value = items.Temporary_CountryName;
                        WorkSheet1.Cells[recordIndex, 27].Value = items.Temporary_StateName;
                        WorkSheet1.Cells[recordIndex, 28].Value = items.Temporary_DistrictName;
                        WorkSheet1.Cells[recordIndex, 29].Value = items.Temporary_Pincode;

                        WorkSheet1.Cells[recordIndex, 30].Value = items.AadharNumber;
                        WorkSheet1.Cells[recordIndex, 31].Value = items.PanNumber;

                        WorkSheet1.Cells[recordIndex, 32].Value = items.OtherProof;
                        WorkSheet1.Cells[recordIndex, 33].Value = items.IsHOD;
                        WorkSheet1.Cells[recordIndex, 34].Value = items.IsManager;
                        WorkSheet1.Cells[recordIndex, 35].Value = items.IsApprover;

                        WorkSheet1.Cells[recordIndex, 36].Value = items.IsMobileUser;
                        WorkSheet1.Cells[recordIndex, 37].Value = items.IsWebUser;
                        WorkSheet1.Cells[recordIndex, 38].Value = items.IsActive == true ? "Active" : "Inactive";

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Exported successfully";
            }

            return _response;
        }

        private byte[] GenerateInvalidImportDataFile(IEnumerable<User_ImportDataValidation> lstUser_ImportDataValidation)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "UserCode";
                    WorkSheet1.Cells[1, 2].Value = "UserName";
                    WorkSheet1.Cells[1, 3].Value = "UserType";
                    WorkSheet1.Cells[1, 4].Value = "MobileNumber";
                    WorkSheet1.Cells[1, 5].Value = "EmailId";
                    WorkSheet1.Cells[1, 6].Value = "Password";
                    WorkSheet1.Cells[1, 7].Value = "GateNumber";

                    WorkSheet1.Cells[1, 8].Value = "Role";
                    WorkSheet1.Cells[1, 9].Value = "ReportingTo";
                    WorkSheet1.Cells[1, 10].Value = "Department";
                    WorkSheet1.Cells[1, 11].Value = "Company";
                    WorkSheet1.Cells[1, 12].Value = "DateOfBirth";
                    WorkSheet1.Cells[1, 13].Value = "DateOfJoining";
                    WorkSheet1.Cells[1, 14].Value = "BloodGroup";

                    WorkSheet1.Cells[1, 15].Value = "Gender";
                    WorkSheet1.Cells[1, 16].Value = "MeritalStatus";
                    WorkSheet1.Cells[1, 17].Value = "EmergencyName";
                    WorkSheet1.Cells[1, 18].Value = "EmergencyContactNumber";
                    WorkSheet1.Cells[1, 19].Value = "EmergencyRelation";

                    WorkSheet1.Cells[1, 20].Value = "PastCompanyName";
                    WorkSheet1.Cells[1, 21].Value = "TotalExp";
                    WorkSheet1.Cells[1, 22].Value = "Remark";

                    WorkSheet1.Cells[1, 23].Value = "Address";
                    WorkSheet1.Cells[1, 24].Value = "Country";
                    WorkSheet1.Cells[1, 25].Value = "State";
                    WorkSheet1.Cells[1, 26].Value = "Province";
                    WorkSheet1.Cells[1, 27].Value = "Pincode";

                    WorkSheet1.Cells[1, 28].Value = "IsSameAsPermanent";
                    WorkSheet1.Cells[1, 29].Value = "Temporary_Address";
                    WorkSheet1.Cells[1, 30].Value = "Temporary_Country";
                    WorkSheet1.Cells[1, 31].Value = "Temporary_State";
                    WorkSheet1.Cells[1, 32].Value = "Temporary_Province";
                    WorkSheet1.Cells[1, 33].Value = "Temporary_Pincode";

                    WorkSheet1.Cells[1, 34].Value = "AadharNumber";
                    WorkSheet1.Cells[1, 35].Value = "PanNumber";
                    WorkSheet1.Cells[1, 36].Value = "OtherProof";
                    WorkSheet1.Cells[1, 37].Value = "IsHOD";
                    WorkSheet1.Cells[1, 38].Value = "IsManager";
                    WorkSheet1.Cells[1, 39].Value = "IsApprover";

                    WorkSheet1.Cells[1, 40].Value = "IsMobileUser";
                    WorkSheet1.Cells[1, 41].Value = "IsWebUser";
                    WorkSheet1.Cells[1, 42].Value = "ErrorMessage";

                    recordIndex = 2;

                    foreach (User_ImportDataValidation record in lstUser_ImportDataValidation)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.UserCode;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.UserName;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.UserType;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.EmailId;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.Password;
                        WorkSheet1.Cells[recordIndex, 7].Value = record.GateNumber;

                        WorkSheet1.Cells[recordIndex, 8].Value = record.Role;
                        WorkSheet1.Cells[recordIndex, 9].Value = record.ReportingTo;
                        WorkSheet1.Cells[recordIndex, 10].Value = record.Department;
                        WorkSheet1.Cells[recordIndex, 11].Value = record.Company;

                        WorkSheet1.Cells[recordIndex, 12].Value = record.DateOfBirth;
                        WorkSheet1.Cells[recordIndex, 13].Value = record.DateOfJoining;
                        WorkSheet1.Cells[recordIndex, 14].Value = record.BloodGroup;

                        WorkSheet1.Cells[recordIndex, 15].Value = record.Gender;
                        WorkSheet1.Cells[recordIndex, 16].Value = record.MeritalStatus;
                        WorkSheet1.Cells[recordIndex, 17].Value = record.EmergencyName;
                        WorkSheet1.Cells[recordIndex, 18].Value = record.EmergencyContactNumber;
                        WorkSheet1.Cells[recordIndex, 19].Value = record.EmergencyRelation;

                        WorkSheet1.Cells[recordIndex, 20].Value = record.PastCompanyName;
                        WorkSheet1.Cells[recordIndex, 21].Value = record.TotalExp;
                        WorkSheet1.Cells[recordIndex, 22].Value = record.Remark;

                        WorkSheet1.Cells[recordIndex, 23].Value = record.Address;
                        WorkSheet1.Cells[recordIndex, 24].Value = record.Country;
                        WorkSheet1.Cells[recordIndex, 25].Value = record.State;
                        WorkSheet1.Cells[recordIndex, 26].Value = record.Province;
                        WorkSheet1.Cells[recordIndex, 27].Value = record.Pincode;

                        WorkSheet1.Cells[recordIndex, 28].Value = record.IsSameAsPermanent;
                        WorkSheet1.Cells[recordIndex, 29].Value = record.Temporary_Address;
                        WorkSheet1.Cells[recordIndex, 30].Value = record.Temporary_Country;
                        WorkSheet1.Cells[recordIndex, 31].Value = record.Temporary_State;
                        WorkSheet1.Cells[recordIndex, 32].Value = record.Temporary_Province;
                        WorkSheet1.Cells[recordIndex, 33].Value = record.Temporary_Pincode;

                        WorkSheet1.Cells[recordIndex, 34].Value = record.AadharNumber;
                        WorkSheet1.Cells[recordIndex, 35].Value = record.PanNumber;
                        WorkSheet1.Cells[recordIndex, 36].Value = record.OtherProof;
                        WorkSheet1.Cells[recordIndex, 37].Value = record.IsHOD;
                        WorkSheet1.Cells[recordIndex, 38].Value = record.IsManager;
                        WorkSheet1.Cells[recordIndex, 39].Value = record.IsApprover;

                        WorkSheet1.Cells[recordIndex, 40].Value = record.IsMobileUser;
                        WorkSheet1.Cells[recordIndex, 41].Value = record.IsWebUser;
                        WorkSheet1.Cells[recordIndex, 42].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }
    }
}
