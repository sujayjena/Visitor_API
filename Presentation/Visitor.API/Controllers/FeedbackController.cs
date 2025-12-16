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
    public class FeedbackController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IFeedbackRepository _feedbackRepository;
        private IFileManager _fileManager;

        public FeedbackController(IFeedbackRepository feedbackRepository, IFileManager fileManager)
        {
            _feedbackRepository = feedbackRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveFeedback(Feedback_Request parameters)
        {
            int result = await _feedbackRepository.SaveFeedback(parameters);

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
        public async Task<ResponseModel> GetFeedbackList(Feedback_Search parameters)
        {
            IEnumerable<Feedback_Response> lstRoles = await _feedbackRepository.GetFeedbackList(parameters);

            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }
    }
}
