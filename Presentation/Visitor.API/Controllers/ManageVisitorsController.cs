using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Net;
using System.Text;
using Visitor.API.CustomAttributes;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using System.Net.Mail;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageVisitorsController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageVisitorsRepository _manageVisitorsRepository;
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly IUserRepository _userRepository;
        private ILoginRepository _loginRepository;
        private IConfigRefRepository _configRefRepository;
        private IFileManager _fileManager;
        private readonly IWebHostEnvironment _environment;
        private IEmailHelper _emailHelper;
        private ISMSHelper _smsHelper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IAssignGateNoRepository _assignGateNoRepository;

        public ManageVisitorsController(IManageVisitorsRepository manageVisitorsRepository, IFileManager fileManager, IBarcodeRepository barcodeRepository, IUserRepository userRepository, IWebHostEnvironment environment, IEmailHelper emailHelper, ILoginRepository loginRepository, IConfigRefRepository configRefRepository, ISMSHelper smsHelper, INotificationRepository notificationRepository, IAssignGateNoRepository assignGateNoRepository)
        {
            _manageVisitorsRepository = manageVisitorsRepository;
            _fileManager = fileManager;
            _barcodeRepository = barcodeRepository;
            _userRepository = userRepository;
            _environment = environment;
            _emailHelper = emailHelper;
            _loginRepository = loginRepository;
            _configRefRepository = configRefRepository;
            _smsHelper = smsHelper;
            _notificationRepository = notificationRepository;
            _assignGateNoRepository = assignGateNoRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVisitors(Visitors_Request parameters)
        {
            // ID Upload
            //if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.IDImage_Base64))
            //{
            //    var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.IDImage_Base64, "\\Uploads\\Visitors\\", parameters.IDOriginalFileName);

            //    if (!string.IsNullOrWhiteSpace(vUploadFile))
            //    {
            //        parameters.IDImage = vUploadFile;
            //    }
            //}

            //Visitor Photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.VisitorPhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.VisitorPhoto_Base64, "\\Uploads\\Visitors\\", parameters.VisitorPhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.VisitorPhoto = vUploadFile;
                }
            }

            //Company Id Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.CompanyId_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.CompanyId_Base64, "\\Uploads\\Visitors\\", parameters.CompanyIdOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.CompanyIdFileName = vUploadFile;
                }
            }

            //Vehicle Photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.VehiclePhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.VehiclePhoto_Base64, "\\Uploads\\Visitors\\", parameters.VehiclePhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.VehiclePhotoFileName = vUploadFile;
                }
            }

            //get prev BranchId, Department, EmployeeId
            int prevBranchId = 0;
            int prevDepartmentId = 0;
            int prevEmployeeId = 0;
            var vVisitNumber = "";

            if (parameters.Id > 0)
            {
                var vVisitor = await _manageVisitorsRepository.GetVisitorsById(parameters.Id);
                if (vVisitor != null)
                {
                    prevBranchId = Convert.ToInt32(vVisitor.BranchId ?? 0);
                    prevDepartmentId = Convert.ToInt32(vVisitor.DepartmentId ?? 0);
                    prevEmployeeId = Convert.ToInt32(vVisitor.EmployeeId ?? 0);
                    vVisitNumber = vVisitor.VisitNumber;
                }
            }

            int result = await _manageVisitorsRepository.SaveVisitors(parameters);

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

                #region // Add/Update Assign GateNo

                // Delete Assign
                var vGateNoDELETEObj = new AssignGateNo_Request()
                {
                    Action = "DELETE",
                    RefId = result,
                    RefType = "Visitor",
                    GateDetailsId = 0
                };
                int resultGateNoDELETE = await _assignGateNoRepository.SaveAssignGateNo(vGateNoDELETEObj);


                // add new gate details
                foreach (var vGateitem in parameters.GateNumberList)
                {
                    var vGateNoMapObj = new AssignGateNo_Request()
                    {
                        Action = "INSERT",
                        RefId = result,
                        RefType = "Visitor",
                        GateDetailsId = vGateitem.GateDetailsId
                    };

                    int resultGateNo = await _assignGateNoRepository.SaveAssignGateNo(vGateNoMapObj);
                }

                #endregion

                #region Document Verification

                foreach (var vitem in parameters.DocumentVerificationList)
                {
                    // Document Upload
                    if (vitem != null && !string.IsNullOrWhiteSpace(vitem.DocumentFile_Base64))
                    {
                        var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(vitem.DocumentFile_Base64, "\\Uploads\\Visitors\\", vitem.DocumentOriginalFileName);

                        if (!string.IsNullOrWhiteSpace(vUploadFile))
                        {
                            vitem.DocumentFileName = vUploadFile;
                        }
                    }

                    var vVisitorDocumentVerification = new VisitorDocumentVerification_Request()
                    {
                        Id = vitem.Id,
                        RefId = result,
                        RefType = "Visitor",
                        //VisitorId = result,
                        IDTypeId = vitem.IDTypeId,
                        DocumentNumber = vitem.DocumentNumber,
                        DocumentOriginalFileName = vitem.DocumentOriginalFileName,
                        DocumentFileName = vitem.DocumentFileName,
                        IsDocumentStatus = vitem.IsDocumentStatus,
                    };

                    int resultGateNo = await _manageVisitorsRepository.SaveVisitorDocumentVerification(vVisitorDocumentVerification);
                }

                #endregion

                #region Asset

                foreach (var vitem in parameters.AssetList)
                {
                    var vVisitorAsset = new VisitorAsset_Request()
                    {
                        Id = vitem.Id,
                        VisitorId = result,
                        AssetName = vitem.AssetName,
                        AssetDesc = vitem.AssetDesc
                    };

                    int resultGateNo = await _manageVisitorsRepository.SaveVisitorAsset(vVisitorAsset);
                }

                #endregion

                #region Log History

                int vLogHistory = await _manageVisitorsRepository.SaveVisitorLogHistory(result);

                #endregion

                #region Visitor Approved and Generate Barcode if Creator User IsHOD or IsApprover and visitor MP_IsApproved
                if (parameters.Id == 0)
                {
                    var vUser = await _userRepository.GetUserById(SessionManager.LoggedInUserId);
                    if (vUser != null)
                    {
                        if (vUser.IsHOD == true || vUser.IsApprover == true || parameters.MP_IsApproved == true)
                        {
                            var vVisitor = await _manageVisitorsRepository.GetVisitorsById(result);
                            if (vVisitor != null)
                            {
                                var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vVisitor.VisitNumber);
                                if (vGenerateBarcode.Barcode_Unique_Id != "")
                                {
                                    var vBarcode_Request = new Barcode_Request()
                                    {
                                        Id = 0,
                                        BarcodeNo = vVisitor.VisitNumber,
                                        BarcodeType = "Visitor",
                                        Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                        BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                        BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                        BranchId = vVisitor.BranchId,
                                        RefId = vVisitor.Id
                                    };
                                    var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
                                }

                                //Send Email
                                var vEmailEmp = await SendVisitorApproved_EmailToSecurity(vVisitor.Id);

                                #region SMS Send

                                var vConfigRef_Search = new ConfigRef_Search()
                                {
                                    Ref_Type = "SMS",
                                    Ref_Param = "VisitorApproved"
                                };

                                string sSMSTemplateName = string.Empty;
                                string sSMSTemplateContent = string.Empty;
                                var vConfigRefObj = _configRefRepository.GetConfigRefList(vConfigRef_Search).Result.ToList().FirstOrDefault();
                                if (vConfigRefObj != null)
                                {
                                    sSMSTemplateName = vConfigRefObj.Ref_Value1;
                                    sSMSTemplateContent = vConfigRefObj.Ref_Value2;

                                    if (!string.IsNullOrWhiteSpace(sSMSTemplateContent))
                                    {
                                        //Replace parameter 
                                        sSMSTemplateContent = sSMSTemplateContent.Replace("{#var#}", vVisitor.VisitorName.ToString());
                                        sSMSTemplateContent = sSMSTemplateContent.Replace("{#var1#}", Convert.ToDateTime(vVisitor.VisitStartDate).ToString("dd/MM/yyyy"));
                                        sSMSTemplateContent = sSMSTemplateContent.Replace("{#var2#}", Convert.ToDateTime(vVisitor.VisitStartDate).ToString("hh:mm:ss:tt"));

                                        //StringBuilder sb = new StringBuilder();
                                        //sb.AppendFormat(sSMSTemplateContent, iOTP.ToString(), parameters.VisitNumber);

                                        //sSMSTemplateContent = sb.ToString();
                                    }
                                }

                                // Send SMS
                                var vsmsRequest = new SMS_Request()
                                {
                                    Ref2_Other = vVisitor.VisitNumber,
                                    TemplateName = sSMSTemplateName,
                                    TemplateContent = sSMSTemplateContent,
                                    Mobile = vVisitor.VisitorMobileNo,
                                };
                                bool bSMSResult = await _smsHelper.SMSSend(vsmsRequest);
                                
                                #endregion
                            }
                        }

                        //Notification
                        var vVisitors = await _manageVisitorsRepository.GetVisitorsById(result);
                        if (vVisitors != null)
                        {
                            var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(vVisitors.Id, "Visitor", 0);

                            var visitorGate = "";
                            if (gateNolistObj != null)
                            {
                                visitorGate = string.Join(",", gateNolistObj.ToList().Select(x => x.GateNumber).ToList());
                            }

                            string notifyMessage = "";
                            if (vVisitors.StatusName == "Approved")
                            {
                                notifyMessage = String.Format(@"Pass created for {0} at Gate Number - {1} on {2}.", vVisitors.VisitorName, visitorGate, vVisitors.CreatedDate);
                            }
                            else
                            {
                                notifyMessage = String.Format(@"Pass created for {0} at Gate Number - {1} on {2}.", vVisitors.VisitorName, visitorGate, vVisitors.CreatedDate);
                            }

                            var vSearch = new User_Search()
                            {
                                UserTypeId = 2,
                                BranchId = 0
                            };

                            var vSecurityList = await _userRepository.GetUserList(vSearch); //get branch wise security list

                            var vGateNumberList = await _assignGateNoRepository.GetAssignGateNoById(0, "Security", 0); //get gate list of security
                            var vGateNumberList_1 = vGateNumberList.Where(x => x.GateNumber == "1").ToList();

                            var vSecurityGate_1 = vSecurityList.Where(x => vGateNumberList_1.Select(x => x.RefId).Contains(x.Id)).ToList();
                            if (vSecurityGate_1.Count > 0)
                            {
                                foreach(var vSecurity in vSecurityGate_1)
                                {
                                    var vNotifyObj = new Notification_Request()
                                    {
                                        Subject = "Visitor Created",
                                        SendTo = "Security",
                                        //CustomerId = vWorkOrderObj.CustomerId,
                                        //CustomerMessage = NotifyMessage,
                                        EmployeeId = vSecurity.Id,
                                        EmployeeMessage = notifyMessage,
                                        RefValue1 = vVisitors.VisitNumber,
                                        ReadUnread = false
                                    };

                                    int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Meeting Employee Changed >> Notification
                if (parameters.Id > 0)
                {
                    if (prevBranchId != parameters.BranchId || prevDepartmentId != parameters.DepartmentId || prevEmployeeId != parameters.EmployeeId)
                    {
                        var vSearch = new User_Search()
                        {
                            UserTypeId = 0,
                            BranchId = 0
                        };

                        var vUserList = await _userRepository.GetUserList(vSearch); //get all User list
                        if (vUserList.ToList().Count > 0)
                        {
                            //Notification to Host Employee
                            string notifyMessage_host = String.Format(@"Visitor {0}, with Visitor ID {1} and Mobile Number {2}, Branch, Department, Employee and Gate Number have been changed.", parameters.VisitorName, vVisitNumber, parameters.VisitorMobileNo);
                            var vNotifyObj_host = new Notification_Request()
                            {
                                Subject = "Meeting Employee Changed",
                                SendTo = "Host Employee",
                                //CustomerId = vWorkOrderObj.CustomerId,
                                //CustomerMessage = NotifyMessage,
                                EmployeeId = parameters.EmployeeId,
                                EmployeeMessage = notifyMessage_host,
                                RefValue1 = vVisitNumber,
                                ReadUnread = false
                            };

                            int resultNotification_host = await _notificationRepository.SaveNotification(vNotifyObj_host);

                            //Notification to all security
                            var vSecurityList = vUserList.ToList().Where(x => x.UserTypeId == 2);
                            foreach (var vSecurity in vSecurityList)
                            {
                                string notifyMessage = String.Format(@"Visitor {0}, with Visitor ID {1} and Mobile Number {2}, Branch, Department, Employee and Gate Number have been changed.", parameters.VisitorName, vVisitNumber, parameters.VisitorMobileNo);
                                var vNotifyObj = new Notification_Request()
                                {
                                    Subject = "Meeting Employee Changed",
                                    SendTo = "Security",
                                    //CustomerId = vWorkOrderObj.CustomerId,
                                    //CustomerMessage = NotifyMessage,
                                    EmployeeId = vSecurity.Id,
                                    EmployeeMessage = notifyMessage,
                                    RefValue1 = vVisitNumber,
                                    ReadUnread = false
                                };

                                int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                            }

                            //Notification to HOD Employee
                            var vHODEmployeeList = vUserList.ToList().Where(x => x.BranchId == parameters.BranchId && x.DepartmentId == parameters.DepartmentId && x.IsHOD == true).ToList();
                            foreach(var vHODEmployee in vHODEmployeeList)
                            {
                                string notifyMessage = String.Format(@"Visitor {0}, with Visitor ID {1} and Mobile Number {2}, Branch, Department, Employee and Gate Number have been changed.", parameters.VisitorName, vVisitNumber, parameters.VisitorMobileNo);
                                var vNotifyObj = new Notification_Request()
                                {
                                    Subject = "Meeting Employee Changed",
                                    SendTo = "HOD Employee",
                                    //CustomerId = vWorkOrderObj.CustomerId,
                                    //CustomerMessage = NotifyMessage,
                                    EmployeeId = vHODEmployee.Id,
                                    EmployeeMessage = notifyMessage,
                                    RefValue1 = vVisitNumber,
                                    ReadUnread = false
                                };

                                int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                            }
                        }
                    }
                }
                #endregion
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorsList(Visitors_Search parameters)
        {
            IEnumerable<Visitors_Response> lstVisitorss = await _manageVisitorsRepository.GetVisitorsList(parameters);
            foreach (var user in lstVisitorss)
            {
                var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(user.Id, "Visitor", 0);
                user.GateNumberList = gateNolistObj.ToList();
            }
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorsById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageVisitorsRepository.GetVisitorsById(Id);
                if (vResultObj != null)
                {
                    var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(vResultObj.Id, "Visitor", 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();

                    var vVisitorDocumentVerification = new VisitorDocumentVerification_Search()
                    {
                        RefId = vResultObj.Id,
                        RefType = "Visitor"
                    };

                    var visitorDocumentVerificationlistObj = await _manageVisitorsRepository.GetVisitorDocumentVerificationList(vVisitorDocumentVerification);
                    vResultObj.DocumentVerificationList = visitorDocumentVerificationlistObj.ToList();

                    var vVisitorAsset_Search = new VisitorAsset_Search()
                    {
                        VisitorId = vResultObj.Id,
                    };

                    var visitorAssetlistObj = await _manageVisitorsRepository.GetVisitorAssetList(vVisitorAsset_Search);
                    vResultObj.AssetList = visitorAssetlistObj.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> VisitorsApproveNReject(Visitor_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vVisitorResponse = await _manageVisitorsRepository.GetVisitorsById(Convert.ToInt32(parameters.Id));
                if (parameters.StatusId == 2)
                {
                    if (vVisitorResponse != null)
                    {
                        var vDuplicateBarcode = _barcodeRepository.GetBarcodeById(vVisitorResponse.VisitNumber);
                        if(vDuplicateBarcode == null)
                        {
                            var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vVisitorResponse.VisitNumber);
                            if (vGenerateBarcode.Barcode_Unique_Id != "")
                            {
                                var vBarcode_Request = new Barcode_Request()
                                {
                                    Id = 0,
                                    BarcodeNo = vVisitorResponse.VisitNumber,
                                    BarcodeType = "Visitor",
                                    Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                    BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                    BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                    BranchId = vVisitorResponse.BranchId,
                                    RefId = vVisitorResponse.Id
                                };
                                var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
                            }

                            if (string.IsNullOrEmpty(vGenerateBarcode.BarcodeFileName) && parameters.StatusId == 2)
                            {
                                _response.IsSuccess = false;
                                _response.Message = "Barcode is not generated";

                                return _response;
                            }
                        }
                    }
                }

                int resultExpenseDetails = await _manageVisitorsRepository.VisitorsApproveNReject(parameters);

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
                     _response.Message = "Visitor Approved successfully";

                    //Send Email
                    if (parameters.Id > 0)
                    {
                        var vEmailEmp = await SendVisitorApproved_EmailToSecurity(Convert.ToInt32(parameters.Id));
                    }

                    #region SMS Send

                    if (vVisitorResponse != null)
                    {
                        var vConfigRef_Search = new ConfigRef_Search()
                        {
                            Ref_Type = "SMS",
                            Ref_Param = "VisitorApproved"
                        };

                        string sSMSTemplateName = string.Empty;
                        string sSMSTemplateContent = string.Empty;
                        var vConfigRefObj = _configRefRepository.GetConfigRefList(vConfigRef_Search).Result.ToList().FirstOrDefault();
                        if (vConfigRefObj != null)
                        {
                            sSMSTemplateName = vConfigRefObj.Ref_Value1;
                            sSMSTemplateContent = vConfigRefObj.Ref_Value2;

                            if (!string.IsNullOrWhiteSpace(sSMSTemplateContent))
                            {
                                //Replace parameter 
                                sSMSTemplateContent = sSMSTemplateContent.Replace("{#var#}", vVisitorResponse.VisitorName.ToString());
                                sSMSTemplateContent = sSMSTemplateContent.Replace("{#var1#}", Convert.ToDateTime(vVisitorResponse.VisitStartDate).ToString("dd/MM/yyyy"));
                                sSMSTemplateContent = sSMSTemplateContent.Replace("{#var2#}", Convert.ToDateTime(vVisitorResponse.VisitStartDate).ToString("hh:mm:ss:tt"));

                                //StringBuilder sb = new StringBuilder();
                                //sb.AppendFormat(sSMSTemplateContent, iOTP.ToString(), parameters.VisitNumber);

                                //sSMSTemplateContent = sb.ToString();
                            }
                        }

                        // Send SMS
                        var vsmsRequest = new SMS_Request()
                        {
                            Ref2_Other = vVisitorResponse.VisitNumber,
                            TemplateName = sSMSTemplateName,
                            TemplateContent = sSMSTemplateContent,
                            Mobile = vVisitorResponse.VisitorMobileNo,
                        };
                        bool bSMSResult = await _smsHelper.SMSSend(vsmsRequest);
                    }
                    #endregion
                }
            }

            return _response;
        }

        protected async Task<bool> SendVisitorApproved_EmailToSecurity(int id)
        {
            bool result = false;
            string templateFilePath = "", emailTemplateContent = "", remarks = "", sSubjectDynamicContent = "";

            try
            {
                string recipientEmail = "";
                string visitorGate = string.Empty;

                remarks = id.ToString();

                var vVisitor = await _manageVisitorsRepository.GetVisitorsById(Convert.ToInt32(id));

                if (vVisitor != null)
                {
                    var vGateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(vVisitor.Id, "Visitor", 0);
                    visitorGate = string.Join(",", vGateNolistObj.Select(x => x.GateNumber).ToList());

                    var vSearch = new User_Search()
                    {
                        UserTypeId = 2,
                        BranchId = 0
                    };

                    var vSecurityList = await _userRepository.GetUserList(vSearch); //get branch wise security list

                    var vGateNumberList = await _assignGateNoRepository.GetAssignGateNoById(0,"Security", 0); //get gate list of security
                    var vGateNumberList_1 = vGateNumberList.Where(x => x.GateNumber == "1").ToList();

                    var vSecurityGate_1 = vSecurityList.Where(x => vGateNumberList_1.Select(x => x.RefId).Contains(x.Id)).ToList();
                    if (vSecurityGate_1.Count > 0)
                    {
                        recipientEmail = string.Join(",", vSecurityGate_1.Select(x => x.EmailId).ToList());
                    }

                    templateFilePath = _environment.ContentRootPath + "\\EmailTemplates\\Visitor_Approved_Template.html";
                    emailTemplateContent = System.IO.File.ReadAllText(templateFilePath);

                    if (emailTemplateContent.IndexOf("[VisitID]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[VisitID]", vVisitor.VisitNumber);
                    }

                    if (emailTemplateContent.IndexOf("[PassType]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[PassType]", vVisitor.PassType);
                    }

                    if (emailTemplateContent.IndexOf("[VisitorName]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[VisitorName]", vVisitor.VisitorName);
                    }

                    if (emailTemplateContent.IndexOf("[MobileNo]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[MobileNo]", vVisitor.VisitorMobileNo);
                    }

                    if (emailTemplateContent.IndexOf("[Validity]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[Validity]", vVisitor.VisitEndDate.ToString());
                    }

                    if (emailTemplateContent.IndexOf("[GateNo]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[GateNo]", visitorGate);
                    }

                    if (emailTemplateContent.IndexOf("[Company]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[Company]", string.IsNullOrEmpty(vVisitor.CompanyName) ? "NA" : vVisitor.CompanyName);
                    }

                    if (emailTemplateContent.IndexOf("[Department]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[Department]", vVisitor.DepartmentName);
                    }

                    if (emailTemplateContent.IndexOf("[Host]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[Host]", vVisitor.EmployeeName);
                    }

                    if (emailTemplateContent.IndexOf("[VehicleNumber]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[VehicleNumber]", string.IsNullOrEmpty(vVisitor.VehicleNumber) ? "NA" : vVisitor.VehicleNumber);
                    }

                    if (emailTemplateContent.IndexOf("[VisitPurpose]", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        emailTemplateContent = emailTemplateContent.Replace("[VisitPurpose]", vVisitor.VisitType);
                    }

                    sSubjectDynamicContent = "Visitor Approved - " + vVisitor.VisitorName;
                    result = await _emailHelper.SendEmail(module: "Visitor Approved", subject: sSubjectDynamicContent, sendTo: "Security", content: emailTemplateContent, recipientEmail: recipientEmail, files: null, remarks: remarks);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;

        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorDetailByMobileNumber(string MobileNumber)
        {
            if (MobileNumber == "")
            {
                _response.Message = "Mobile Number is required";
            }
            else
            {
                var vResultObj = await _manageVisitorsRepository.GetVisitorDetailByMobileNumber(MobileNumber);
                if (vResultObj != null)
                {
                    var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(vResultObj.Id, "Visitor", 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();

                    var vVisitorDocumentVerification = new VisitorDocumentVerification_Search()
                    {
                        RefId = vResultObj.Id,
                        RefType = "Visitor"
                    };

                    var visitorDocumentVerificationlistObj = await _manageVisitorsRepository.GetVisitorDocumentVerificationList(vVisitorDocumentVerification);
                    vResultObj.DocumentVerificationList = visitorDocumentVerificationlistObj.ToList();

                    var vVisitorAsset_Search = new VisitorAsset_Search()
                    {
                        VisitorId = vResultObj.Id,
                    };

                    var visitorAssetlistObj = await _manageVisitorsRepository.GetVisitorAssetList(vVisitorAsset_Search);
                    vResultObj.AssetList = visitorAssetlistObj.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorApproveNRejectHistoryListById(VisitorApproveNRejectHistory_Search parameters)
        {
            IEnumerable<VisitorApproveNRejectHistory_Response> lstVisitorss = await _manageVisitorsRepository.GetVisitorApproveNRejectHistoryListById(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorLogHistoryList(VisitorLogHistory_Search parameters)
        {
            IEnumerable<VisitorLogHistory_Response> lstVisitorss = await _manageVisitorsRepository.GetVisitorLogHistoryList(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorPlannedList(VisitorPlanned_Search parameters)
        {
            IEnumerable<VisitorPlanned_Response> lstVisitorss = await _manageVisitorsRepository.GetVisitorPlannedList(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVisitorCheckedInOut(VisitorCheckedInOut_Request parameters)
        {
            if(parameters.GateDetailsId == 0)
            {
                _response.IsSuccess = false;
                _response.Message = "Gate Details is required.";

                return _response;
            }

            int result = await _manageVisitorsRepository.SaveVisitorCheckedInOut(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.IsSuccess = false;
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.IsSuccess = false;
                _response.Message = "Record is already exists";
            }
            else if (result == -3)
            {
                _response.IsSuccess = false;
                _response.Message = "Permission from previous gate is required.";
            }
            else if (result == -4)
            {
                _response.Message = "Already checked In for this gate.";
            }
            else if (result == -5)
            {
                _response.Message = "Already Checked Out for this gate.";
            }
            else if (result == -6)
            {
                _response.Message = "This gate is not assigned for you.";
            }
            //else if (result == -7)
            //{
            //    _response.Message = "Not Allow to Checked Out from this gate";
            //}
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.IsSuccess = false;
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
        public async Task<ResponseModel> GetCheckedInOutLogHistoryList(CheckedInOutLogHistory_Search parameters)
        {
            IEnumerable<CheckedInOutLogHistory_Response> lstVisitorss = await _manageVisitorsRepository.GetCheckedInOutLogHistoryList(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorMobileNoListForSelectList()
        {
            IEnumerable<SelectListResponse> lstResponse = await _manageVisitorsRepository.GetVisitorMobileNoListForSelectList();
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> VisitorOTPGenerate(VisitorOTPVerify parameters)
        {
            var vOTPRequestModelObj = new OTPRequestModel()
            {
                MobileNumber = parameters.MobileNo
            };

            int iOTP = Utilities.GenerateRandomNumForOTP();
            if (iOTP > 0)
            {
                vOTPRequestModelObj.OTP = Convert.ToString(iOTP);
            }

            // Opt save
            int resultOTP = await _loginRepository.SaveOTP(vOTPRequestModelObj);

            if (resultOTP > 0)
            {
                _response.Message = "OTP sent successfully.";

                #region SMS Send

                var vConfigRef_Search = new ConfigRef_Search()
                {
                    Ref_Type = "SMS",
                    Ref_Param = "OTPForVisitor"
                };

                string sSMSTemplateName = string.Empty;
                string sSMSTemplateContent = string.Empty;
                var vConfigRefObj = _configRefRepository.GetConfigRefList(vConfigRef_Search).Result.ToList().FirstOrDefault();
                if (vConfigRefObj != null)
                {
                    sSMSTemplateName = vConfigRefObj.Ref_Value1;
                    sSMSTemplateContent = vConfigRefObj.Ref_Value2;

                    if (!string.IsNullOrWhiteSpace(sSMSTemplateContent))
                    {
                        //Replace parameter 
                        sSMSTemplateContent = sSMSTemplateContent.Replace("{#var#}", iOTP.ToString());
                        //sSMSTemplateContent = sSMSTemplateContent.Replace("{#var1#}", parameters.VisitNumber);

                        //StringBuilder sb = new StringBuilder();
                        //sb.AppendFormat(sSMSTemplateContent, iOTP.ToString(), parameters.VisitNumber);

                        //sSMSTemplateContent = sb.ToString();
                    }
                }

                if (parameters != null)
                {
                    // Send SMS
                    var vsmsRequest = new SMS_Request()
                    {
                        Ref1_OTPId = resultOTP,
                        TemplateName = sSMSTemplateName,
                        TemplateContent = sSMSTemplateContent,
                        Mobile = parameters.MobileNo,
                    };
                    bool bSMSResult = await _smsHelper.SMSSend(vsmsRequest);

                }
                _response.Id = resultOTP;

                #endregion
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPreviousVisitorList(PreviousVisitor_Search parameters)
        {
            IEnumerable<PreviousVisitor_Response> lstVisitorss = await _manageVisitorsRepository.GetPreviousVisitorList(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMeetingPurposeLogHistoryList(MeetingPurposeLogHistory_Search parameters)
        {
            IEnumerable<MeetingPurposeLogHistory_Response> lstVisitorss = await _manageVisitorsRepository.GetMeetingPurposeLogHistoryList(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> DeleteVisitorDocumentVerification(int Id)
        {
            int result = await _manageVisitorsRepository.DeleteVisitorDocumentVerification(Id);

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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportVisitorAttendanceData(Visitors_Search parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Visitors_Response> lstSizeObj = await _manageVisitorsRepository.GetVisitorsList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("VisitorAttendance");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Sr.No";
                    WorkSheet1.Cells[1, 2].Value = "Visit ID";
                    WorkSheet1.Cells[1, 3].Value = "Visit Name";
                    WorkSheet1.Cells[1, 4].Value = "Visit Purpose";
                    WorkSheet1.Cells[1, 5].Value = "Branch";
                    WorkSheet1.Cells[1, 6].Value = "Department";
                    WorkSheet1.Cells[1, 7].Value = "Gate No";
                    WorkSheet1.Cells[1, 8].Value = "Status";
                    WorkSheet1.Cells[1, 9].Value = "Remark";
                    WorkSheet1.Cells[1, 10].Value = "Created Date";
                    WorkSheet1.Cells[1, 11].Value = "Created By";

                    recordIndex = 2;

                    int i = 1;

                    foreach (var items in lstSizeObj)
                    {
                        //log history list
                        var vCheckedInOutLogHistory_Search = new CheckedInOutLogHistory_Search();
                        vCheckedInOutLogHistory_Search.RefId = items.Id;
                        vCheckedInOutLogHistory_Search.RefType = "Visitor";
                        vCheckedInOutLogHistory_Search.GateDetailsId = 0;
                        vCheckedInOutLogHistory_Search.IsReject = null;

                        int j = 0;
                        IEnumerable<CheckedInOutLogHistory_Response> lstMUserObj = await _manageVisitorsRepository.GetCheckedInOutLogHistoryList(vCheckedInOutLogHistory_Search);
                        if (lstMUserObj.ToList().Count > 0)
                        {
                            foreach (var mitems in lstMUserObj.ToList().OrderBy(x=>x.Id))
                            {
                                if (j == 0)
                                {
                                    WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                                }
                                else
                                {
                                    WorkSheet1.Cells[recordIndex, 1].Value = i + "." + j;
                                }
                                WorkSheet1.Cells[recordIndex, 2].Value = items.VisitNumber;
                                WorkSheet1.Cells[recordIndex, 3].Value = items.VisitorName;
                                WorkSheet1.Cells[recordIndex, 4].Value = items.VisitType;
                                WorkSheet1.Cells[recordIndex, 5].Value = items.BranchName;
                                WorkSheet1.Cells[recordIndex, 6].Value = items.DepartmentName;
                                WorkSheet1.Cells[recordIndex, 7].Value = mitems.GateNumber;
                                WorkSheet1.Cells[recordIndex, 8].Value = mitems.CheckedStatus;
                                WorkSheet1.Cells[recordIndex, 9].Value = mitems.CheckedRemark;
                                WorkSheet1.Cells[recordIndex, 10].Value = Convert.ToDateTime(mitems.CreatedDate).ToString("dd/MM/yyyy");
                                WorkSheet1.Cells[recordIndex, 11].Value = mitems.CreatorName;

                                recordIndex += 1;

                                j++;
                            }
                        }
                        else
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                            WorkSheet1.Cells[recordIndex, 2].Value = items.VisitNumber;
                            WorkSheet1.Cells[recordIndex, 3].Value = items.VisitorName;
                            WorkSheet1.Cells[recordIndex, 4].Value = items.VisitType;
                            WorkSheet1.Cells[recordIndex, 5].Value = items.BranchName;
                            WorkSheet1.Cells[recordIndex, 6].Value = items.DepartmentName;

                            recordIndex += 1;
                        }

                        //meeting log
                        var vMeetingPurposeLogHistory_Search = new MeetingPurposeLogHistory_Search();
                        vMeetingPurposeLogHistory_Search.VisitorId = items.Id;

                        int k = 1;
                        IEnumerable<MeetingPurposeLogHistory_Response> lstMeetingObj = await _manageVisitorsRepository.GetMeetingPurposeLogHistoryList(vMeetingPurposeLogHistory_Search);
                        foreach (var mitems in lstMeetingObj)
                        {
                            if (j > 0)
                            {
                                WorkSheet1.Cells[recordIndex, 1].Value = i + "." + j + "." + k;
                            }
                            else
                            {
                                WorkSheet1.Cells[recordIndex, 1].Value = i + "." + k;
                            }
                            WorkSheet1.Cells[recordIndex, 2].Value = items.VisitNumber;
                            WorkSheet1.Cells[recordIndex, 3].Value = items.VisitorName;
                            WorkSheet1.Cells[recordIndex, 4].Value = items.VisitType;
                            WorkSheet1.Cells[recordIndex, 5].Value = items.BranchName;
                            WorkSheet1.Cells[recordIndex, 6].Value = items.DepartmentName;
                            WorkSheet1.Cells[recordIndex, 7].Value = mitems.GateNumber;
                            WorkSheet1.Cells[recordIndex, 8].Value = ((mitems.IsMeetingOver == false || mitems.IsMeetingOver == null) ? "Reassign" : "Meeting Over");
                            WorkSheet1.Cells[recordIndex, 9].Value = "";
                            WorkSheet1.Cells[recordIndex, 10].Value = Convert.ToDateTime(mitems.CreatedDate).ToString("dd/MM/yyyy");
                            WorkSheet1.Cells[recordIndex, 11].Value = mitems.CreatorName;

                            recordIndex += 1;

                            k++;
                        }

                        i++;
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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> DownloadVisitorTemplate()
        {
            byte[]? formatFile = await Task.Run(() => _fileManager.GetFormatFileFromPath("Template_Visitor.xlsx"));

            if (formatFile != null)
            {
                _response.Data = formatFile;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportVisitor([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;

            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            List<string[]> data = new List<string[]>();
            List<Visitor_ImportData> lstVisitor_ImportData = new List<Visitor_ImportData>();
            IEnumerable<Visitor_ImportDataValidation> lstVisitor_ImportDataValidation;

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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "PassType", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "VisitStartDate", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "VisitEndDate", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "VisitorMobileNo", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "VisitorName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 6].Value.ToString(), "VisitorEmailId", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 7].Value.ToString(), "Gender", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 8].Value.ToString(), "Country", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 9].Value.ToString(), "State", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 10].Value.ToString(), "Province", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 11].Value.ToString(), "Pincode", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 12].Value.ToString(), "Address", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 13].Value.ToString(), "IsCompany", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 14].Value.ToString(), "VisitorCompany", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 15].Value.ToString(), "VisitPurpose", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 16].Value.ToString(), "Branch", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 17].Value.ToString(), "Department", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 18].Value.ToString(), "GateNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 19].Value.ToString(), "EmployeeName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 20].Value.ToString(), "IsVehicle", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 21].Value.ToString(), "VehicleNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 22].Value.ToString(), "VehicleType", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 23].Value.ToString(), "IsDrivingLicense", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 24].Value.ToString(), "IsPUC", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 25].Value.ToString(), "IsInsurance", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 26].Value.ToString(), "Remarks", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    if (!string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 2].Value?.ToString()) && !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 3].Value?.ToString()))
                    {
                        lstVisitor_ImportData.Add(new Visitor_ImportData()
                        {
                            PassType = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                            VisitStartDate = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 2].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 2].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            VisitEndDate = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 3].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 3].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            VisitorMobileNo = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                            VisitorName = workSheet.Cells[rowIterator, 5].Value?.ToString(),
                            VisitorEmailId = workSheet.Cells[rowIterator, 6].Value?.ToString(),
                            Gender = workSheet.Cells[rowIterator, 7].Value?.ToString(),
                            Country = workSheet.Cells[rowIterator, 8].Value?.ToString(),
                            State = workSheet.Cells[rowIterator, 9].Value?.ToString(),
                            Province = workSheet.Cells[rowIterator, 10].Value?.ToString(),
                            Pincode = workSheet.Cells[rowIterator, 11].Value?.ToString(),
                            Address = workSheet.Cells[rowIterator, 12].Value?.ToString(),
                            IsCompany = workSheet.Cells[rowIterator, 13].Value?.ToString(),
                            VisitorCompany = workSheet.Cells[rowIterator, 14].Value?.ToString(),
                            VisitPurpose = workSheet.Cells[rowIterator, 15].Value?.ToString(),
                            Branch = workSheet.Cells[rowIterator, 16].Value?.ToString(),
                            Department = workSheet.Cells[rowIterator, 17].Value?.ToString(),
                            GateNumber = workSheet.Cells[rowIterator, 18].Value?.ToString(),
                            EmployeeName = workSheet.Cells[rowIterator, 19].Value?.ToString(),
                            IsVehicle = workSheet.Cells[rowIterator, 20].Value?.ToString(),
                            VehicleNumber = workSheet.Cells[rowIterator, 21].Value?.ToString(),
                            VehicleType = workSheet.Cells[rowIterator, 22].Value?.ToString(),

                            IsDrivingLicense = workSheet.Cells[rowIterator, 23].Value?.ToString(),
                            IsPUC = workSheet.Cells[rowIterator, 24].Value?.ToString(),
                            IsInsurance = workSheet.Cells[rowIterator, 25].Value?.ToString(),
                            Remarks = workSheet.Cells[rowIterator, 26].Value?.ToString()
                        }); ;
                    }
                }
            }

            if (lstVisitor_ImportData.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstVisitor_ImportDataValidation = await _manageVisitorsRepository.ImportVisitor(lstVisitor_ImportData);

            #region Generate Excel file for Invalid Data

            if (lstVisitor_ImportDataValidation.ToList().Count > 0 && lstVisitor_ImportDataValidation.ToList().FirstOrDefault().VisitStartDate != null)
            {
                _response.IsSuccess = false;
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidImportDataFile(lstVisitor_ImportDataValidation);
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Record imported successfully";
            }

            #endregion

            return _response;
        }

        private byte[] GenerateInvalidImportDataFile(IEnumerable<Visitor_ImportDataValidation> lstVisitor_ImportDataValidation)
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

                    WorkSheet1.Cells[1, 1].Value = "PassType";
                    WorkSheet1.Cells[1, 2].Value = "VisitStartDate";
                    WorkSheet1.Cells[1, 3].Value = "VisitEndDate";
                    WorkSheet1.Cells[1, 4].Value = "VisitorMobileNo";
                    WorkSheet1.Cells[1, 5].Value = "VisitorName";
                    WorkSheet1.Cells[1, 6].Value = "VisitorEmailId";

                    WorkSheet1.Cells[1, 7].Value = "Gender";
                    WorkSheet1.Cells[1, 8].Value = "Country";
                    WorkSheet1.Cells[1, 9].Value = "State";
                    WorkSheet1.Cells[1, 10].Value = "Province";
                    WorkSheet1.Cells[1, 11].Value = "Pincode";
                    WorkSheet1.Cells[1, 12].Value = "Address";
                    WorkSheet1.Cells[1, 13].Value = "IsCompany";
                    WorkSheet1.Cells[1, 14].Value = "VisitorCompany";

                    WorkSheet1.Cells[1, 15].Value = "VisitPurpose";
                    WorkSheet1.Cells[1, 16].Value = "Branch";
                    WorkSheet1.Cells[1, 17].Value = "Department";
                    WorkSheet1.Cells[1, 18].Value = "GateNumber";
                    WorkSheet1.Cells[1, 19].Value = "EmployeeName";

                    WorkSheet1.Cells[1, 20].Value = "IsVehicle";
                    WorkSheet1.Cells[1, 21].Value = "VehicleNumber";
                    WorkSheet1.Cells[1, 22].Value = "VehicleType";

                    WorkSheet1.Cells[1, 23].Value = "IsDrivingLicense";
                    WorkSheet1.Cells[1, 24].Value = "IsPUC";
                    WorkSheet1.Cells[1, 25].Value = "IsInsurance";
                    WorkSheet1.Cells[1, 26].Value = "Remarks";
                    WorkSheet1.Cells[1, 27].Value = "ErrorMessage";

                    recordIndex = 2;

                    foreach (Visitor_ImportDataValidation record in lstVisitor_ImportDataValidation)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.PassType;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.VisitStartDate;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.VisitEndDate;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.VisitorMobileNo;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.VisitorName;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.VisitorEmailId;

                        WorkSheet1.Cells[recordIndex, 7].Value = record.Gender;
                        WorkSheet1.Cells[recordIndex, 8].Value = record.Country;
                        WorkSheet1.Cells[recordIndex, 9].Value = record.State;
                        WorkSheet1.Cells[recordIndex, 10].Value = record.Province;
                        WorkSheet1.Cells[recordIndex, 11].Value = record.Pincode;

                        WorkSheet1.Cells[recordIndex, 12].Value = record.Address;
                        WorkSheet1.Cells[recordIndex, 13].Value = record.IsCompany;
                        WorkSheet1.Cells[recordIndex, 14].Value = record.VisitorCompany;

                        WorkSheet1.Cells[recordIndex, 15].Value = record.VisitPurpose;
                        WorkSheet1.Cells[recordIndex, 16].Value = record.Branch;
                        WorkSheet1.Cells[recordIndex, 17].Value = record.Department;
                        WorkSheet1.Cells[recordIndex, 18].Value = record.GateNumber;
                        WorkSheet1.Cells[recordIndex, 19].Value = record.EmployeeName;

                        WorkSheet1.Cells[recordIndex, 20].Value = record.IsVehicle;
                        WorkSheet1.Cells[recordIndex, 21].Value = record.VehicleNumber;
                        WorkSheet1.Cells[recordIndex, 22].Value = record.VehicleType;

                        WorkSheet1.Cells[recordIndex, 23].Value = record.IsDrivingLicense;
                        WorkSheet1.Cells[recordIndex, 24].Value = record.IsPUC;
                        WorkSheet1.Cells[recordIndex, 25].Value = record.IsInsurance;
                        WorkSheet1.Cells[recordIndex, 26].Value = record.Remarks;
                        WorkSheet1.Cells[recordIndex, 27].Value = record.ValidationMessage;

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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportVisitorData(Visitors_Search parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Visitors_Response> lstSizeObj = await _manageVisitorsRepository.GetVisitorsList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Visitor");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Sr.No";
                    WorkSheet1.Cells[1, 2].Value = "Visit ID";
                    WorkSheet1.Cells[1, 3].Value = "Visitor Name";
                    WorkSheet1.Cells[1, 4].Value = "Visitor Mobile Number";
                    WorkSheet1.Cells[1, 5].Value = "Pass Type";
                    WorkSheet1.Cells[1, 6].Value = "Visit Start Date & Time";
                    WorkSheet1.Cells[1, 7].Value = "Visit End Date & Time";
                    WorkSheet1.Cells[1, 8].Value = "Visitor Email Id";
                    WorkSheet1.Cells[1, 9].Value = "Gender";
                    WorkSheet1.Cells[1, 10].Value = "Visitor Company";
                    WorkSheet1.Cells[1, 11].Value = "Address";
                    WorkSheet1.Cells[1, 12].Value = "Country";
                    WorkSheet1.Cells[1, 13].Value = "State";
                    WorkSheet1.Cells[1, 14].Value = "Province";
                    WorkSheet1.Cells[1, 15].Value = "Pincode";
                    WorkSheet1.Cells[1, 16].Value = "Meeting Purpose";
                    WorkSheet1.Cells[1, 17].Value = "Branch";
                    WorkSheet1.Cells[1, 18].Value = "Department";
                    WorkSheet1.Cells[1, 19].Value = "Gate Number";
                    WorkSheet1.Cells[1, 20].Value = "Employee Name";
                    WorkSheet1.Cells[1, 21].Value = "Employee Mobile Number";
                    WorkSheet1.Cells[1, 22].Value = "Employee EmailId";
                    WorkSheet1.Cells[1, 23].Value = "Is Vehicle";
                    WorkSheet1.Cells[1, 24].Value = "Vehicle Number";
                    WorkSheet1.Cells[1, 25].Value = "Vehicle Type";
                    WorkSheet1.Cells[1, 26].Value = "Is Driving Licence";
                    WorkSheet1.Cells[1, 27].Value = "Is PUC";
                    WorkSheet1.Cells[1, 28].Value = "Is Insurance";
                    WorkSheet1.Cells[1, 29].Value = "Remarks";
                    WorkSheet1.Cells[1, 30].Value = "Status";
                    WorkSheet1.Cells[1, 31].Value = "Is Active";
                    WorkSheet1.Cells[1, 32].Value = "Created Date";
                    WorkSheet1.Cells[1, 33].Value = "Created By";

                    recordIndex = 2;

                    int i = 1;

                    foreach (var items in lstSizeObj)
                    {
                        var vResultObj = await _userRepository.GetUserById(items.Id);

                        string strGateNumberList = string.Empty;
                        var vSecurityGateDetail = await _assignGateNoRepository.GetAssignGateNoById(RefId: Convert.ToInt32(items.Id), "Visitor", GateDetailsId: 0);
                        if (vSecurityGateDetail.ToList().Count > 0)
                        {
                            strGateNumberList = string.Join(",", vSecurityGateDetail.ToList().Select(x => x.GateNumber));
                        }

                        WorkSheet1.Cells[recordIndex, 1].Value = i;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.VisitNumber;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.VisitorName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.VisitorMobileNo;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.PassType;
                       
                        WorkSheet1.Cells[recordIndex, 6].Value = items.VisitStartDate.HasValue? items.VisitStartDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.VisitEndDate.HasValue ? items.VisitEndDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.VisitorEmailId;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.GenderName;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.CompanyName;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.AddressLine;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 16].Value = items.MeetingType;
                        WorkSheet1.Cells[recordIndex, 17].Value = items.BranchName;
                        WorkSheet1.Cells[recordIndex, 18].Value = items.DepartmentName;
                        WorkSheet1.Cells[recordIndex, 19].Value = strGateNumberList;
                        WorkSheet1.Cells[recordIndex, 20].Value = items.EmployeeName;
                        WorkSheet1.Cells[recordIndex, 21].Value = items.Employee_MobileNumber;
                        WorkSheet1.Cells[recordIndex, 22].Value = items.Employee_EmailId;
                        WorkSheet1.Cells[recordIndex, 23].Value = items.IsVehicle;
                        WorkSheet1.Cells[recordIndex, 24].Value = items.VehicleNumber;
                        WorkSheet1.Cells[recordIndex, 25].Value = items.VehicleType;
                        WorkSheet1.Cells[recordIndex, 26].Value = items.IsDrivingLicense;
                        WorkSheet1.Cells[recordIndex, 27].Value = items.IsPUC;
                        WorkSheet1.Cells[recordIndex, 28].Value = items.IsInsurance;
                        WorkSheet1.Cells[recordIndex, 29].Value = items.Remarks;
                        WorkSheet1.Cells[recordIndex, 30].Value = items.StatusName;

                        WorkSheet1.Cells[recordIndex, 31].Value = items.IsActive == true ? "Active" : "Inactive";
                        WorkSheet1.Cells[recordIndex, 32].Value = Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 33].Value = items.CreatorName;

                        recordIndex += 1;
                        i++;
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

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> AutoDailyReport(string refType = "Visitor", string? JobType = "")
        {
            if (refType == "Visitor")
            {
                IEnumerable<AutoDailyReport_Response> lst = await _manageVisitorsRepository.AutoDailyReport();

                //CHOWGULE LAVGAN
                var vCHOWGULE = lst.ToList().Where(x => x.BranchName == "CHOWGULE LAVGAN").ToList();
                string file1 = CreateExcel(vCHOWGULE, null, null, "CHOWGULE LAVGAN", refType);

                //ANGRE PORT
                var vANGRE = lst.ToList().Where(x => x.BranchName == "ANGRE PORT").ToList();
                string file2 = CreateExcel(vANGRE, null, null, "ANGRE PORT", refType);

                //FIBER GLASS
                var vFIBER = lst.ToList().Where(x => x.BranchName == "FIBER GLASS").ToList();
                string file3 = CreateExcel(vFIBER, null, null, "FIBER GLASS", refType);
            }
            else if (refType == "Worker")
            {
                IEnumerable<AutoDailyReport_Worker_Response> lst = await _manageVisitorsRepository.AutoDailyReport_Worker(JobType);

                //CHOWGULE LAVGAN
                var vCHOWGULE = lst.ToList().Where(x => x.BranchName == "CHOWGULE LAVGAN").ToList();
                string file1 = CreateExcel(null,vCHOWGULE, null, "CHOWGULE LAVGAN", refType, JobType);

                //ANGRE PORT
                var vANGRE = lst.ToList().Where(x => x.BranchName == "ANGRE PORT").ToList();
                string file2 = CreateExcel(null,vANGRE, null, "ANGRE PORT", refType, JobType);

                //FIBER GLASS
                var vFIBER = lst.ToList().Where(x => x.BranchName == "FIBER GLASS").ToList();
                string file3 = CreateExcel(null,vFIBER, null, "FIBER GLASS", refType, JobType);
            }
            else if (refType == "Employee")
            {
                IEnumerable<AutoDailyReport_Employee_Response> lst = await _manageVisitorsRepository.AutoDailyReport_Employee(JobType);

                //CHOWGULE LAVGAN
                var vCHOWGULE = lst.ToList().Where(x => x.BranchName == "CHOWGULE LAVGAN").ToList();
                string file1 = CreateExcel(null, null, vCHOWGULE, "CHOWGULE LAVGAN", refType, JobType);

                //ANGRE PORT
                var vANGRE = lst.ToList().Where(x => x.BranchName == "ANGRE PORT").ToList();
                string file2 = CreateExcel(null, null, vANGRE, "ANGRE PORT", refType, JobType);

                //FIBER GLASS
                var vFIBER = lst.ToList().Where(x => x.BranchName == "FIBER GLASS").ToList();
                string file3 = CreateExcel(null, null, vFIBER, "FIBER GLASS", refType, JobType);
            }

            //Send Email
            var vEmailEmp = await SendDailyReport_EmailToSecurity(refType, JobType);
            if (vEmailEmp)
            {
                _response.Message = "Daily Report send successfully";
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Daily Report not send successfully";
            }

            return _response;
        }

        private string CreateExcel(List<AutoDailyReport_Response> parameter, List<AutoDailyReport_Worker_Response> wParameter, List<AutoDailyReport_Employee_Response> empParameter, string branchName, string refType = "Visitor", string JobType = "")
        {
            var fileName = string.Empty;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add(branchName);

                var headerRow = new List<string[]>();
                if (refType == "Visitor")
                {
                    headerRow.Add(new string[] { "VisitorID", "Date", "VisitorName", "VisitorCompany", "VisitorMobileNo", "HostDepartment", "HostName", "GateNumber", "CheckInTime", "CheckOutTime", "Remark" });

                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                    var worksheet = excel.Workbook.Worksheets[branchName];
                    var stream = excel.Stream;
                    var count = parameter.Count();
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var fileN = string.Empty;

                    for (int i = 2; i <= count + 1;)
                    {
                        foreach (var item in parameter)
                        {
                            worksheet.Cells["A" + i].Value = item.VisitNumber;
                            worksheet.Cells["B" + i].Value = item.VisitDate;
                            worksheet.Cells["C" + i].Value = item.VisitorName;
                            worksheet.Cells["D" + i].Value = item.VisitorCompany;
                            worksheet.Cells["E" + i].Value = item.VisitorMobileNo;
                            worksheet.Cells["F" + i].Value = item.HostDepartment;
                            worksheet.Cells["G" + i].Value = item.HostName;
                            worksheet.Cells["H" + i].Value = item.GateNumber;
                            worksheet.Cells["I" + i].Value = item.CheckInTime;
                            worksheet.Cells["J" + i].Value = item.CheckOutTime;
                            worksheet.Cells["K" + i].Value = item.Remarks;
                            i++;
                        }
                    }
                    worksheet.Columns.AutoFit();
                }
                else if (refType == "Worker")
                {
                    headerRow.Add(new string[] { "Date", "GateNumber", "ContractorCompanyName", "WorkerName", "WorkerId", "WorkerType", "MobileNumber", "CheckInTime", "CheckOutTime" });

                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                    var worksheet = excel.Workbook.Worksheets[branchName];
                    var stream = excel.Stream;
                    var count = wParameter.Count();
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var fileN = string.Empty;

                    for (int i = 2; i <= count + 1;)
                    {
                        foreach (var item in wParameter)
                        {
                            worksheet.Cells["A" + i].Value = item.VisitDate;
                            worksheet.Cells["B" + i].Value = item.GateNumber;
                            worksheet.Cells["C" + i].Value = item.ContractorName;
                            worksheet.Cells["D" + i].Value = item.WorkerName;
                            worksheet.Cells["E" + i].Value = item.WorkerId;
                            worksheet.Cells["F" + i].Value = item.WorkerType;
                            worksheet.Cells["G" + i].Value = item.WorkerMobileNo;
                            worksheet.Cells["H" + i].Value = item.CheckInTime;
                            worksheet.Cells["I" + i].Value = item.CheckOutTime;
                            i++;
                        }
                    }
                    worksheet.Columns.AutoFit();
                }
                else if (refType == "Employee")
                {
                    headerRow.Add(new string[] { "Date", "GateNumber", "EmployeeName", "ContactNumber", "EmployeeCode", "Department", "Role", "CheckInTime", "CheckOutTime" });

                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                    var worksheet = excel.Workbook.Worksheets[branchName];
                    var stream = excel.Stream;
                    var count = empParameter.Count();
                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var fileN = string.Empty;

                    for (int i = 2; i <= count + 1;)
                    {
                        foreach (var item in empParameter)
                        {
                            worksheet.Cells["A" + i].Value = item.VisitDate;
                            worksheet.Cells["B" + i].Value = item.GateNumber;
                            worksheet.Cells["C" + i].Value = item.EmployeeName;
                            worksheet.Cells["D" + i].Value = item.ContactNumber;
                            worksheet.Cells["E" + i].Value = item.EmployeeCode;
                            worksheet.Cells["F" + i].Value = item.DepartmentName;
                            worksheet.Cells["G" + i].Value = item.RoleName;
                            worksheet.Cells["H" + i].Value = item.CheckInTime;
                            worksheet.Cells["I" + i].Value = item.CheckOutTime;
                            i++;
                        }
                    }
                    worksheet.Columns.AutoFit();
                }

                fileName = "DailyReport_" + branchName + "_" + refType + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
                string path = "";

                if (JobType != "")
                {
                    path = _environment.ContentRootPath + "\\Uploads\\DailyReportFile\\" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + JobType + "\\";
                }
                else
                {
                    path = _environment.ContentRootPath + "\\Uploads\\DailyReportFile\\" + DateTime.Now.ToString("dd-MM-yyyy") + "\\";
                }
                

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                //if (System.IO.File.Exists(path + fileName))
                //{
                //    System.IO.File.Delete(path + fileName);
                //}

                excel.SaveAs(new FileInfo(path + fileName));
            }
            return fileName;
        }

        protected async Task<bool> SendDailyReport_EmailToSecurity(string refType, string JobType = "")
        {
            bool result = false;
            string templateFilePath = "", emailTemplateContent = "", sSubjectDynamicContent = "";

            try
            {
                string recipientEmail = "";

                var vSearch = new User_Search()
                {
                    UserTypeId = 2,
                    BranchId = 0
                };

                var vSecurityList = await _userRepository.GetUserList(vSearch); //get branch wise security list

                var vGateNumberList = await _assignGateNoRepository.GetAssignGateNoById(0, "Security", 0); //get gate list of security
                var vGateNumberList_1 = vGateNumberList.Where(x => x.GateNumber == "1").ToList();

                var vSecurityGate_1 = vSecurityList.Where(x => vGateNumberList_1.Select(x => x.RefId).Contains(x.Id)).ToList();
                if (vSecurityGate_1.Count > 0)
                {
                    recipientEmail = string.Join(",", vSecurityGate_1.Select(x => x.EmailId).ToList());
                }

                if (refType == "Visitor")
                {
                    templateFilePath = _environment.ContentRootPath + "\\EmailTemplates\\AutoDailyReport_Visitor_Template.html";
                    emailTemplateContent = System.IO.File.ReadAllText(templateFilePath);
                }
                else if (refType == "Worker")
                {
                    templateFilePath = _environment.ContentRootPath + "\\EmailTemplates\\AutoDailyReport_Worker_Template.html";
                    emailTemplateContent = System.IO.File.ReadAllText(templateFilePath);
                }
                else if (refType == "Employee")
                {
                    templateFilePath = _environment.ContentRootPath + "\\EmailTemplates\\AutoDailyReport_Employee_Template.html";
                    emailTemplateContent = System.IO.File.ReadAllText(templateFilePath);
                }
               

                if (emailTemplateContent.IndexOf("[Date]", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    emailTemplateContent = emailTemplateContent.Replace("[Date]", DateTime.Now.ToString("dd/MM/yyyy"));
                }

                string basePath = "";
                string moduleName = "";
                if (JobType != "")
                {
                    basePath = _environment.ContentRootPath + "\\Uploads\\DailyReportFile\\" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + JobType + "\\";
                    moduleName = "Auto Daily Report - " + JobType;
                }
                else
                {
                    basePath = _environment.ContentRootPath + "\\Uploads\\DailyReportFile\\" + DateTime.Now.ToString("dd-MM-yyyy") + "\\";
                    moduleName = "Auto Daily Report";
                }


                var vAtt = basePath + "DailyReport_CHOWGULE LAVGAN_" + refType + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
                var vAtt1 = basePath + "DailyReport_ANGRE PORT_" + refType + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";
                var vAtt2 = basePath + "DailyReport_FIBER GLASS_" + refType + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";

                var vfiles = new List<Attachment>()
                {
                    new Attachment(vAtt),
                    new Attachment(vAtt1),
                    new Attachment(vAtt2)
                };
                    

                sSubjectDynamicContent = refType + " Report - " + DateTime.Now.ToShortDateString();
                result = await _emailHelper.SendEmail(module: moduleName, subject: sSubjectDynamicContent, sendTo: "Security", content: emailTemplateContent, recipientEmail: recipientEmail, files: vfiles, remarks: "");
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;

        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVisitorCheckedInOut_Offline(VisitorCheckedInOut_Offline_Request parameters)
        {
            if (parameters.GateDetailsId == 0)
            {
                _response.IsSuccess = false;
                _response.Message = "Gate Details is required.";

                return _response;
            }
            else if (parameters.CheckedInDate == null)
            {
                _response.IsSuccess = false;
                _response.Message = "CheckedIn Date is required.";

                return _response;
            }
            else if (parameters.CheckedOutDate == null && parameters.IsCheckedIn_Out == 2)
            {
                _response.IsSuccess = false;
                _response.Message = "CheckedOut Date is required.";

                return _response;
            }

            int result = await _manageVisitorsRepository.SaveVisitorCheckedInOut_Offline(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.IsSuccess = false;
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.IsSuccess = false;
                _response.Message = "Record is already exists";
            }
            else if (result == -3)
            {
                _response.IsSuccess = false;
                _response.Message = "Permission from previous gate is required.";
            }
            else if (result == -4)
            {
                _response.Message = "Already checked In for this gate.";
            }
            else if (result == -5)
            {
                _response.Message = "Already Checked Out for this gate.";
            }
            else if (result == -6)
            {
                _response.Message = "This gate is not assigned for you.";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.IsSuccess = false;
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
        [AllowAnonymous]
        public async Task<ResponseModel> DeleteVisitorAsset(int Id)
        {
            int result = await _manageVisitorsRepository.DeleteVisitorAsset(Id);

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
    }
}