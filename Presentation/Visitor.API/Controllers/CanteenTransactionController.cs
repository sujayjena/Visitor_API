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
    public class CanteenTransactionController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly ICanteenTransactionRepository _canteenTransactionRepository;
        private IFileManager _fileManager;

        public CanteenTransactionController(ICanteenTransactionRepository canteenTransactionRepository, IFileManager fileManager)
        {
            _canteenTransactionRepository = canteenTransactionRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCanteenTransaction(CanteenTransaction_Request parameters)
        {
            int result = await _canteenTransactionRepository.SaveCanteenTransaction(parameters);

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
        public async Task<ResponseModel> GetCanteenTransactionList(CanteenTransaction_Search parameters)
        {
            IEnumerable<CanteenTransaction_Response> lstUsers = await _canteenTransactionRepository.GetCanteenTransactionList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCanteenTransactionToken(CanteenTransactionToken_Request parameters)
        {
            if (string.IsNullOrEmpty(parameters.MealType))
            {
                _response.IsSuccess = false;
                _response.Message = "Meal Type is required.";
                return _response;
            }

            int result = await _canteenTransactionRepository.SaveCanteenTransactionToken(parameters);

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
                _response.Message = "Record details saved successfully";

                var tokenList = await _canteenTransactionRepository.GetCanteenTransactionTokenById(result);
                _response.Data = tokenList;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCanteenTransactionTokenTest(CanteenTransactionToken_Request parameters)
        {
            if (string.IsNullOrEmpty(parameters.MealType))
            {
                _response.IsSuccess = false;
                _response.Message = "Meal Type is required.";
                return _response;
            }

            int result = 0;

            var tokenList = new List<CanteenTransactionToken_Response>();

            if (parameters.NoofToken > 0)
            {
                for (int i = 0; i < parameters.NoofToken; i++)
                {
                    result = await _canteenTransactionRepository.SaveCanteenTransactionToken(parameters);
                    var vtokenList = await _canteenTransactionRepository.GetCanteenTransactionTokenById(result);
                    if (vtokenList != null)
                    {
                        tokenList.Add(new CanteenTransactionToken_Response
                        {
                            Id = vtokenList.Id,
                            TokenNo = vtokenList.TokenNo
                        });
                    }
                }
            }
            else
            {
                result = await _canteenTransactionRepository.SaveCanteenTransactionToken(parameters);
                var vtokenList = await _canteenTransactionRepository.GetCanteenTransactionTokenById(result);
                if (vtokenList != null)
                {
                    tokenList.Add(new CanteenTransactionToken_Response
                    {
                        Id = vtokenList.Id,
                        TokenNo = vtokenList.TokenNo
                    });
                }
            }

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
                _response.Message = "Record details saved successfully";

                _response.Data = tokenList;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportCanteenTransactionData(CanteenTransaction_Search parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<CanteenTransaction_Response> lstObj = await _canteenTransactionRepository.GetCanteenTransactionList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("CanteenTransaction");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = parameters.RefType == "Employee" ? "Employee Code" : parameters.RefType == "Visitor" ? "Visit Number" : parameters.RefType == "Contractor" ? "Contractor Code" : "";
                    WorkSheet1.Cells[1, 2].Value = parameters.RefType == "Employee" ? "Employee Name" : parameters.RefType == "Visitor" ? "Visitor Name" : parameters.RefType == "Contractor" ? "Contractor Name" : "";
                    WorkSheet1.Cells[1, 3].Value = "Meal Type";
                    WorkSheet1.Cells[1, 4].Value = "Meal Name";
                    WorkSheet1.Cells[1, 5].Value = "Token Code";
                    WorkSheet1.Cells[1, 6].Value = "MRP";
                    WorkSheet1.Cells[1, 7].Value = "Subsidized Price";
                    WorkSheet1.Cells[1, 8].Value = "Branch";
                    WorkSheet1.Cells[1, 9].Value = "Created Date";
                    WorkSheet1.Cells[1, 10].Value = "Created By";

                    recordIndex = 2;

                    foreach (var items in lstObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.RefCode;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.RefName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.MealType;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.MenuItemName;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.TokenNo;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.SellingPrice;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.SubsidizedPrice;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.BranchName;
                        WorkSheet1.Cells[recordIndex, 9].Value = Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 10].Value = items.CreatorName;

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
