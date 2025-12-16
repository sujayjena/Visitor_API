using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                            string approvalTypeName = (vApprovalType == 1 ? "APPROVER 1" : vApprovalType == 2 ? "APPROVER 2" : "");

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

        #endregion
    }
}
