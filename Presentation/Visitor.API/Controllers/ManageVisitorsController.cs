using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
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
        private IFileManager _fileManager;

        public ManageVisitorsController(IManageVisitorsRepository manageVisitorsRepository, IFileManager fileManager)
        {
            _manageVisitorsRepository = manageVisitorsRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

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

            // Visitor Photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.VisitorPhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.VisitorPhoto_Base64, "\\Uploads\\Visitors\\", parameters.VisitorPhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.VisitorPhoto = vUploadFile;
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
                _response.Message = "Record details saved successfully";

                #region // Add/Update Visitor GateNo

                // Delete Region of Branch
                var vGateNoDELETEObj = new VisitorGateNo_Request()
                {
                    Action = "DELETE",
                    VisitorId = result,
                    GateDetailsId = 0
                };
                int resultGateNoDELETE = await _manageVisitorsRepository.SaveVisitorsGateNo(vGateNoDELETEObj);


                // add new Visitor field
                foreach (var vGateitem in parameters.GateNumberList)
                {
                    var vGateNoMapObj = new VisitorGateNo_Request()
                    {
                        Action = "INSERT",
                        VisitorId = result,
                        GateDetailsId = vGateitem.GateDetailsId
                    };

                    int resultGateNo = await _manageVisitorsRepository.SaveVisitorsGateNo(vGateNoMapObj);
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
                        Id= vitem.Id,
                        VisitorId = result,
                        IDTypeId = vitem.IDTypeId,
                        DocumentOriginalFileName = vitem.DocumentOriginalFileName,
                        DocumentFileName = vitem.DocumentFileName,
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
                var gateNolistObj = await _manageVisitorsRepository.GetVisitorsGateNoByVisitorId(user.Id, 0);
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
                    var gateNolistObj = await _manageVisitorsRepository.GetVisitorsGateNoByVisitorId(vResultObj.Id, 0);
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
                
                if (parameters.StatusId == 2)
                {
                    var vVisitorResponse = await _manageVisitorsRepository.GetVisitorsById(Convert.ToInt32(parameters.Id));
                    if (vVisitorResponse != null)
                    {
                        //Prepare you post parameters  
                        var postData = new Visitor_Barcode_Request()
                        {
                            value = vVisitorResponse.VisitNumber
                        };

                        //Call API
                        string sendUri = "http://164.52.213.175:5050/generate_barcode_v2";

                        //Create HTTPWebrequest  
                        HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendUri);

                        var jsonData = JsonConvert.SerializeObject(postData);

                        //Prepare and Add URL Encoded data  
                        UTF8Encoding encoding = new UTF8Encoding();
                        byte[] data = encoding.GetBytes(jsonData);

                        //Specify post method  
                        httpWReq.Method = "POST";
                        httpWReq.ContentType = "application/json";
                        httpWReq.ContentLength = data.Length;
                        using (Stream stream = httpWReq.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        //Get the response  
                        HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string responseString = reader.ReadToEnd();

                        //Close the response  
                        reader.Close();

                        response.Close();

                        dynamic jsonResults = JsonConvert.DeserializeObject<dynamic>(responseString);
                        var status = jsonResults.ContainsKey("isSuccess") ? jsonResults.isSuccess : false;

                        if (status == true)
                        {
                            var barcode = jsonResults["barcode"];

                            var barcode_image_base64 = barcode.ContainsKey("barcode_image_base64") ? barcode.barcode_image_base64 : string.Empty;
                            var vbarcode_image_base64 = Convert.ToString(barcode_image_base64);

                            var unique_id = barcode.ContainsKey("unique_id") ? barcode.unique_id : string.Empty;
                            var vUniqueId = Convert.ToString(unique_id);

                            if (!string.IsNullOrWhiteSpace(vbarcode_image_base64))
                            {
                                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(vbarcode_image_base64, "\\Uploads\\Barcode\\", vUniqueId + ".png");
                                if (!string.IsNullOrWhiteSpace(vUploadFile))
                                {
                                    parameters.BarcodeOriginalFileName = vUniqueId + ".png";
                                    parameters.BarcodeFileName = vUploadFile;
                                }
                            }

                            if (vUniqueId != "")
                            {
                                var vBarcode_Request = new Barcode_Request()
                                {
                                    Id = 0,
                                    BarcodeNo = vVisitorResponse.VisitNumber,
                                    BarcodeType = "Visitor",
                                    Barcode_Unique_Id = vUniqueId,
                                    RefId = vVisitorResponse.Id
                                };
                                var resultBarcode = _manageVisitorsRepository.SaveBarcode(vBarcode_Request);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(parameters.BarcodeFileName) && parameters.StatusId == 2)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Barcode is not generated";

                    return _response;
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

                    _response.Message = "Record details saved successfully";
                }
            }

            return _response;
        }

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
                    var gateNolistObj = await _manageVisitorsRepository.GetVisitorsGateNoByVisitorId(vResultObj.Id, 0);

                    vResultObj.GateNumberList = gateNolistObj.ToList();
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
            int result = await _manageVisitorsRepository.SaveVisitorCheckedInOut(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record is already exists";
            }
            else if (result == -3)
            {
                _response.Message = "Already Checked In for this gate.";
            }
            else if (result == -4)
            {
                _response.Message = "Not Allowed checked In for this gate again.";
            }
            else if (result == -5)
            {
                _response.Message = "Already Checked Out for this gate.";
            }
            else if (result == -6)
            {
                _response.Message = "Not Allowed Checked Out this gate again.";
            }
            else if (result == -7)
            {
                _response.Message = "Not Allow to Checked Out from this gate";
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
        public async Task<ResponseModel> GetBarcodeById(string BarcodeNo)
        {
            if (BarcodeNo == "")
            {
                _response.Message = "Barcode No. is required";
            }
            else
            {
                var vResultObj = await _manageVisitorsRepository.GetBarcodeById(BarcodeNo);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
