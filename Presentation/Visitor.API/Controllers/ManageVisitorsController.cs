﻿using Dapper;
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
                        VisitorId = result,
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
                        VisitorId = vResultObj.Id
                    };

                    var visitorDocumentVerificationlistObj = await _manageVisitorsRepository.GetVisitorDocumentVerificationList(vVisitorDocumentVerification);
                    vResultObj.DocumentVerificationList = visitorDocumentVerificationlistObj.ToList();

                    var visitorAssetlistObj = await _manageVisitorsRepository.GetVisitorAssetList(vVisitorDocumentVerification);
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
                        VisitorId = vResultObj.Id
                    };

                    var visitorDocumentVerificationlistObj = await _manageVisitorsRepository.GetVisitorDocumentVerificationList(vVisitorDocumentVerification);
                    vResultObj.DocumentVerificationList = visitorDocumentVerificationlistObj.ToList();

                    var visitorAssetlistObj = await _manageVisitorsRepository.GetVisitorAssetList(vVisitorDocumentVerification);
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
                            foreach (var mitems in lstMUserObj)
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
                                WorkSheet1.Cells[recordIndex, 4].Value = items.Purpose;
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
                            WorkSheet1.Cells[recordIndex, 4].Value = items.Purpose;
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
                            WorkSheet1.Cells[recordIndex, 4].Value = items.Purpose;
                            WorkSheet1.Cells[recordIndex, 5].Value = items.BranchName;
                            WorkSheet1.Cells[recordIndex, 6].Value = items.DepartmentName;
                            WorkSheet1.Cells[recordIndex, 7].Value = mitems.GateNumber;
                            WorkSheet1.Cells[recordIndex, 8].Value = ((mitems.IsMeetingOver == false || mitems.IsMeetingOver == null) ? "Start" : "Meeting Over");
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
    }
}