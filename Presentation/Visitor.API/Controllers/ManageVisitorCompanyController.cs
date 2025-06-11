using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}
