using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using System.Net.Mail;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageContractorController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageContractorRepository _manageContractorRepository;
        private readonly IManageVisitorsRepository _manageVisitorsRepository;
        private IFileManager _fileManager;
        private readonly IWebHostEnvironment _environment;
        private IEmailHelper _emailHelper;
        private readonly IUserRepository _userRepository;

        public ManageContractorController(IManageContractorRepository manageContractorRepository, IFileManager fileManager, IManageVisitorsRepository manageVisitorsRepository, IWebHostEnvironment environment, IEmailHelper emailHelper, IUserRepository userRepository)
        {
            _manageContractorRepository = manageContractorRepository;
            _fileManager = fileManager;
            _manageVisitorsRepository = manageVisitorsRepository;
            _environment = environment;
            _emailHelper = emailHelper;
            _userRepository = userRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Contractor
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveContractor(Contractor_Request parameters)
        {
            // Document Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.DocumentImage_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.DocumentImage_Base64, "\\Uploads\\Contractor\\", parameters.DocumentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DocumentImage = vUploadFile;
                }
            }

            // Visitor Photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.ContractorPhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.ContractorPhoto_Base64, "\\Uploads\\Contractor\\", parameters.ContractorPhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.ContractorPhoto = vUploadFile;
                }
            }

            // Aadhar Card Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.AadharCard_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.AadharCard_Base64, "\\Uploads\\Contractor\\", parameters.AadharCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.AadharCardFileName = vUploadFile;
                }
            }

            // Pan Card Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.PanCard_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.PanCard_Base64, "\\Uploads\\Contractor\\", parameters.PanCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PanCardFileName = vUploadFile;
                }
            }

            //insurance Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.DV_Insurance_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.DV_Insurance_Base64, "\\Uploads\\Contractor\\", parameters.DV_InsuranceOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DV_InsuranceFileName = vUploadFile;
                }
            }

            //wc Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.DV_WC_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.DV_WC_Base64, "\\Uploads\\Contractor\\", parameters.DV_WCOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DV_WCFileName = vUploadFile;
                }
            }

            //esic Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.DV_ESIC_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.DV_ESIC_Base64, "\\Uploads\\Contractor\\", parameters.DV_ESICOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DV_ESICFileName = vUploadFile;
                }
            }

            int result = await _manageContractorRepository.SaveContractor(parameters);

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

                #region Contractor Asset
                if (result > 0)
                {
                    foreach (var items in parameters.assetList)
                    {
                        var vContractorAsset_Request = new ContractorAsset_Request()
                        {
                            Id = items.Id,
                            ContractorId = result,
                            AssetName = items.AssetName,
                            AssetDesc = items.AssetDesc,
                            Quantity = items.Quantity,
                            UOMId = items.UOMId
                        };

                        int resultSaveContractor = await _manageContractorRepository.SaveContractorAsset(vContractorAsset_Request);
                    }

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
                        RefType = "Contractor",
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

                #region Document Approval Email 
                if(parameters.DV_IsInsurance == true && parameters.DV_IsWC == true && parameters.DV_IsESIC == true && parameters.Id > 0)
                {
                    var vEmailEmployee = await SendContractorDocumentApproval_EmailToEmployee(result);
                }
                #endregion
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContractorList(ContractorSearch_Request parameters)
        {
            IEnumerable<Contractor_Response> lstContractors = await _manageContractorRepository.GetContractorList(parameters);
            _response.Data = lstContractors.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContractorById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageContractorRepository.GetContractorById(Id);
                if (vResultObj != null)
                {
                    var vContractorAsset_Search = new ContractorAsset_Search()
                    {
                        ContractorId = vResultObj.Id
                    };
                    var vContractorAsset = await _manageContractorRepository.GetContractorAssetList(vContractorAsset_Search);
                    vResultObj.assetList = vContractorAsset.ToList();

                    var vVisitorDocumentVerification = new VisitorDocumentVerification_Search()
                    {
                        RefId = vResultObj.Id,
                        RefType = "Contractor"
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
        public async Task<ResponseModel> ExportContractorData(ContractorSearch_Request parameters)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Contractor_Response> lstData = await _manageContractorRepository.GetContractorList(parameters);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Contractor");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Contractor Level";
                    WorkSheet1.Cells[1, 2].Value = "Contractor Type";
                    WorkSheet1.Cells[1, 3].Value = "Contractor Firm";
                    WorkSheet1.Cells[1, 4].Value = "Contractor Person Name";
                    WorkSheet1.Cells[1, 5].Value = "Mobile Number";
                    WorkSheet1.Cells[1, 6].Value = "Valid From";
                    WorkSheet1.Cells[1, 7].Value = "Valid To";
                    WorkSheet1.Cells[1, 8].Value = "Address";
                    WorkSheet1.Cells[1, 9].Value = "Country";
                    WorkSheet1.Cells[1, 10].Value = "State";
                    WorkSheet1.Cells[1, 11].Value = "Province";
                    WorkSheet1.Cells[1, 12].Value = "Pincode";
                    WorkSheet1.Cells[1, 13].Value = "Insurance Available?";
                    WorkSheet1.Cells[1, 14].Value = "Insurance Number";
                    WorkSheet1.Cells[1, 15].Value = "Workmen Compensation (WC)?";
                    WorkSheet1.Cells[1, 16].Value = "WC Number";
                    WorkSheet1.Cells[1, 17].Value = "ESIC Available?";
                    WorkSheet1.Cells[1, 18].Value = "ESIC Number";
                    WorkSheet1.Cells[1, 19].Value = "Branch";
                    WorkSheet1.Cells[1, 20].Value = "Department";
                    WorkSheet1.Cells[1, 21].Value = "Employee Name";
                    WorkSheet1.Cells[1, 22].Value = "Blacklisted?";
                    WorkSheet1.Cells[1, 23].Value = "Status";
                    WorkSheet1.Cells[1, 24].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 25].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstData)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.ContractorLevel == 1 ? "INHOUSE" : items.ContractorLevel == 2 ? "OUTSIDE" : "";
                        WorkSheet1.Cells[recordIndex, 2].Value = items.ContractorType;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.ContractorName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.ContractorPerson;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.MobileNo;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.ValidFromDate.HasValue? items.ValidFromDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.ValidToDate.HasValue? items.ValidToDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.AddressLine;
                        WorkSheet1.Cells[recordIndex, 9].Value = items.CountryName;
                        WorkSheet1.Cells[recordIndex, 10].Value =items.StateName;
                        WorkSheet1.Cells[recordIndex, 11].Value =items.DistrictName;
                        WorkSheet1.Cells[recordIndex, 12].Value =items.Pincode;
                        WorkSheet1.Cells[recordIndex, 13].Value =items.DV_IsInsurance == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 14].Value =items.DV_InsuranceNumber;
                        WorkSheet1.Cells[recordIndex, 15].Value =items.DV_IsWC == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 16].Value =items.DV_WCNumber;
                        WorkSheet1.Cells[recordIndex, 17].Value =items.DV_IsESIC == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 18].Value =items.DV_ESICNumber;
                        WorkSheet1.Cells[recordIndex, 19].Value =items.BranchName;
                        WorkSheet1.Cells[recordIndex, 20].Value =items.DepartmentName;
                        WorkSheet1.Cells[recordIndex, 21].Value =items.EmployeeName;
                        WorkSheet1.Cells[recordIndex, 22].Value =items.IsBlackList == true ? "Yes" : "No";
                        WorkSheet1.Cells[recordIndex, 23].Value =items.IsActive == true ? "Active" : "Inactive";
                        WorkSheet1.Cells[recordIndex, 24].Value =Convert.ToDateTime(items.CreatedDate).ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 25].Value = items.CreatorName;

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

        protected async Task<bool> SendContractorDocumentApproval_EmailToEmployee(int id)
        {
            bool result = false;
            string templateFilePath = "", emailTemplateContent = "", sSubjectDynamicContent = "";

            try
            {
                string recipientEmail = "";
                string moduleName = "";
                var vfiles = new List<Attachment>();

                var vContractor = await _manageContractorRepository.GetContractorById(id); //get contact list

                if (vContractor != null)
                {
                    var vSearch = new User_Search()
                    {
                        UserTypeId = 0,
                        BranchId = 0
                    };

                    List<string> vRoleName = new List<string>() { "SR EXECUTIVE-HR", "IR & PR" };

                    var vUserList = await _userRepository.GetUserList(vSearch); //get employee list
                    if (vUserList.ToList().Count > 0)
                    {
                        var vUserListFilter = vUserList.ToList().Where(x => vRoleName.Contains(x.RoleName));
                        if (vUserListFilter.ToList().Count > 0)
                        {
                            recipientEmail = string.Join(",", new List<string>(vUserListFilter.ToList().Select(x => x.EmailId)).ToArray());
                        }
                    }

                    var vInsurance = _environment.ContentRootPath + "\\Uploads\\Contractor\\" + vContractor.DV_InsuranceFileName;
                    var vWC = _environment.ContentRootPath + "\\Uploads\\Contractor\\" + vContractor.DV_WCFileName;
                    var vESIC = _environment.ContentRootPath + "\\Uploads\\Contractor\\" + vContractor.DV_ESICFileName;

                    vfiles.Add(new Attachment(vInsurance));
                    vfiles.Add(new Attachment(vWC));
                    vfiles.Add(new Attachment(vESIC));
                }

                templateFilePath = _environment.ContentRootPath + "\\EmailTemplates\\Document_Approved_Template.html";
                emailTemplateContent = System.IO.File.ReadAllText(templateFilePath);

                if (emailTemplateContent.IndexOf("[RefType]", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    emailTemplateContent = emailTemplateContent.Replace("[RefType]", "Contractor");
                }

                if (emailTemplateContent.IndexOf("[Documents]", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    emailTemplateContent = emailTemplateContent.Replace("[Documents]", "Insurance, WC,  and ESIC");
                }         

                moduleName = "Document Approval - Contractor";
                sSubjectDynamicContent = "Request for Approval of Contractor Documents";
                
                result = await _emailHelper.SendEmail(module: moduleName, subject: sSubjectDynamicContent, sendTo: "Employee", content: emailTemplateContent, recipientEmail: recipientEmail, files: vfiles, remarks: "");
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;

        }

        #endregion

        #region Contractor Insurance
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveContractorInsurance(ContractorInsurance_Request parameters)
        {
            // Pan Card Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Insurance_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Insurance_Base64, "\\Uploads\\Contractor\\", parameters.InsuranceOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.InsuranceFileName = vUploadFile;
                }
            }

            int result = await _manageContractorRepository.SaveContractorInsurance(parameters);

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
        public async Task<ResponseModel> GetContractorInsuranceList(ContractorInsuranceSearch_Request parameters)
        {
            IEnumerable<ContractorInsurance_Response> lstInsurances = await _manageContractorRepository.GetContractorInsuranceList(parameters);
            _response.Data = lstInsurances.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContractorInsuranceById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageContractorRepository.GetContractorInsuranceById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
        #endregion

        #region Contractor Asset

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> DeleteContractorAsset(int Id)
        {
            int result = await _manageContractorRepository.DeleteContractorAsset(Id);

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

        #endregion
    }
}
