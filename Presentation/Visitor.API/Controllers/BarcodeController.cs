using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarcodeController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IBarcodeRepository _barcodeRepository;

        public BarcodeController(IBarcodeRepository barcodeRepository)
        {
            _barcodeRepository = barcodeRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBarcodeById(string BarcodeNo)
        {
            if (BarcodeNo == "")
            {
                _response.Message = "Barcode No. is required";
            }
            else
            {
                var vResultObj = await _barcodeRepository.GetBarcodeById(BarcodeNo);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
