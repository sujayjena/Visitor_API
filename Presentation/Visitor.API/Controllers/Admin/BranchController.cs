using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visitor.API.CustomAttributes;

namespace Visitor.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IBranchRepository _branchRepository;
        private readonly ICompanyRepository _companyRepository;
        private IFileManager _fileManager;

        public BranchController(IBranchRepository branchRepository, ICompanyRepository companyRepository, IFileManager fileManager)
        {
            _branchRepository = branchRepository;
            _companyRepository = companyRepository;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Branch 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBranch(Branch_Request parameters)
        {
            #region Branch Restriction 

            if (parameters.Id == 0)
            {
                int iBranchCanAdd = 0;
                var tblCompanies = await _companyRepository.GetCompanyById(parameters.CompanyId ?? 0);
                if (tblCompanies != null)
                {
                    iBranchCanAdd = tblCompanies.NoofBranchAdd ?? 0;
                }

                var bParameter = new BranchSearch_Request();
                var tblBranchesList = await _branchRepository.GetBranchList(bParameter);
                var vBranchList = tblBranchesList.ToList();
                if (vBranchList.Count > 0)
                {
                    if (iBranchCanAdd == vBranchList.Count)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "You are not allowed to create more then " + iBranchCanAdd + " branch, Please contact your administrator to access this feature!";
                        return _response;
                    }
                    else if (vBranchList.Count > iBranchCanAdd)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "You are not allowed to create more then " + iBranchCanAdd + " branch, Please contact your administrator to access this feature!";
                        return _response;
                    }
                }
            }

            #endregion   

            // Branch Upload
            if (parameters != null && !string.IsNullOrWhiteSpace(parameters.Branch_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(parameters.Branch_Base64, "\\Uploads\\Branch\\", parameters.BranchOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    parameters.BranchFileName = vUploadFile;
                }
            }

            int result = await _branchRepository.SaveBranch(parameters);

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
        [AllowAnonymous]
        public async Task<ResponseModel> GetBranchList(BranchSearch_Request parameters)
        {
            IEnumerable<Branch_Response> lstBranchs = await _branchRepository.GetBranchList(parameters);
            _response.Data = lstBranchs.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBranchById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _branchRepository.GetBranchById(Id);
                _response.Data = vResultObj;
            }
            return _response;
        }

        #endregion
    }
}
