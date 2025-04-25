using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Domain.Entities;

namespace Visitor.Application.Models
{
    public class BarcodeGenerate_Request
    {
        public string? value { get; set; }
    }
    public class BarcodeGenerate_Response
    {
        public string? Barcode_Unique_Id { get; set; }
        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
    }
    public class Barcode_Request : BaseEntity
    {
        public string? BarcodeNo { get; set; }
        public string? BarcodeType { get; set; }
        public string? Barcode_Unique_Id { get; set; }
        public string? BarcodeOriginalFileName { get; set; }
        public string? BarcodeFileName { get; set; }
        public int? RefId { get; set; }
    }

    public class Barcode_Response : BaseEntity
    {
        public string? BarcodeNo { get; set; }
        public string? BarcodeType { get; set; }
        public string? Barcode_Unique_Id { get; set; }
        public int? RefId { get; set; }
        public string? Validity { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
