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
    public class ManagePurchaseOrderController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManagePurchaseOrderRepository _managePurchaseOrderRepository;
        private readonly IManageContractorRepository _manageContractorRepository;
        private readonly IAssignGateNoRepository _assignGateNoRepository;
        private readonly IBarcodeRepository _barcodeRepository;
        private IFileManager _fileManager;

        public ManagePurchaseOrderController(IManagePurchaseOrderRepository managePurchaseOrderRepository, IFileManager fileManager, IManageContractorRepository manageContractorRepository, IAssignGateNoRepository assignGateNoRepository, IBarcodeRepository barcodeRepository)
        {
            _managePurchaseOrderRepository = managePurchaseOrderRepository;
            _fileManager = fileManager;
            _manageContractorRepository = manageContractorRepository;
            _assignGateNoRepository = assignGateNoRepository;
            _barcodeRepository = barcodeRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePurchaseOrder(PurchaseOrder_Request parameters)
        {
            #region User Restriction 

            int vNoofContractedPO = 0;
            int totalPORegistered = 0;

            if (parameters.Id == 0)
            {
                var vPurchaseOrderSearch_Request = new PurchaseOrderSearch_Request();
                vPurchaseOrderSearch_Request.ContractorId = 0;
                vPurchaseOrderSearch_Request.IsActive = true;

                var vPO = await _managePurchaseOrderRepository.GetPurchaseOrderList(vPurchaseOrderSearch_Request);

                #region Contractor Wise PO Check

                if (parameters.ContractorId > 0)
                {
                    //get total po count
                    totalPORegistered = vPO.Where(x => x.ContractorId == parameters.ContractorId).Count();

                    //get total NoofContractedPO 
                    var vContractor = await _manageContractorRepository.GetContractorById(Convert.ToInt32(parameters.ContractorId));
                    if (vContractor != null)
                    {
                        vNoofContractedPO = vContractor.NoofContractedPO ?? 0;
                    }
                }

                // Total Contractor check with register worker
                if (totalPORegistered >= vNoofContractedPO)
                {
                    _response.Message = "You are not allowed to create po more than " + vNoofContractedPO + ", Please contact your administrator to access this feature!";
                    return _response;
                }

                #endregion
            }

            #endregion

            //Document Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Document_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Document_Base64, "\\Uploads\\PurchaseOrder\\", parameters.DocumentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.DocumentFileName = vUploadFile;
                }
            }

            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.POAttachment_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.POAttachment_Base64, "\\Uploads\\PurchaseOrder\\", parameters.POAttachmentOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.POAttachmentFileName = vUploadFile;
                }
            }

            int result = await _managePurchaseOrderRepository.SavePurchaseOrder(parameters);

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
        public async Task<ResponseModel> GetPurchaseOrderList(PurchaseOrderSearch_Request parameters)
        {
            IEnumerable<PurchaseOrder_Response> lstPurchaseOrders = await _managePurchaseOrderRepository.GetPurchaseOrderList(parameters);
            _response.Data = lstPurchaseOrders.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPurchaseOrderById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _managePurchaseOrderRepository.GetPurchaseOrderById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
