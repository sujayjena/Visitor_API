using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Domain.Entities;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Models
{
    public class ManageRFIDModel
    {
    }

    #region RFID
    public class RFID_Request : BaseEntity
    {
        [DefaultValue("Employee")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }

        [DefaultValue("")]
        public string? RFID { get; set; }
        public decimal? BalanceAmt { get; set; }
        public bool? IsActive { get; set; }
    }

    public class RFID_Search_Request : BaseSearchEntity
    {
        [DefaultValue("Employee")]
        public string? RefType { get; set; }
        public int? RefId { get; set; }
    }


    public class RFID_Response : BaseResponseEntity
    {
        public string? RefType { get; set; }
        public int? RefId { get; set; }
        //public string? Name { get; set; }
        public string? RFID { get; set; }
        public decimal? BalanceAmt { get; set; }
        public bool? IsActive { get; set; }
    }
    #endregion

    #region RFID Topup

    public class RFIDTopup_Request : BaseEntity
    {
        public RFIDTopup_Request()
        {
            rfidTopupList = new List<RFIDTopupDetails_Request>();
        }
        public List<RFIDTopupDetails_Request> rfidTopupList { get; set; }
    }

    public class RFIDTopupDetails_Request : BaseEntity
    {
        public int? RFIDId { get; set; }
        public int? MenuItemId { get; set; }
        public decimal? Amount { get; set; }
    }

    public class RFIDTopup_Search_Request : BaseSearchEntity
    {
        public int? RFIDId { get; set; }
    }

    public class RFIDTopup_Response : BaseResponseEntity
    {
        public int? RFIDId { get; set; }
        public string? RFID { get; set; }
        public int? MenuItemId { get; set; }
        public string? MenuItemName { get; set; }
        public decimal? CreditAmt { get; set; }
        public decimal? DebitAmt { get; set; }
        public decimal? BalanceAmt { get; set; }
    }

    #endregion

}
