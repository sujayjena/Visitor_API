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
    public class ManageRFIDController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageRFIDRepository _manageRFIDRepository;
        private readonly IFileManager _fileManager;

        public ManageRFIDController(IManageRFIDRepository manageRFIDRepository, IFileManager fileManager)
        {
            _manageRFIDRepository = manageRFIDRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region RFID

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRFID(RFID_Request parameters)
        {
            int result = await _manageRFIDRepository.SaveRFID(parameters);

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
        public async Task<ResponseModel> GetRFIDList(RFID_Search_Request parameters)
        {
            IEnumerable<RFID_Response> lstRoles = await _manageRFIDRepository.GetRFIDList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRFIDById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageRFIDRepository.GetRFIDById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion

        #region RFID Topup
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRFIDTopup(RFIDTopup_Request parameters)
        {
            int result = 0;
            if (parameters != null)
            {
                foreach (var items in parameters.rfidTopupList)
                {
                    var vRFIDTopupDetails = new RFIDTopupDetails_Request()
                    {
                        Id=items.Id,
                        RFIDId = items.RFIDId,
                        MenuItemId = items.MenuItemId,
                        Amount=items.Amount,
                    };
                    result = await _manageRFIDRepository.SaveRFIDTopup(vRFIDTopupDetails);
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
        public async Task<ResponseModel> GetRFIDTopupList(RFIDTopup_Search_Request parameters)
        {
            IEnumerable<RFIDTopup_Response> lstRoles = await _manageRFIDRepository.GetRFIDTopupList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
        #endregion
    }
}
