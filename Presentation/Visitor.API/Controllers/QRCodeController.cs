using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Enums;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IQRCodeRepository _qrCodeRepository;

        public QRCodeController(IQRCodeRepository qrCodeRepository)
        {
            _qrCodeRepository = qrCodeRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveQRCode(QRCodeGenerate_Request parameters)
        {
            var vQRCode_Request = new QRCode_Request();

            var vGenerateQRCode = _qrCodeRepository.GenerateQRCode(parameters.value);
            if (vGenerateQRCode.QRCode_Unique_Id != "")
            {
                vQRCode_Request.Id = 0;
                vQRCode_Request.QRCodeNo = parameters.value;
                vQRCode_Request.QRCode_Unique_Id = vGenerateQRCode.QRCode_Unique_Id;
                vQRCode_Request.QRCodeOriginalFileName = vGenerateQRCode.QRCodeOriginalFileName;
                vQRCode_Request.QRCodeFileName = vGenerateQRCode.QRCodeFileName;
            }

            int result = await _qrCodeRepository.SaveQRCode(vQRCode_Request);

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
                if (vQRCode_Request.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }

                var vQRCodeObj = await _qrCodeRepository.GetQRCodeById(parameters.value);
                _response.Data = vQRCodeObj;
            }
            return _response;
        }
    }
}
