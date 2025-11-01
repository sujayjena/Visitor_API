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
        public async Task<ResponseModel> GetCanteenTransactionList(CanteenTransaction_Search parameters)
        {
            IEnumerable<CanteenTransaction_Response> lstUsers = await _canteenTransactionRepository.GetCanteenTransactionList(parameters);
            _response.Data = lstUsers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
    }
}
