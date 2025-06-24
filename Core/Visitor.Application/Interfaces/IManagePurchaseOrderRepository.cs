using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IManagePurchaseOrderRepository
    {
        Task<int> SavePurchaseOrder(PurchaseOrder_Request parameters);
        Task<IEnumerable<PurchaseOrder_Response>> GetPurchaseOrderList(PurchaseOrderSearch_Request parameters);
        Task<PurchaseOrder_Response?> GetPurchaseOrderById(int Id);
    }
}
