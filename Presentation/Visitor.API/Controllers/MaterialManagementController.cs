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
    public class MaterialManagementController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IMaterialManagementRepository _materialManagementRepository;
        private readonly IManageVisitorsRepository _manageVisitorsRepository;
        private readonly IAssignGateNoRepository _assignGateNoRepository;
        private IFileManager _fileManager;

        public MaterialManagementController(IMaterialManagementRepository materialManagementRepository, IManageVisitorsRepository manageVisitorsRepository, IAssignGateNoRepository assignGateNoRepository, IFileManager fileManager)
        {
            _materialManagementRepository = materialManagementRepository;
            _manageVisitorsRepository = manageVisitorsRepository;
            _assignGateNoRepository = assignGateNoRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Material Request
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveMaterialRequest(MaterialRequest_Request parameters)
        {
            //Photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.WorkerPhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.WorkerPhoto_Base64, "\\Uploads\\Visitors\\", parameters.WorkerPhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.WorkerPhotoFileName = vUploadFile;
                }
            }

            int result = await _materialManagementRepository.SaveMaterialRequest(parameters);

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
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
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

                #region Material Request Details
                if (result > 0)
                {
                    foreach (var items in parameters.MaterialRequestDetails)
                    {
                        var vMaterialRequestDetails = new MaterialRequestDetails_Request()
                        {
                            Id = items.Id,
                            MaterialRequestId = result,
                            MaterialDetailsId = items.MaterialDetailsId,
                            Quantity = items.Quantity,
                            Rate = items.Rate,
                            Amount = items.Amount,
                            SerialNo = items.SerialNo,
                        };

                        int resultUserOtherDetails = await _materialManagementRepository.SaveMaterialRequestDetails(vMaterialRequestDetails);
                    }
                }
                #endregion

                var vMaterialRequest = await _materialManagementRepository.GetMaterialRequestById(result);
                if (vMaterialRequest != null)
                {
                    #region // Add/Update Assign GateNo

                    // Delete Assign
                    var vGateNoDELETEObj = new AssignGateNo_Request()
                    {
                        Action = "DELETE",
                        RefId = vMaterialRequest.VisitorId,
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
                            RefId = vMaterialRequest.VisitorId,
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
                            RefId = vMaterialRequest.VisitorId,
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
                }
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaterialRequestList(MaterialRequest_Search parameters)
        {
            IEnumerable<MaterialRequestList_Response> lstRoles = await _materialManagementRepository.GetMaterialRequestList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetMaterialRequestById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _materialManagementRepository.GetMaterialRequestById(Id);
                if (vResultObj != null)
                {
                    var vMaterialRequestDetails = new MaterialRequestDetails_Search()
                    {
                        MaterialRequestId = vResultObj.Id,
                    };

                    var vMaterialRequestDetailsList = await _materialManagementRepository.GetMaterialRequestDetailsList(vMaterialRequestDetails);
                    vResultObj.MaterialRequestDetails = vMaterialRequestDetailsList.ToList();

                    var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(Convert.ToInt32(vResultObj.VisitorId), "Visitor", 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();

                    var vVisitorDocumentVerification = new VisitorDocumentVerification_Search()
                    {
                        RefId = Convert.ToInt32(vResultObj.VisitorId),
                        RefType = "Visitor"
                    };

                    var visitorDocumentVerificationlistObj = await _manageVisitorsRepository.GetVisitorDocumentVerificationList(vVisitorDocumentVerification);
                    vResultObj.DocumentVerificationList = visitorDocumentVerificationlistObj.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> MaterialRequestApproveNReject(MaterialRequest_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                int resultExpenseDetails = await _materialManagementRepository.MaterialRequestApproveNReject(parameters);

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
                        _response.Message = "Material Request Approved successfully";
                    }
                    else if (parameters.StatusId == 3)
                    {
                        _response.Message = "Material Request Rejected successfully";
                    }
                }
            }

            return _response;
        }

        #endregion
    }
}
