using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;

namespace Visitor.Persistence.Repositories
{
    public class QRCodeRepository : GenericRepository, IQRCodeRepository
    {
        private IConfiguration _configuration;
        private IFileManager _fileManager;

        public QRCodeRepository(IConfiguration configuration, IFileManager fileManager) : base(configuration)
        {
            _configuration = configuration;
            _fileManager = fileManager;
        }

        public QRCodeGenerate_Response GenerateQRCode(string value)
        {
            //Prepare you post parameters  
            var postData = new QRCodeGenerate_Request()
            {
                value = value
            };

            //Call API
            string sendUri = "http://164.52.213.175:5050/generate_qrcode_v2";

            //Create HTTPWebrequest  
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendUri);

            var jsonData = JsonConvert.SerializeObject(postData);

            //Prepare and Add URL Encoded data  
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(jsonData);

            //Specify post method  
            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/json";
            httpWReq.ContentLength = data.Length;
            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            //Get the response  
            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseString = reader.ReadToEnd();

            //Close the response  
            reader.Close();

            response.Close();

            dynamic jsonResults = JsonConvert.DeserializeObject<dynamic>(responseString);
            var status = jsonResults.ContainsKey("isSuccess") ? jsonResults.isSuccess : false;

            var vQRCodeGenerate = new QRCodeGenerate_Response();

            if (status == true)
            {
                var qr_code = jsonResults["qr_code"];

                var qrcode_image_base64 = qr_code.ContainsKey("qr_code_image_base64") ? qr_code.qr_code_image_base64 : string.Empty;
                var vQRCode_image_base64 = Convert.ToString(qrcode_image_base64);

                var unique_id = qr_code.ContainsKey("unique_id") ? qr_code.unique_id : string.Empty;
                var vUniqueId = Convert.ToString(unique_id);

                if (!string.IsNullOrWhiteSpace(vQRCode_image_base64))
                {
                    var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(vQRCode_image_base64, "\\Uploads\\QRCode\\", vUniqueId + ".png");
                    if (!string.IsNullOrWhiteSpace(vUploadFile))
                    {
                        vQRCodeGenerate.QRCode_Unique_Id = vUniqueId;
                        vQRCodeGenerate.QRCodeOriginalFileName = vUniqueId + ".png";
                        vQRCodeGenerate.QRCodeFileName = vUploadFile;

                    }
                }
            }

            return vQRCodeGenerate;
        }

        public async Task<int> SaveQRCode(QRCode_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@QRCodeNo", parameters.QRCodeNo);
            queryParameters.Add("@QRCode_Unique_Id", parameters.QRCode_Unique_Id);
            queryParameters.Add("@QRCodeOriginalFileName", parameters.QRCodeOriginalFileName);
            queryParameters.Add("@QRCodeFileName", parameters.QRCodeFileName);

            return await SaveByStoredProcedure<int>("SaveQRCode", queryParameters);
        }

        public async Task<QRCode_Response?> GetQRCodeById(string QRCodeNo)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@QRCodeNo", QRCodeNo);

            return (await ListByStoredProcedure<QRCode_Response>("GetQRCodeById", queryParameters)).FirstOrDefault();
        }
    }
}
