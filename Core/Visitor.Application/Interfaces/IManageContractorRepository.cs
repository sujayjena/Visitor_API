using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
   
    public interface IManageContractorRepository
    {
        #region Contractor
        Task<int> SaveContractor(Contractor_Request parameters);

        Task<IEnumerable<Contractor_Response>> GetContractorList(ContractorSearch_Request parameters);

        Task<Contractor_Response?> GetContractorById(int Id);
        #endregion

        #region Contractor Insurance
        Task<int> SaveContractorInsurance(ContractorInsurance_Request parameters);

        Task<IEnumerable<ContractorInsurance_Response>> GetContractorInsuranceList(ContractorInsuranceSearch_Request parameters);

        Task<ContractorInsurance_Response?> GetContractorInsuranceById(int Id);
        #endregion

        #region Contractor Asset
        Task<int> SaveContractorAsset(ContractorAsset_Request parameters);
        Task<IEnumerable<ContractorAsset_Response>> GetContractorAssetList(ContractorAsset_Search parameters);
        Task<int> DeleteContractorAsset(int Id);
        #endregion
    }
}
