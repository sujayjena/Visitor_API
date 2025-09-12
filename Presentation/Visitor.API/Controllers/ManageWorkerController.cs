using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageWorkerController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageWorkerRepository _manageWorkerRepository;
        private readonly IManageContractorRepository _manageContractorRepository;
        private readonly IAssignGateNoRepository _assignGateNoRepository;
        private readonly IBarcodeRepository _barcodeRepository;
        private readonly IManagePurchaseOrderRepository _managePurchaseOrderRepository;
        private readonly IManageVisitorsRepository _manageVisitorsRepository;
        private IFileManager _fileManager;

        public ManageWorkerController(IManageWorkerRepository manageWorkerRepository, IFileManager fileManager, IManageContractorRepository manageContractorRepository, IAssignGateNoRepository assignGateNoRepository, IBarcodeRepository barcodeRepository, IManagePurchaseOrderRepository managePurchaseOrderRepository, IManageVisitorsRepository manageVisitorsRepository)
        {
            _manageWorkerRepository = manageWorkerRepository;
            _fileManager = fileManager;
            _manageContractorRepository = manageContractorRepository;
            _assignGateNoRepository = assignGateNoRepository;
            _barcodeRepository = barcodeRepository;
            _managePurchaseOrderRepository = managePurchaseOrderRepository;
            _manageVisitorsRepository = manageVisitorsRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorker(Worker_Request parameters)
        {
            #region User Restriction 

            int vNoofPOWorker = 0;
            int totalWorkderRegistered = 0;

            if (parameters.Id == 0)
            {
                var vWorkerSearch = new WorkerSearch_Request();
                vWorkerSearch.PurchaseOrderId = 0;
                vWorkerSearch.BranchId = 0;
                vWorkerSearch.IsBlackList = false;
                vWorkerSearch.IsActive = true;

                var vWorker = await _manageWorkerRepository.GetWorkerList(vWorkerSearch);
              
                #region Contractor Wise Worker Check

                if (parameters.PurchaseOrderId > 0)
                {
                    //get total worker count
                    totalWorkderRegistered = vWorker.Where(x => x.PurchaseOrderId == parameters.PurchaseOrderId).Count();

                    //get total NoofPOWorkers 
                    var vPurchaseOrder = await _managePurchaseOrderRepository.GetPurchaseOrderById(Convert.ToInt32(parameters.PurchaseOrderId));
                    if (vPurchaseOrder != null)
                    {
                        vNoofPOWorker = vPurchaseOrder.NoofPOWorker ?? 0;
                    }
                }

                // Total Contractor check with register worker
                if (totalWorkderRegistered >= vNoofPOWorker)
                {
                    _response.Message = "You are not allowed to create worker more than " + vNoofPOWorker + ", Please contact your administrator to access this feature!";
                    return _response;
                }

                #endregion
            }

            #endregion

            //Document Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Document_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Document_Base64, "\\Uploads\\Worker\\", parameters.DocumentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DocumentFileName = vUploadFile;
                }
            }

            //photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.WorkerPhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.WorkerPhoto_Base64, "\\Uploads\\Worker\\", parameters.WorkerPhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.WorkerPhotoFileName = vUploadFile;
                }
            }

            //insurance Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.DV_Insurance_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.DV_Insurance_Base64, "\\Uploads\\Worker\\", parameters.DV_InsuranceOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DV_InsuranceFileName = vUploadFile;
                }
            }

            //wc Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.DV_WC_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.DV_WC_Base64, "\\Uploads\\Worker\\", parameters.DV_WCOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DV_WCFileName = vUploadFile;
                }
            }

            //esic Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.DV_ESIC_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.DV_ESIC_Base64, "\\Uploads\\Worker\\", parameters.DV_ESICOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DV_ESICFileName = vUploadFile;
                }
            }

            //police verification Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.PoliceV_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.PoliceV_Base64, "\\Uploads\\Worker\\", parameters.PoliceVOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PoliceVFileName = vUploadFile;
                }
            }

            //Fitness Certificate Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.FitnessCert_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.FitnessCert_Base64, "\\Uploads\\Worker\\", parameters.FitnessCertOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.FitnessCertFileName = vUploadFile;
                }
            }

            int result = await _manageWorkerRepository.SaveWorker(parameters);

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
                    RefType = "Worker",
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
                        RefType = "Worker",
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
                        RefType = "Worker",
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

                #region Worker Pass

                if (parameters.Id == 0)
                {
                    var vWorkerPass = new WorkerPass_Request()
                    {
                        Id = 0,
                        WorkerId = result,
                        PassNumber = "",
                        ValidFromDate = parameters.ValidFromDate,
                        ValidToDate = parameters.ValidToDate,
                        IsActive = parameters.IsActive
                    };

                    int resultWorkerPass = await _manageWorkerRepository.SaveWorkerPass(vWorkerPass);
                }
                
                #endregion

                #region Generate Barcode
                if (parameters.Id == 0)
                {
                    var vWorker = await _manageWorkerRepository.GetWorkerById(result);
                    if (vWorker != null)
                    {
                        string vBarcodeNo = "";
                        if (vWorker.BranchName == "ANGRE PORT")
                        {
                            vBarcodeNo = await _barcodeRepository.AutoBarcodeGenerate(Convert.ToInt32(vWorker.BranchId ?? 0), "Worker", "");
                        }
                        else
                        {
                            vBarcodeNo = vWorker.PassNumber;
                        }

                        if (vBarcodeNo != "")
                        {
                            var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vBarcodeNo);
                            if (vGenerateBarcode.Barcode_Unique_Id != "")
                            {
                                var vBarcode_Request = new Barcode_Request()
                                {
                                    Id = 0,
                                    BarcodeNo = vBarcodeNo,
                                    BarcodeType = "Worker",
                                    Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                    BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                    BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                    BranchId = vWorker.BranchId,

                                    RefId = result
                                };
                                var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
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
        public async Task<ResponseModel> GetWorkerList(WorkerSearch_Request parameters)
        {
            IEnumerable<Worker_Response> lstWorkers = await _manageWorkerRepository.GetWorkerList(parameters);
            _response.Data = lstWorkers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageWorkerRepository.GetWorkerById(Id);
                if(vResultObj != null)
                {
                    var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(vResultObj.Id, "Worker", 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();

                    var vVisitorDocumentVerification = new VisitorDocumentVerification_Search()
                    {
                        RefId = vResultObj.Id,
                        RefType = "Worker"
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
        public async Task<ResponseModel> SaveWorkerPass(WorkerPass_Request parameters)
        {
            int result = await _manageWorkerRepository.SaveWorkerPass(parameters);

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

                #region Generate Barcode
                if (parameters.Id == 0)
                {
                    var vWorker = await _manageWorkerRepository.GetWorkerById(Convert.ToInt32(parameters.WorkerId));
                    if (vWorker != null)
                    {
                        string vBarcodeNo = "";
                        if (vWorker.BranchName == "ANGRE PORT")
                        {
                            vBarcodeNo = await _barcodeRepository.AutoBarcodeGenerate(Convert.ToInt32(vWorker.BranchId ?? 0), "Worker", "");
                        }
                        else
                        {
                            vBarcodeNo = vWorker.PassNumber;
                        }

                        if (vBarcodeNo != "")
                        {
                            var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vBarcodeNo);
                            if (vGenerateBarcode.Barcode_Unique_Id != "")
                            {
                                var vBarcode_Request = new Barcode_Request()
                                {
                                    Id = 0,
                                    BarcodeNo = vBarcodeNo,
                                    BarcodeType = "Worker",
                                    Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                    BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                    BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                    BranchId = vWorker.BranchId,
                                    RefId = result
                                };
                                var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
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
        public async Task<ResponseModel> ExportWorkerAttendanceData(WorkerSearch_Request parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Worker_Response> lstSizeObj = await _manageWorkerRepository.GetWorkerList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("WorkerAttendance");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Sr.No";
                    WorkSheet1.Cells[1, 2].Value = "Worker ID";
                    WorkSheet1.Cells[1, 3].Value = "Worker Name";
                    WorkSheet1.Cells[1, 4].Value = "Branch";
                    WorkSheet1.Cells[1, 5].Value = "Gate No";
                    WorkSheet1.Cells[1, 6].Value = "Status";
                    WorkSheet1.Cells[1, 7].Value = "Remark";
                    WorkSheet1.Cells[1, 8].Value = "Created Date";
                    WorkSheet1.Cells[1, 9].Value = "Created By";

                    recordIndex = 2;

                    int i = 1;

                    foreach (var items in lstSizeObj)
                    {
                        //log history list
                        var vCheckedInOutLogHistory_Search = new CheckedInOutLogHistory_Search();
                        vCheckedInOutLogHistory_Search.RefId = items.Id;
                        vCheckedInOutLogHistory_Search.RefType = "Worker";
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
                                WorkSheet1.Cells[recordIndex, 2].Value = items.WorkerId;
                                WorkSheet1.Cells[recordIndex, 3].Value = items.WorkerName;
                                WorkSheet1.Cells[recordIndex, 4].Value = items.BranchName;
                                WorkSheet1.Cells[recordIndex, 5].Value = mitems.GateNumber;
                                WorkSheet1.Cells[recordIndex, 6].Value = mitems.CheckedStatus;
                                WorkSheet1.Cells[recordIndex, 7].Value = mitems.CheckedRemark;
                                WorkSheet1.Cells[recordIndex, 8].Value = Convert.ToDateTime(mitems.CreatedDate).ToString("dd/MM/yyyy");
                                WorkSheet1.Cells[recordIndex, 9].Value = mitems.CreatorName;

                                recordIndex += 1;

                                j++;
                            }
                        }
                        else
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = i.ToString();
                            WorkSheet1.Cells[recordIndex, 2].Value = items.WorkerId;
                            WorkSheet1.Cells[recordIndex, 3].Value = items.WorkerName;
                            WorkSheet1.Cells[recordIndex, 4].Value = items.BranchName;

                            recordIndex += 1;
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
        public async Task<ResponseModel> DownloadWorkerTemplate()
        {
            byte[]? formatFile = await Task.Run(() => _fileManager.GetFormatFileFromPath("Template_Worker.xlsx"));

            if (formatFile != null)
            {
                _response.Data = formatFile;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportWorker([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;

            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            List<string[]> data = new List<string[]>();
            List<Worker_ImportData> lst_ImportData = new List<Worker_ImportData>();
            IEnumerable<Worker_ImportDataValidation> lst_ImportDataValidation;

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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "WorkerShift", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "WorkerName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "WorkerType", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "WorkerMobileNo", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "ValidFromDate", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 6].Value.ToString(), "ValidToDate", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 7].Value.ToString(), "PONumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 8].Value.ToString(), "DateOfBirth", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 9].Value.ToString(), "BloodGroup", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 10].Value.ToString(), "IdentificationMark", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 11].Value.ToString(), "Address", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 12].Value.ToString(), "Country", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 13].Value.ToString(), "State", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 14].Value.ToString(), "Province", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 15].Value.ToString(), "Pincode", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 16].Value.ToString(), "IsInsurance", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 17].Value.ToString(), "InsuranceNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 18].Value.ToString(), "IsWC", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 19].Value.ToString(), "WCNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 20].Value.ToString(), "IsESIC", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 21].Value.ToString(), "ESICNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 22].Value.ToString(), "IsPoliceVerification", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 23].Value.ToString(), "IsFitnessCertificate", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 24].Value.ToString(), "Branch", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 25].Value.ToString(), "WorkPlace", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 26].Value.ToString(), "Department", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 27].Value.ToString(), "EmployeeName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 28].Value.ToString(), "GateNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 29].Value.ToString(), "IsDriver", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 30].Value.ToString(), "VehicleNumber", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 31].Value.ToString(), "DrivingLicenseNo", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 32].Value.ToString(), "LicenseValidFrom", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 33].Value.ToString(), "LicenseValidTo", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 34].Value.ToString(), "IsBlackList", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 35].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    if (!string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 2].Value?.ToString()) && !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 3].Value?.ToString()))
                    {
                        lst_ImportData.Add(new Worker_ImportData()
                        {
                            WorkerShift = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                            WorkerName = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                            WorkerType = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                            WorkerMobileNo = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                            ValidFromDate = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 5].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 5].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            ValidToDate = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 6].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 6].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            PONumber = workSheet.Cells[rowIterator, 7].Value?.ToString(),
                            DateOfBirth = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 8].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 8].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            BloodGroup = workSheet.Cells[rowIterator, 9].Value?.ToString(),
                            IdentificationMark = workSheet.Cells[rowIterator, 10].Value?.ToString(),
                            Address = workSheet.Cells[rowIterator, 11].Value?.ToString(),
                            Country = workSheet.Cells[rowIterator, 12].Value?.ToString(),
                            State = workSheet.Cells[rowIterator, 13].Value?.ToString(),
                            Province = workSheet.Cells[rowIterator, 14].Value?.ToString(),
                            Pincode = workSheet.Cells[rowIterator, 15].Value?.ToString(),
                            IsInsurance = workSheet.Cells[rowIterator, 16].Value?.ToString(),
                            InsuranceNumber = workSheet.Cells[rowIterator, 17].Value?.ToString(),
                            IsWC = workSheet.Cells[rowIterator, 18].Value?.ToString(),
                            WCNumber = workSheet.Cells[rowIterator, 19].Value?.ToString(),
                            IsESIC = workSheet.Cells[rowIterator, 20].Value?.ToString(),
                            ESICNumber = workSheet.Cells[rowIterator, 21].Value?.ToString(),
                            IsPoliceVerification = workSheet.Cells[rowIterator, 22].Value?.ToString(),
                            IsFitnessCertificate = workSheet.Cells[rowIterator, 23].Value?.ToString(),
                            Branch = workSheet.Cells[rowIterator, 24].Value?.ToString(),
                            WorkPlace = workSheet.Cells[rowIterator, 25].Value?.ToString(),
                            Department = workSheet.Cells[rowIterator, 26].Value?.ToString(),
                            EmployeeName = workSheet.Cells[rowIterator, 27].Value?.ToString(),
                            GateNumber = workSheet.Cells[rowIterator, 28].Value?.ToString(),
                            IsDriver = workSheet.Cells[rowIterator, 29].Value?.ToString(),
                            VehicleNumber = workSheet.Cells[rowIterator, 30].Value?.ToString(),
                            DrivingLicenseNo = workSheet.Cells[rowIterator, 31].Value?.ToString(),
                            LicenseValidFrom = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 32].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 32].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            LicenseValidTo = !string.IsNullOrWhiteSpace(workSheet.Cells[rowIterator, 33].Value?.ToString()) ? DateTime.ParseExact(workSheet.Cells[rowIterator, 33].Value?.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat) : null,
                            IsBlackList = workSheet.Cells[rowIterator, 34].Value?.ToString(),
                            IsActive = workSheet.Cells[rowIterator, 35].Value?.ToString()
                        }); ;
                    }
                }
            }

            if (lst_ImportData.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lst_ImportDataValidation = await _manageWorkerRepository.ImportWorker(lst_ImportData);

            #region Generate Excel file for Invalid Data

            if (lst_ImportDataValidation.ToList().Count > 0 && lst_ImportDataValidation.ToList().FirstOrDefault().ValidFromDate != null)
            {
                _response.IsSuccess = false;
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidImportDataFile(lst_ImportDataValidation);
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Record imported successfully";
            }

            #endregion

            return _response;
        }

        private byte[] GenerateInvalidImportDataFile(IEnumerable<Worker_ImportDataValidation> lst_ImportDataValidation)
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

                    WorkSheet1.Cells[1, 1].Value = "WorkerShift";
                    WorkSheet1.Cells[1, 2].Value = "WorkerName";
                    WorkSheet1.Cells[1, 3].Value = "WorkerType";
                    WorkSheet1.Cells[1, 4].Value = "WorkerMobileNo";
                    WorkSheet1.Cells[1, 5].Value = "ValidFromDate";
                    WorkSheet1.Cells[1, 6].Value = "ValidToDate";
                    WorkSheet1.Cells[1, 7].Value = "PONumber";
                    WorkSheet1.Cells[1, 8].Value = "DateOfBirth";
                    WorkSheet1.Cells[1, 9].Value = "BloodGroup";
                    WorkSheet1.Cells[1, 10].Value = "IdentificationMark";
                    WorkSheet1.Cells[1, 11].Value = "Address";
                    WorkSheet1.Cells[1, 12].Value = "Country";
                    WorkSheet1.Cells[1, 13].Value = "State";
                    WorkSheet1.Cells[1, 14].Value = "Province";
                    WorkSheet1.Cells[1, 15].Value = "Pincode";
                    WorkSheet1.Cells[1, 16].Value = "IsInsurance";
                    WorkSheet1.Cells[1, 17].Value = "InsuranceNumber";
                    WorkSheet1.Cells[1, 18].Value = "IsWC";
                    WorkSheet1.Cells[1, 19].Value = "WCNumber";
                    WorkSheet1.Cells[1, 20].Value = "IsESIC";
                    WorkSheet1.Cells[1, 21].Value = "ESICNumber";
                    WorkSheet1.Cells[1, 22].Value = "IsPoliceVerification";
                    WorkSheet1.Cells[1, 23].Value = "IsFitnessCertificate";
                    WorkSheet1.Cells[1, 24].Value = "WorkPlace";
                    WorkSheet1.Cells[1, 25].Value = "Branch";
                    WorkSheet1.Cells[1, 26].Value = "Department";
                    WorkSheet1.Cells[1, 27].Value = "EmployeeName";
                    WorkSheet1.Cells[1, 28].Value = "GateNumber";
                    WorkSheet1.Cells[1, 29].Value = "IsDriver";
                    WorkSheet1.Cells[1, 30].Value = "VehicleNumber";
                    WorkSheet1.Cells[1, 31].Value = "DrivingLicenseNo";
                    WorkSheet1.Cells[1, 32].Value = "LicenseValidFrom";
                    WorkSheet1.Cells[1, 33].Value = "LicenseValidTo";
                    WorkSheet1.Cells[1, 34].Value = "IsBlackList";
                    WorkSheet1.Cells[1, 35].Value = "IsActive";
                    WorkSheet1.Cells[1, 36].Value = "ErrorMessage";

                    recordIndex = 2;

                    foreach (Worker_ImportDataValidation record in lst_ImportDataValidation)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.WorkerShift;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.WorkerName;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.WorkerType;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.WorkerMobileNo;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.ValidFromDate;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.ValidToDate;
                        WorkSheet1.Cells[recordIndex, 7].Value = record.PONumber;
                        WorkSheet1.Cells[recordIndex, 8].Value = record.DateOfBirth;
                        WorkSheet1.Cells[recordIndex, 9].Value =  record.BloodGroup;
                        WorkSheet1.Cells[recordIndex, 10].Value = record.IdentificationMark;
                        WorkSheet1.Cells[recordIndex, 11].Value = record.Address;
                        WorkSheet1.Cells[recordIndex, 12].Value = record.Country;
                        WorkSheet1.Cells[recordIndex, 13].Value = record.State;
                        WorkSheet1.Cells[recordIndex, 14].Value = record.Province;
                        WorkSheet1.Cells[recordIndex, 15].Value = record.Pincode;
                        WorkSheet1.Cells[recordIndex, 16].Value = record.IsInsurance;
                        WorkSheet1.Cells[recordIndex, 17].Value = record.InsuranceNumber;
                        WorkSheet1.Cells[recordIndex, 18].Value = record.IsWC;
                        WorkSheet1.Cells[recordIndex, 19].Value = record.WCNumber;
                        WorkSheet1.Cells[recordIndex, 20].Value = record.IsESIC;
                        WorkSheet1.Cells[recordIndex, 21].Value = record.ESICNumber;
                        WorkSheet1.Cells[recordIndex, 22].Value = record.IsPoliceVerification;
                        WorkSheet1.Cells[recordIndex, 23].Value = record.IsFitnessCertificate;
                        WorkSheet1.Cells[recordIndex, 24].Value = record.Branch;
                        WorkSheet1.Cells[recordIndex, 25].Value = record.WorkPlace;
                        WorkSheet1.Cells[recordIndex, 26].Value = record.Department;
                        WorkSheet1.Cells[recordIndex, 27].Value = record.EmployeeName;
                        WorkSheet1.Cells[recordIndex, 28].Value = record.GateNumber;
                        WorkSheet1.Cells[recordIndex, 29].Value = record.IsDriver;
                        WorkSheet1.Cells[recordIndex, 30].Value = record.VehicleNumber;
                        WorkSheet1.Cells[recordIndex, 31].Value = record.DrivingLicenseNo;
                        WorkSheet1.Cells[recordIndex, 32].Value = record.LicenseValidFrom;
                        WorkSheet1.Cells[recordIndex, 33].Value = record.LicenseValidTo;
                        WorkSheet1.Cells[recordIndex, 34].Value = record.IsBlackList;
                        WorkSheet1.Cells[recordIndex, 35].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 36].Value = record.ValidationMessage;

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
        public async Task<ResponseModel> ExportWorkerData(WorkerSearch_Request parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Worker_Response> lstData = await _manageWorkerRepository.GetWorkerList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Worker");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Worker Shift";
                    WorkSheet1.Cells[1, 2].Value = "Contractor Type";
                    WorkSheet1.Cells[1, 3].Value = "Contractor Firm";
                    WorkSheet1.Cells[1, 4].Value = "PO Number";
                    WorkSheet1.Cells[1, 5].Value = "Worker Name";
                    WorkSheet1.Cells[1, 6].Value = "Worker Pass ID";
                    WorkSheet1.Cells[1, 7].Value = "Worker Type";
                    WorkSheet1.Cells[1, 8].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 9].Value = "Valid From";
                    WorkSheet1.Cells[1, 10].Value = "Valid To";
                    WorkSheet1.Cells[1, 11].Value = "Contractor Start Date";
                    WorkSheet1.Cells[1, 12].Value = "Contractor End Date";
                    WorkSheet1.Cells[1, 13].Value = "PO Start Date";
                    WorkSheet1.Cells[1, 14].Value = "PO End Date";
                    WorkSheet1.Cells[1, 15].Value = "Date of Birth";
                    WorkSheet1.Cells[1, 16].Value = "Blood Group";
                    WorkSheet1.Cells[1, 17].Value = "Identification Mark";
                    WorkSheet1.Cells[1, 18].Value = "Address";
                    WorkSheet1.Cells[1, 19].Value = "Country";
                    WorkSheet1.Cells[1, 20].Value = "State";
                    WorkSheet1.Cells[1, 21].Value = "Province";
                    WorkSheet1.Cells[1, 22].Value = "Pincode";
                    WorkSheet1.Cells[1, 23].Value = "Insurance Available?";
                    WorkSheet1.Cells[1, 24].Value = "Insurance Number";
                    WorkSheet1.Cells[1, 25].Value = "Workmen Compensation (WC)?";
                    WorkSheet1.Cells[1, 26].Value = "WC Number";
                    WorkSheet1.Cells[1, 27].Value = "ESIC Available?";
                    WorkSheet1.Cells[1, 28].Value = "ESIC Number";
                    WorkSheet1.Cells[1, 29].Value = "Police Verification?";
                    WorkSheet1.Cells[1, 30].Value = "Fitness Certificate?";
                    WorkSheet1.Cells[1, 31].Value = "Branch";
                    WorkSheet1.Cells[1, 32].Value = "Department";
                    WorkSheet1.Cells[1, 33].Value = "Employee Name";
                    WorkSheet1.Cells[1, 34].Value = "Gate Number";
                    WorkSheet1.Cells[1, 35].Value = "Is Driver?";
                    WorkSheet1.Cells[1, 36].Value = "Vehicle Number";
                    WorkSheet1.Cells[1, 37].Value = "Driving License Number";
                    WorkSheet1.Cells[1, 38].Value = "License Valid From";
                    WorkSheet1.Cells[1, 39].Value = "License Valid To";
                    WorkSheet1.Cells[1, 40].Value = "Validity Period";
                    WorkSheet1.Cells[1, 41].Value = "Blacklisted?";
                    WorkSheet1.Cells[1, 42].Value = "Status";
                    WorkSheet1.Cells[1, 43].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 44].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstData)
                    {
                        string strGateNumberList = string.Empty;
                        var vSecurityGateDetail = await _assignGateNoRepository.GetAssignGateNoById(RefId: Convert.ToInt32(items.Id), "Worker", GateDetailsId: 0);
                        if (vSecurityGateDetail.ToList().Count > 0)
                        {
                            strGateNumberList = string.Join(",", vSecurityGateDetail.ToList().Select(x => x.GateNumber));
                        }

                        if (items.WorkerShift == 1)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = "8HR";
                        }
                        else if (items.WorkerShift == 2)
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = "12HR";
                        }
                        else
                        {
                            WorkSheet1.Cells[recordIndex, 1].Value = "";
                        }
                        WorkSheet1.Cells[recordIndex, 2].Value = items.ContractorType;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.ContractorName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.PONumber;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.WorkerName;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.WorkerId;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.WorkerType;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.WorkerMobileNo;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.ValidFromDate.HasValue? items.ValidFromDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 10].Value = items.ValidToDate.HasValue? items.ValidToDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 11].Value = items.ContractorStartDate.HasValue? items.ContractorStartDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 12].Value = items.ContractorEndDate.HasValue? items.ContractorEndDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 13].Value = items.POStartDate.HasValue? items.POStartDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 14].Value = items.POEndDate.HasValue? items.POEndDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 15].Value = items.DOB.HasValue ? items.DOB.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty; 
                        WorkSheet1.Cells[recordIndex, 16].Value = items.BloodGroup;
                        WorkSheet1.Cells[recordIndex, 17].Value = items.IdentificationMark;
                        WorkSheet1.Cells[recordIndex, 18].Value = items.Address;
                        WorkSheet1.Cells[recordIndex, 19].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 20].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 21].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 22].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 23].Value = items.DV_IsInsurance == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 24].Value = items.DV_InsuranceNumber;
                        WorkSheet1.Cells[recordIndex, 25].Value = items.DV_IsWC == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 26].Value = items.DV_WCNumber;
                        WorkSheet1.Cells[recordIndex, 27].Value = items.DV_IsESIC == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 28].Value = items.DV_ESICNumber;
                        WorkSheet1.Cells[recordIndex, 29].Value = items.IsPoliceV == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 30].Value = items.IsFitnessCert == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 31].Value = items.BranchName;
                        WorkSheet1.Cells[recordIndex, 32].Value = items.DepartmentName;
                        WorkSheet1.Cells[recordIndex, 33].Value = items.EmployeeName;
                        WorkSheet1.Cells[recordIndex, 34].Value = strGateNumberList;
                        WorkSheet1.Cells[recordIndex, 35].Value = items.IsDriver == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 36].Value = items.VehicleNumber;
                        WorkSheet1.Cells[recordIndex, 37].Value = items.DrivingLicenseNo;
                        WorkSheet1.Cells[recordIndex, 38].Value = items.LicenseValidFrom.HasValue ? items.LicenseValidFrom.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty; 
                        WorkSheet1.Cells[recordIndex, 39].Value = items.LicenseValidTo.HasValue? items.LicenseValidTo.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 40].Value = items.ValidityPeriod;
                        WorkSheet1.Cells[recordIndex, 41].Value = items.IsBlackList == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 42].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 43].Value = Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 44].Value = items.CreatorName;

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
    }
}
