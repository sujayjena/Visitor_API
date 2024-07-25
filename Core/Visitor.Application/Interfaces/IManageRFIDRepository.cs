using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IManageRFIDRepository
    {
        #region RFID
        Task<int> SaveRFID(RFID_Request parameters);

        Task<IEnumerable<RFID_Response>> GetRFIDList(RFID_Search_Request parameters);

        Task<RFID_Response?> GetRFIDById(int Id);

        #endregion

        #region RFID Topup
        Task<int> SaveRFIDTopup(RFIDTopupDetails_Request parameters);

        Task<IEnumerable<RFIDTopup_Response>> GetRFIDTopupList(RFIDTopup_Search_Request parameters);

        #endregion
    }
}
