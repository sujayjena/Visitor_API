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
    public class ManageFoodWastageController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageFoodWastageRepository _manageFoodWastageRepository;
        private readonly IFileManager _fileManager;

        public ManageFoodWastageController(IManageFoodWastageRepository manageFoodWastageRepository, IFileManager fileManager)
        {
            _manageFoodWastageRepository = manageFoodWastageRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveFoodWastage(FoodWastage_Request parameters)
        {
            int result = await _manageFoodWastageRepository.SaveFoodWastage(parameters);

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
        public async Task<ResponseModel> GetFoodWastageList(FoodWastage_Search parameters)
        {
            IEnumerable<FoodWastage_Response> lstRoles = await _manageFoodWastageRepository.GetFoodWastageList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetFoodWastageById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageFoodWastageRepository.GetFoodWastageById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportFoodWastageData(FoodWastage_Search parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<FoodWastage_Response> lstObj = await _manageFoodWastageRepository.GetFoodWastageList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("FoodWastage");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Date";
                    WorkSheet1.Cells[1, 2].Value = "Item Name";
                    WorkSheet1.Cells[1, 3].Value = "Quantity";
                    WorkSheet1.Cells[1, 4].Value = "UOM";
                    WorkSheet1.Cells[1, 5].Value = "Created Date";
                    WorkSheet1.Cells[1, 6].Value = "Created By";

                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.FWDate != null ? Convert.ToDateTime(items.FWDate).ToString("dd/MM/yyyy") : ""; 
                        WorkSheet1.Cells[recordIndex, 2].Value = items.ItemName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.Quantity;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.UOMName;
                        WorkSheet1.Cells[recordIndex, 5].Value = Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 6].Value = items.CreatorName;

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
