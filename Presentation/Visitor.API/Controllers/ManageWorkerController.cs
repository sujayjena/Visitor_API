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
                        var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vWorker.PassNumber);
                        if (vGenerateBarcode.Barcode_Unique_Id != "")
                        {
                            var vBarcode_Request = new Barcode_Request()
                            {
                                Id = 0,
                                BarcodeNo = vWorker.PassNumber,
                                BarcodeType = "Worker",
                                Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                RefId = result
                            };
                            var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
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
                        var vGenerateBarcode = _barcodeRepository.GenerateBarcode(vWorker.PassNumber);
                        if (vGenerateBarcode.Barcode_Unique_Id != "")
                        {
                            var vBarcode_Request = new Barcode_Request()
                            {
                                Id = 0,
                                BarcodeNo = vWorker.PassNumber,
                                BarcodeType = "Worker",
                                Barcode_Unique_Id = vGenerateBarcode.Barcode_Unique_Id,
                                BarcodeOriginalFileName = vGenerateBarcode.BarcodeOriginalFileName,
                                BarcodeFileName = vGenerateBarcode.BarcodeFileName,
                                RefId = result
                            };
                            var resultBarcode = _barcodeRepository.SaveBarcode(vBarcode_Request);
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
    }
}
