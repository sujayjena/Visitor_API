using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageReportController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageReportRepository _manageReportRepository;
        private IFileManager _fileManager;

        public ManageReportController(IManageReportRepository manageReportRepository, IFileManager fileManager)
        {
            _manageReportRepository = manageReportRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Canteen Usage Report
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenUsageReport(CanteenUsageReport_Search parameters)
        {
            IEnumerable<CanteenUsageReport_Response> lst = await _manageReportRepository.GetCanteenUsageReport(parameters);

            _response.Data = lst.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportCanteenUsageSummaryData(CanteenUsageReport_Search request)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<CanteenUsageReport_Response> lstObj = await _manageReportRepository.GetCanteenUsageReport(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("CanteenUsage_Summary");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Code";
                    WorkSheet1.Cells[1, 2].Value = "Name";
                    WorkSheet1.Cells[1, 3].Value = "Company Name";
                    WorkSheet1.Cells[1, 4].Value = "Breakfast";
                    WorkSheet1.Cells[1, 5].Value = "Lunch";
                    WorkSheet1.Cells[1, 6].Value = "Snacks";
                    WorkSheet1.Cells[1, 7].Value = "Dinner";

                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.RefTypeID;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.RefName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.CompanyName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.Breakfast;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.Lunch;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.Snacks;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.Dinner;

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
                _response.Message = "Data Exported successfully";
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportCanteenUsageDetailsData(CanteenUsageReport_Search request)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<CanteenUsageReport_Response> lstObj = await _manageReportRepository.GetCanteenUsageReport(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("CanteenUsage_Details");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Consumption Date";
                    WorkSheet1.Cells[1, 2].Value = "Code";
                    WorkSheet1.Cells[1, 3].Value = "Name";
                    WorkSheet1.Cells[1, 4].Value = "Company Name";
                    WorkSheet1.Cells[1, 5].Value = "Breakfast";
                    WorkSheet1.Cells[1, 6].Value = "Lunch";
                    WorkSheet1.Cells[1, 7].Value = "Snacks";
                    WorkSheet1.Cells[1, 8].Value = "Dinner";

                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = Convert.ToDateTime(items.ConsumptionDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 2].Value = items.RefTypeID;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.RefName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CompanyName;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.Breakfast;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.Lunch;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.Snacks;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.Dinner;

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
                _response.Message = "Data Exported successfully";
            }

            return _response;
        }

        #endregion

        #region Canteen Usage Report
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCanteenWastageReport(CanteenWastageReport_Search parameters)
        {
            IEnumerable<CanteenWastageReport_Response> lst = await _manageReportRepository.GetCanteenWastageReport(parameters);

            _response.Data = lst.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportCanteenWastageData(CanteenWastageReport_Search request)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<CanteenWastageReport_Response> lstObj = await _manageReportRepository.GetCanteenWastageReport(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("CanteenWastage");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Date";
                    WorkSheet1.Cells[1, 2].Value = "Meal Type";
                    WorkSheet1.Cells[1, 3].Value = "Item Name";
                    WorkSheet1.Cells[1, 4].Value = "Qty";
                    WorkSheet1.Cells[1, 5].Value = "UOM";
                    WorkSheet1.Cells[1, 6].Value = "Created Date";
                    WorkSheet1.Cells[1, 7].Value = "Created By";

                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.FWDate != null ? Convert.ToDateTime(items.FWDate).ToString("dd/MM/yyyy") : "";
                        WorkSheet1.Cells[recordIndex, 2].Value = items.MealType;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.ItemName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.Quantity;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.UOMName;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.CreatedDate.ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 7].Value = items.CreatorName;

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
                _response.Message = "Data Exported successfully";
            }

            return _response;
        }
        #endregion

        #region Safety Report
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSafetyReport(SafetyReport_Search parameters)
        {
            IEnumerable<SafetyReport_Response> lst = await _manageReportRepository.GetSafetyReport(parameters);

            _response.Data = lst.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportSafetyReportData(SafetyReport_Search request)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<SafetyReport_Response> lstObj = await _manageReportRepository.GetSafetyReport(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Safety_Report");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Sr.No";
                    WorkSheet1.Cells[1, 2].Value = "Date";
                    WorkSheet1.Cells[1, 3].Value = "Employee";
                    WorkSheet1.Cells[1, 4].Value = "Worker";
                    WorkSheet1.Cells[1, 5].Value = "Visitor";

                    recordIndex = 2;

                    int a = 1;
                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = a;
                        WorkSheet1.Cells[recordIndex, 2].Value = Convert.ToDateTime(items.CheckInDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 3].Value = items.Employee;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.Worker;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.Visitor;

                        recordIndex += 1;

                        a++;
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
                _response.Message = "Data Exported successfully";
            }

            return _response;
        }

        #endregion
    }
}
