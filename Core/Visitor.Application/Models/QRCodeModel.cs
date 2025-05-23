using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Domain.Entities;

namespace Visitor.Application.Models
{
    public class QRCodeGenerate_Request
    {
        public string? value { get; set; }
    }
    public class QRCodeGenerate_Response
    {
        public string? QRCode_Unique_Id { get; set; }
        public string? QRCodeOriginalFileName { get; set; }
        public string? QRCodeFileName { get; set; }
    }
    public class QRCode_Request : BaseEntity
    {
        public string? QRCodeNo { get; set; }
        public string? QRCode_Unique_Id { get; set; }
        public string? QRCodeOriginalFileName { get; set; }
        public string? QRCodeFileName { get; set; }
    }

    public class QRCode_Response : BaseEntity
    {
        public string? QRCodeNo { get; set; }
        public string? QRCode_Unique_Id { get; set; }
        public string? QRCodeOriginalFileName { get; set; }
        public string? QRCodeFileName { get; set; }
        public string? QRCodeURL { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
