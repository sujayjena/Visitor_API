using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface ICanteenTransactionRepository
    {
        Task<int> SaveCanteenTransaction(CanteenTransaction_Request parameters);
        Task<IEnumerable<CanteenTransaction_Response>> GetCanteenTransactionList(CanteenTransaction_Search parameters);
    }
}
