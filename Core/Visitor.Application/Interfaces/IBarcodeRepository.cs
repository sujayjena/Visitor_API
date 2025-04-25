using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IBarcodeRepository
    {
        BarcodeGenerate_Response GenerateBarcode(string value);
        Task<int> SaveBarcode(Barcode_Request parameters);
        Task<Barcode_Response?> GetBarcodeById(string BarcodeNo);
    }
}
