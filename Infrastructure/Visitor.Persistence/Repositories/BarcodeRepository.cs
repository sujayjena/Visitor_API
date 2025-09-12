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
    public class BarcodeRepository : GenericRepository, IBarcodeRepository
    {
        private IConfiguration _configuration;
        private IFileManager _fileManager;

        public BarcodeRepository(IConfiguration configuration, IFileManager fileManager) : base(configuration)
        {
            _configuration = configuration;
            _fileManager = fileManager;
        }

        public BarcodeGenerate_Response GenerateBarcode(string value)
        {
            //Prepare you post parameters  
            var postData = new BarcodeGenerate_Request()
            {
                value = value
            };

            //Call API
            string sendUri = "http://164.52.213.175:5050/generate_barcode_v2";

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

            var vBarcodeGenerate = new BarcodeGenerate_Response();

            if (status == true)
            {
                var barcode = jsonResults["barcode"];

                var barcode_image_base64 = barcode.ContainsKey("barcode_image_base64") ? barcode.barcode_image_base64 : string.Empty;
                var vbarcode_image_base64 = Convert.ToString(barcode_image_base64);

                var unique_id = barcode.ContainsKey("unique_id") ? barcode.unique_id : string.Empty;
                var vUniqueId = Convert.ToString(unique_id);

                if (!string.IsNullOrWhiteSpace(vbarcode_image_base64))
                {
                    var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(vbarcode_image_base64, "\\Uploads\\Barcode\\", vUniqueId + ".png");
                    if (!string.IsNullOrWhiteSpace(vUploadFile))
                    {
                        vBarcodeGenerate.Barcode_Unique_Id = vUniqueId;
                        vBarcodeGenerate.BarcodeOriginalFileName = vUniqueId + ".png";
                        vBarcodeGenerate.BarcodeFileName = vUploadFile;

                    }
                }
            }

            return vBarcodeGenerate;
        }

        public async Task<int> SaveBarcode(Barcode_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@BarcodeNo", parameters.BarcodeNo);
            queryParameters.Add("@BarcodeType", parameters.BarcodeType);
            queryParameters.Add("@Barcode_Unique_Id", parameters.Barcode_Unique_Id);
            queryParameters.Add("@BarcodeOriginalFileName", parameters.BarcodeOriginalFileName);
            queryParameters.Add("@BarcodeFileName", parameters.BarcodeFileName);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@RefId", parameters.RefId);

            return await SaveByStoredProcedure<int>("SaveBarcode", queryParameters);
        }

        public async Task<Barcode_Response?> GetBarcodeById(string BarcodeNo)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BarcodeNo", BarcodeNo);

            return (await ListByStoredProcedure<Barcode_Response>("GetBarcodeById", queryParameters)).FirstOrDefault();
        }

        public async Task<string?> AutoBarcodeGenerate(int BranchId, string BarcodeType, string BarcodeNumber)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BranchId", BranchId);
            queryParameters.Add("@BarcodeType", BarcodeType);
            queryParameters.Add("@BarcodeNumber", BarcodeNumber, null, System.Data.ParameterDirection.Output);

            var result = await SaveByStoredProcedure<string>("sp_AutoBarcodeGenerate", queryParameters);
            var vBarcodeNumber = queryParameters.Get<string>("BarcodeNumber");

            return vBarcodeNumber;
        }
    }
}
