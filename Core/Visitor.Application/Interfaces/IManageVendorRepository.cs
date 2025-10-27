using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IManageVendorRepository
    {
        Task<int> SaveVendor(Vendor_Request parameters);
        Task<IEnumerable<Vendor_Response>> GetVendorList(Vendor_Search parameters);
        Task<Vendor_Response?> GetVendorById(int Id);
    }
}
