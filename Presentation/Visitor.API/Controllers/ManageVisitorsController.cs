﻿using Microsoft.AspNetCore.Http;
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
    public class ManageVisitorsController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageVisitorsRepository _manageVisitorsRepository;
        private IFileManager _fileManager;

        public ManageVisitorsController(IManageVisitorsRepository manageVisitorsRepository, IFileManager fileManager)
        {
            _manageVisitorsRepository = manageVisitorsRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVisitors(Visitors_Request parameters)
        {
            // ID Upload
            //if (parameters! != null && !string.IsNullOrWhiteSpace(parameters.IDImage_Base64))
            //{
            //    var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.IDImage_Base64, "\\Uploads\\Visitors\\", parameters.IDOriginalFileName);

            //    if (!string.IsNullOrWhiteSpace(vUploadFile))
            //    {
            //        parameters.IDImage = vUploadFile;
            //    }
            //}

            // Visitor Photo Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.VisitorPhoto_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.VisitorPhoto_Base64, "\\Uploads\\Visitors\\", parameters.VisitorPhotoOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.VisitorPhoto = vUploadFile;
                }
            }

            int result = await _manageVisitorsRepository.SaveVisitors(parameters);

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
                _response.Message = "Record details saved sucessfully";

                #region // Add/Update Visitor GateNo

                // Delete Region of Branch
                var vGateNoDELETEObj = new VisitorGateNo_Request()
                {
                    Action = "DELETE",
                    VisitorId = result,
                    GateDetailsId = 0
                };
                int resultGateNoDELETE = await _manageVisitorsRepository.SaveVisitorsGateNo(vGateNoDELETEObj);


                // add new Visitor field
                foreach (var vGateitem in parameters.GateNumberList)
                {
                    var vGateNoMapObj = new VisitorGateNo_Request()
                    {
                        Action = "INSERT",
                        VisitorId = result,
                        GateDetailsId = vGateitem.GateDetailsId
                    };

                    int resultGateNo = await _manageVisitorsRepository.SaveVisitorsGateNo(vGateNoMapObj);
                }

                #endregion
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorsList(Visitors_Search parameters)
        {
            IEnumerable<Visitors_Response> lstVisitorss = await _manageVisitorsRepository.GetVisitorsList(parameters);
            _response.Data = lstVisitorss.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVisitorsById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageVisitorsRepository.GetVisitorsById(Id);
                if (vResultObj != null)
                {
                    var gateNolistObj = await _manageVisitorsRepository.GetVisitorsGateNoByVisitorId(vResultObj.Id, 0);

                    vResultObj.GateNumberList = gateNolistObj.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

    }
}
