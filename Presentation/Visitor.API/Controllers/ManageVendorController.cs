using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageVendorController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageVendorRepository _manageVendorRepository;
        private IFileManager _fileManager;

        public ManageVendorController(IManageVendorRepository manageVendorRepository, IFileManager fileManager)
        {
            _manageVendorRepository = manageVendorRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVendor(Vendor_Request parameters)
        {
            // PanCard Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.PanCard_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.PanCard_Base64, "\\Uploads\\Vendor\\", parameters.PanCardOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.PanCardFileName = vUploadFile;
                }
            }

            // GST Upload
            if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.GST_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.GST_Base64, "\\Uploads\\Vendor\\", parameters.GSTOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.GSTFileName = vUploadFile;
                }
            }

            int result = await _manageVendorRepository.SaveVendor(parameters);

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
        public async Task<ResponseModel> GetVendorList(Vendor_Search parameters)
        {
            IEnumerable<Vendor_Response> lstRoles = await _manageVendorRepository.GetVendorList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVendorById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageVendorRepository.GetVendorById(Id);

                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
