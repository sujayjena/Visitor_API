using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageContractorController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageContractorRepository _manageContractorRepository;
        private IFileManager _fileManager;

        public ManageContractorController(IManageContractorRepository manageContractorRepository, IFileManager fileManager)
        {
            _manageContractorRepository = manageContractorRepository;
            _fileManager = fileManager;

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

            int result = await _manageContractorRepository.SaveContractor(parameters);

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
                _response.Data = vResultObj;
            }
            return _response;
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
    }
}
