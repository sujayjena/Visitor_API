using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Visitor.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly ICompanyRepository _companyRepository;
        private IFileManager _fileManager;

        public CompanyController(ICompanyRepository companyRepository, IFileManager fileManager)
        {
            _companyRepository = companyRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Company 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCompany(Company_Request parameters)
        {
            // Company Logo Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.CompanyLogo_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.CompanyLogo_Base64, "\\Uploads\\Company\\", parameters.CompanyLogoFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.CompanyLogoFileName = vUploadFile;
                }
            }

            int result = await _companyRepository.SaveCompany(parameters);

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
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCompanyList(CompanySearch_Request parameters)
        {
            IEnumerable<Company_Response> lstCompanys = await _companyRepository.GetCompanyList(parameters);
            _response.Data = lstCompanys.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCompanyById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _companyRepository.GetCompanyById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}
