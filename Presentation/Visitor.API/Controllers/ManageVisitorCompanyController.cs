using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
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
    public class ManageVisitorCompanyController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageVisitorCompanyRepository _manageVisitorCompanyRepository;
        private IFileManager _fileManager;

        public ManageVisitorCompanyController(IManageVisitorCompanyRepository manageVisitorCompanyRepository, IFileManager fileManager)
        {
            _manageVisitorCompanyRepository = manageVisitorCompanyRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
          
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseModel> SaveVisitorCompany(VisitorCompany_Request parameters)
        {
            // GSt Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.GSTFile_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.GSTFile_Base64, "\\Uploads\\Visitors\\", parameters.GSTOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.GSTFileName = vUploadFile;
                }
            }

            // Pan Card Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.PanCardFile_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.PanCardFile_Base64, "\\Uploads\\Visitors\\", parameters.PanCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PanCardFileName = vUploadFile;
                }
            }

            int result = await _manageVisitorCompanyRepository.SaveVisitorCompany(parameters);

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

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorCompanyList(BaseSearchEntity parameters)
        {
            IEnumerable<VisitorCompany_Response> lstVisitorCompanys = await _manageVisitorCompanyRepository.GetVisitorCompanyList(parameters);
            _response.Data = lstVisitorCompanys.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorCompanyById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageVisitorCompanyRepository.GetVisitorCompanyById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportVisitorCompanyData(BaseSearchEntity parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<VisitorCompany_Response> lstData = await _manageVisitorCompanyRepository.GetVisitorCompanyList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("VisitorCompany");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Company Name";
                    WorkSheet1.Cells[1, 2].Value = "Address";
                    WorkSheet1.Cells[1, 3].Value = "Country";
                    WorkSheet1.Cells[1, 4].Value = "State";
                    WorkSheet1.Cells[1, 5].Value = "Province";
                    WorkSheet1.Cells[1, 6].Value = "Pincode";
                    WorkSheet1.Cells[1, 7].Value = "Company Phone";
                    WorkSheet1.Cells[1, 8].Value = "GST";
                    WorkSheet1.Cells[1, 9].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 10].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstData)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.CompanyName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.CompanyAddress;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.StateName;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.Pincode;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.CompanyPhone;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.GSTNo;

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
