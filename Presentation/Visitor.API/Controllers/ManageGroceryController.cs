using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Visitor.Application.Enums;
using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageGroceryController : CustomBaseController
    {
        private ResponseModel _response;
        private readonly IManageGroceryRepository _manageGroceryRepository;
        private readonly IAdminMasterRepository _adminMasterRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public ManageGroceryController(IManageGroceryRepository manageGroceryRepository, IAdminMasterRepository adminMasterRepository, INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _manageGroceryRepository = manageGroceryRepository;
            _adminMasterRepository = adminMasterRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Grocery Requisition
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGroceryRequisition(GroceryRequisition_Request parameters)
        {
            int result = await _manageGroceryRepository.SaveGroceryRequisition(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
            }
            else
            {
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }

                #region Grocery Requisition Details
                if (result > 0)
                {
                    foreach (var items in parameters.GroceryRequisitionDetails)
                    {
                        var vGroceryRequisitionDetails = new GroceryRequisitionDetails_Request()
                        {
                            Id = items.Id,
                            GroceryRequisitionId = result,
                            GroceryId = items.GroceryId,
                            OrderQty = items.OrderQty,
                            ReceivedQty = parameters.IsReceived == true ? items.OrderQty : 0,
                            IsOK = parameters.IsReceived == true ? 1 : 0,
                            LOTNumber = items.LOTNumber,
                            ExpiryDate = items.ExpiryDate,
                        };

                        int resultUserOtherDetails = await _manageGroceryRepository.SaveGroceryRequisitionDetails(vGroceryRequisitionDetails);
                    }
                }
                #endregion

                #region  Notification
                var vGroceryRequisition = await _manageGroceryRepository.GetGroceryRequisitionById(result);
                if (vGroceryRequisition != null && parameters.Id == 0)
                {
                    string notifyMessage = String.Format(@"Requisition ID {0} is raised for approval.", vGroceryRequisition.RequisitionId);

                    var vSearch = new GroceryApproval_Search()
                    {
                        IsActive = true,
                    };

                    var vGroceryApprovalList = await _adminMasterRepository.GetGroceryApprovalList(vSearch); //get grocery approval list
                    var vEmployeeId = vGroceryApprovalList.Where(x => x.ApprovalType == 1).Select(x => x.EmployeeId).FirstOrDefault();
                    if (vEmployeeId != null)
                    {
                        var vNotifyObj = new Notification_Request()
                        {
                            Subject = "Grocery Approval",
                            SendTo = "Employee (Approver 1)",
                            //CustomerId = vWorkOrderObj.CustomerId,
                            //CustomerMessage = NotifyMessage,
                            EmployeeId = vEmployeeId,
                            EmployeeMessage = notifyMessage,
                            RefValue1 = vGroceryRequisition.RequisitionId,
                            ReadUnread = false
                        };

                        int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                    }
                }
                #endregion
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryRequisitionList(GroceryRequisition_Search parameters)
        {
            IEnumerable<GroceryRequisitionList_Response> lstRoles = await _manageGroceryRepository.GetGroceryRequisitionList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryRequisitionById(int Id)
        {
            if (Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                var vResultObj = await _manageGroceryRepository.GetGroceryRequisitionById(Id);
                if (vResultObj != null)
                {
                    var vGroceryRequisitionDetails = new GroceryRequisitionDetails_Search()
                    {
                        GroceryRequisitionId = vResultObj.Id,
                        IsOk = 0,
                    };

                    var vGroceryRequisitionDetailsList = await _manageGroceryRepository.GetGroceryRequisitionDetailsList(vGroceryRequisitionDetails);
                    vResultObj.GroceryRequisitionDetails = vGroceryRequisitionDetailsList.ToList();
                }
                _response.Data = vResultObj;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GroceryRequisitionApproveNReject(GroceryRequisition_ApproveNReject parameters)
        {
            if (parameters.Id <= 0)
            {
                _response.Message = "Id is required";
            }
            else
            {
                int resultExpenseDetails = await _manageGroceryRepository.GroceryRequisitionApproveNReject(parameters);

                if (resultExpenseDetails == (int)SaveOperationEnums.NoRecordExists)
                {
                    _response.Message = "No record exists";
                }
                else if (resultExpenseDetails == (int)SaveOperationEnums.ReocrdExists)
                {
                    _response.Message = "Record already exists";
                }
                else if (resultExpenseDetails == (int)SaveOperationEnums.NoResult)
                {
                    _response.Message = "Something went wrong, please try again";
                }
                else
                {
                    if (parameters.StatusId == 2)
                    {
                        _response.Message = "Grocery Requisition Approved successfully";
                    }
                    else if (parameters.StatusId == 3)
                    {
                        _response.Message = "Grocery Requisition Rejected successfully";
                    }

                    #region  Notification
                    var vGroceryRequisition = await _manageGroceryRepository.GetGroceryRequisitionById(Convert.ToInt32(parameters.Id));
                    if (vGroceryRequisition != null)
                    {
                        var vSearch = new GroceryApproval_Search()
                        {
                            IsActive = true,
                        };

                        var vGroceryApprovalList = await _adminMasterRepository.GetGroceryApprovalList(vSearch); //get grocery approval list

                        if (parameters.StatusId == 2)
                        {
                            if (vGroceryRequisition.Approver1_Id > 0 && (vGroceryRequisition.Approver2_Id == null || vGroceryRequisition.Approver2_Id == 0))
                            {
                                string notifyMessage = String.Format(@"Requisition ID {0} is raised for approval.", vGroceryRequisition.RequisitionId);

                                var vEmployeeId = vGroceryApprovalList.Where(x => x.ApprovalType == 2).Select(x => x.EmployeeId).FirstOrDefault();
                                if (vEmployeeId != null)
                                {
                                    var vNotifyObj = new Notification_Request()
                                    {
                                        Subject = "Grocery Approval",
                                        SendTo = "Employee (Approver 2)",
                                        //CustomerId = vWorkOrderObj.CustomerId,
                                        //CustomerMessage = NotifyMessage,
                                        EmployeeId = vEmployeeId,
                                        EmployeeMessage = notifyMessage,
                                        RefValue1 = vGroceryRequisition.RequisitionId,
                                        ReadUnread = false
                                    };

                                    int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                                }
                            }
                        }
                        else if (parameters.StatusId == 3)
                        {
                            string approverName = string.Empty;
                            var vUser = await _userRepository.GetUserById(SessionManager.LoggedInUserId);
                            if (vUser != null)
                            {
                                approverName = vUser.UserName;
                            }

                            var vApprovalType = vGroceryApprovalList.Where(x => x.EmployeeId == SessionManager.LoggedInUserId).Select(x => x.ApprovalType).FirstOrDefault();
                            string approvalTypeName = (vApprovalType == 1 ? "PRIMARY APPROVER" : vApprovalType == 2 ? "FINAL APPROVER" : "");

                            string notifyMessage = String.Format(@"This Requisition ID {0} has been rejected by the approver: {1} ({2}), Rejection Remarks {3}.", vGroceryRequisition.RequisitionId, approverName, approvalTypeName, parameters.Remarks);

                            var vCreatorEmployeeId = vGroceryRequisition?.CreatedBy;
                            if (vCreatorEmployeeId != null)
                            {
                                var vNotifyObj = new Notification_Request()
                                {
                                    Subject = "Grocery Approval",
                                    SendTo = "Employee Approver Reject",
                                    //CustomerId = vWorkOrderObj.CustomerId,
                                    //CustomerMessage = NotifyMessage,
                                    EmployeeId = Convert.ToInt32(vCreatorEmployeeId),
                                    EmployeeMessage = notifyMessage,
                                    RefValue1 = vGroceryRequisition.RequisitionId,
                                    ReadUnread = false
                                };

                                int resultNotification = await _notificationRepository.SaveNotification(vNotifyObj);
                            }
                        }
                    }
                    #endregion
                }
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryRequisition_ApproveNRejectHistoryListById(GroceryRequisition_ApproveNRejectHistory_Search parameters)
        {
            IEnumerable<GroceryRequisition_ApproveNRejectHistory_Response> lst = await _manageGroceryRepository.GetGroceryRequisition_ApproveNRejectHistoryListById(parameters);
            _response.Data = lst.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        #endregion

        #region Grocery Inwarding
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGroceryRequisitionDetails(GroceryRequisitionDetails_Request parameters)
        {
            int result = await _manageGroceryRepository.SaveGroceryRequisitionDetails(parameters);

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
            }
            else
            {
                if (parameters.Id > 0)
                {
                    _response.Message = "Record updated successfully";
                }
                else
                {
                    _response.Message = "Record details saved successfully";
                }
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryRequisitionDetailsList(GroceryRequisitionDetails_Search parameters)
        {
            IEnumerable<GroceryRequisitionDetails_Response> lstRoles = await _manageGroceryRepository.GetGroceryRequisitionDetailsList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SendStockPending_Notification()
        {
            int result = await _manageGroceryRepository.SendStockPending_Notification();

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
            }
            else
            {
               _response.Message = "Record details saved successfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SendInwardingItemExpiry_Notification()
        {
            int result = await _manageGroceryRepository.SendInwardingItemExpiry_Notification();

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else if (result == -3)
            {
                _response.Message = "Not Allowed to approved requisition";
            }
            else
            {
                _response.Message = "Record details saved successfully";
            }
            return _response;
        }

        #endregion

        #region Grocery Outwarding
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGroceryOutwarding(List<GroceryOutwarding_Request> parameters)
        {
            int result = 0;

            foreach (var item in parameters)
            {
                var vGroceryOutwarding = new GroceryOutwarding_Request()
                {
                    Id = item.Id,
                    GroceryId = item.GroceryId,
                    AvailableQty = item.AvailableQty,
                    OutwardingQty = item.OutwardingQty,
                    RemainingQty = item.RemainingQty
                };
                result = await _manageGroceryRepository.SaveGroceryOutwarding(vGroceryOutwarding);
            }

            if (result == (int)SaveOperationEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveOperationEnums.ReocrdExists)
            {
                _response.Message = "Record already exists";
            }
            else if (result == (int)SaveOperationEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.Message = "Record details saved successfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGroceryOutwardingList(GroceryOutwarding_Search parameters)
        {
            IEnumerable<GroceryOutwarding_Response> lstRoles = await _manageGroceryRepository.GetGroceryOutwardingList(parameters);
            _response.Data = lstRoles.ToList();
            _response.Total = parameters.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportAvailableStockData(Grocery_Search request)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<Grocery_Response> lstGroceryObj = await _adminMasterRepository.GetGroceryList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("AvailableStock");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "GroceryName";
                    WorkSheet1.Cells[1, 2].Value = "UOMName";
                    WorkSheet1.Cells[1, 3].Value = "AvailableQty";
                    WorkSheet1.Cells[1, 4].Value = "OutwardingQty";
                    WorkSheet1.Cells[1, 5].Value = "RemainingQty";
                    WorkSheet1.Cells[1, 6].Value = "MinQty";
                    WorkSheet1.Cells[1, 7].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 8].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstGroceryObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.GroceryName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.UOMName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.AvailableQty;
                        WorkSheet1.Cells[recordIndex, 4].Value = 0;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.AvailableQty;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.MinQty;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.CreatedDate.ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 8].Value = items.CreatorName;

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Data Exported successfully";
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportOutwardingStockData(GroceryOutwarding_Search request)
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IEnumerable<GroceryOutwarding_Response> lstGroceryObj = await _manageGroceryRepository.GetGroceryOutwardingList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("OutwardingStock");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "OutwardingId";
                    WorkSheet1.Cells[1, 2].Value = "GroceryName";
                    WorkSheet1.Cells[1, 3].Value = "UOM";
                    WorkSheet1.Cells[1, 4].Value = "AvailableQty";
                    WorkSheet1.Cells[1, 5].Value = "OutwardingQty";
                    WorkSheet1.Cells[1, 6].Value = "RemainingQty";
                    WorkSheet1.Cells[1, 7].Value = "MinQty";

                    WorkSheet1.Cells[1, 8].Value = "CreatedDate";
                    WorkSheet1.Cells[1, 9].Value = "CreatedBy";

                    recordIndex = 2;

                    foreach (var items in lstGroceryObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.OutwardingId;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.GroceryName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.UOMName;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.AvailableQty;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.OutwardingQty;
                        WorkSheet1.Cells[recordIndex, 6].Value = items.RemainingQty;
                        WorkSheet1.Cells[recordIndex, 7].Value = items.MinQty;

                        //WorkSheet1.Cells[recordIndex, 5].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 8].Value = items.CreatedDate.ToString("dd/MM/yyyy");
                        WorkSheet1.Cells[recordIndex, 9].Value = items.CreatorName;

                        recordIndex += 1;
                    }

                    WorkSheet1.Columns.AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Data Exported successfully";
            }

            return _response;
        }
        #endregion
    }
}
