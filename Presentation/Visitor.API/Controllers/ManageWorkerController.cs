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
    public class ManageWorkerController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageWorkerRepository _manageWorkerRepository;
        private readonly IManageContractorRepository _manageContractorRepository;
        private readonly IAssignGateNoRepository _assignGateNoRepository;
        private IFileManager _fileManager;

        public ManageWorkerController(IManageWorkerRepository manageWorkerRepository, IFileManager fileManager, IManageContractorRepository manageContractorRepository, IAssignGateNoRepository assignGateNoRepository)
        {
            _manageWorkerRepository = manageWorkerRepository;
            _fileManager = fileManager;
            _manageContractorRepository = manageContractorRepository;
            _assignGateNoRepository = assignGateNoRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWorker(Worker_Request parameters)
        {
            #region User Restriction 

            int vNoofContractedWorker = 0;
            int totalWorkderRegistered = 0;

            if (parameters.Id == 0)
            {
                var vWorkerSearch = new WorkerSearch_Request();
                vWorkerSearch.ContractorId = 0;
                vWorkerSearch.IsBlackList = false;
                vWorkerSearch.IsActive = true;

                var vWorker = await _manageWorkerRepository.GetWorkerList(vWorkerSearch);
              
                #region Contractor Wise Worker Check

                if (parameters.ContractorId > 0)
                {
                    //get total worker count
                    totalWorkderRegistered = vWorker.Where(x => x.ContractorId == parameters.ContractorId).Count();

                    //get total NoofContractedWorkers 
                    var vContractor = await _manageContractorRepository.GetContractorById(Convert.ToInt32(parameters.ContractorId));
                    if (vContractor != null)
                    {
                        vNoofContractedWorker = vContractor.NoofContractedWorkers ?? 0;
                    }
                }

                // Total Contractor check with register worker
                if (totalWorkderRegistered >= vNoofContractedWorker)
                {
                    _response.Message = "You are not allowed to create worker more than " + vNoofContractedWorker + ", Please contact your administrator to access this feature!";
                    return _response;
                }

                #endregion
            }

            #endregion


            int result = await _manageWorkerRepository.SaveWorker(parameters);

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

                #region // Add/Update Assign GateNo

                // Delete Assign
                var vGateNoDELETEObj = new AssignGateNo_Request()
                {
                    Action = "DELETE",
                    RefId = result,
                    RefType = "Worker",
                    GateDetailsId = 0
                };
                int resultGateNoDELETE = await _assignGateNoRepository.SaveAssignGateNo(vGateNoDELETEObj);


                // add new Visitor field
                foreach (var vGateitem in parameters.GateNumberList)
                {
                    var vGateNoMapObj = new AssignGateNo_Request()
                    {
                        Action = "INSERT",
                        RefId = result,
                        RefType = "Worker",
                        GateDetailsId = vGateitem.GateDetailsId
                    };

                    int resultGateNo = await _assignGateNoRepository.SaveAssignGateNo(vGateNoMapObj);
                }

                #endregion
            }
            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerList(WorkerSearch_Request parameters)
        {
            IEnumerable<Worker_Response> lstWorkers = await _manageWorkerRepository.GetWorkerList(parameters);
            _response.Data = lstWorkers.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWorkerById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageWorkerRepository.GetWorkerById(Id);
                if(vResultObj != null)
                {
                    var gateNolistObj = await _assignGateNoRepository.GetAssignGateNoById(vResultObj.Id, "Worker", 0);
                    vResultObj.GateNumberList = gateNolistObj.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }
    }
}
