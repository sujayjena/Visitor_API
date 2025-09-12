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
    public class ManagePurchaseOrderController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManagePurchaseOrderRepository _managePurchaseOrderRepository;
        private readonly IManageContractorRepository _manageContractorRepository;
        private readonly IAssignGateNoRepository _assignGateNoRepository;
        private readonly IBarcodeRepository _barcodeRepository;
        private IFileManager _fileManager;

        public ManagePurchaseOrderController(IManagePurchaseOrderRepository managePurchaseOrderRepository, IFileManager fileManager, IManageContractorRepository manageContractorRepository, IAssignGateNoRepository assignGateNoRepository, IBarcodeRepository barcodeRepository)
        {
            _managePurchaseOrderRepository = managePurchaseOrderRepository;
            _fileManager = fileManager;
            _manageContractorRepository = manageContractorRepository;
            _assignGateNoRepository = assignGateNoRepository;
            _barcodeRepository = barcodeRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePurchaseOrder(PurchaseOrder_Request parameters)
        {
            #region User Restriction 

            //int vNoofContractedPO = 0;
            //int totalPORegistered = 0;

            //if (parameters.Id == 0)
            //{
            //    var vPurchaseOrderSearch_Request = new PurchaseOrderSearch_Request();
            //    vPurchaseOrderSearch_Request.ContractorId = 0;
            //    vPurchaseOrderSearch_Request.IsActive = true;

            //    var vPO = await _managePurchaseOrderRepository.GetPurchaseOrderList(vPurchaseOrderSearch_Request);

            //    #region Contractor Wise PO Check

            //    if (parameters.ContractorId > 0)
            //    {
            //        //get total po count
            //        totalPORegistered = vPO.Where(x => x.ContractorId == parameters.ContractorId).Count();

            //        //get total NoofContractedPO 
            //        var vContractor = await _manageContractorRepository.GetContractorById(Convert.ToInt32(parameters.ContractorId));
            //        if (vContractor != null)
            //        {
            //            vNoofContractedPO = vContractor.NoofContractedPO ?? 0;
            //        }
            //    }

            //    // Total Contractor check with register worker
            //    if (totalPORegistered >= vNoofContractedPO)
            //    {
            //        _response.Message = "You are not allowed to create po more than " + vNoofContractedPO + ", Please contact your administrator to access this feature!";
            //        return _response;
            //    }

            //    #endregion
            //}

            #endregion

            //Document Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Document_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Document_Base64, "\\Uploads\\PurchaseOrder\\", parameters.DocumentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DocumentFileName = vUploadFile;
                }
            }

            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.POAttachment_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.POAttachment_Base64, "\\Uploads\\PurchaseOrder\\", parameters.POAttachmentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.POAttachmentFileName = vUploadFile;
                }
            }

            int result = await _managePurchaseOrderRepository.SavePurchaseOrder(parameters);

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
        public async Task<ResponseModel> GetPurchaseOrderList(PurchaseOrderSearch_Request parameters)
        {
            IEnumerable<PurchaseOrder_Response> lstPurchaseOrders = await _managePurchaseOrderRepository.GetPurchaseOrderList(parameters);
            _response.Data = lstPurchaseOrders.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPurchaseOrderById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _managePurchaseOrderRepository.GetPurchaseOrderById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportPurchaseOrderData(PurchaseOrderSearch_Request parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<PurchaseOrder_Response> lstData = await _managePurchaseOrderRepository.GetPurchaseOrderList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("PurchaseOrder");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Contractor Name";
                    WorkSheet1.Cells[1, 2].Value = "Purchase Order Number";
                    WorkSheet1.Cells[1, 3].Value = "Purchase Order Name";
                    WorkSheet1.Cells[1, 4].Value = "Start Date";
                    WorkSheet1.Cells[1, 5].Value = "End Date";
                    WorkSheet1.Cells[1, 6].Value = "Number of Workers";
                    WorkSheet1.Cells[1, 7].Value = "Status";
                    WorkSheet1.Cells[1, 8].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 9].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstData)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.ContractorName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.PONumber;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.POName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.ValidFromDate.HasValue ? items.ValidFromDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.ValidToDate.HasValue ? items.ValidToDate.Value.ToString("dd/MM/yyyy hh:mm:ss:tt") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.NoofPOWorker;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 8].Value = Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 9].Value = items.CreatorName;

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
