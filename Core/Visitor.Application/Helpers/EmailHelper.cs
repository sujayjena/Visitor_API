using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Visitor.Application.Helpers
{
    public interface IEmailHelper
    {
        Task<bool> SendEmail(string module = "", string subject = "", string sendTo = "", string content = "", string recipientEmail = "", List<Attachment> files = null, string remarks = "");
    }

    public class EmailHelper : IEmailHelper
    {
        private readonly IEmailConfigRepository _emailConfigRepository;

        public EmailHelper(IEmailConfigRepository emailConfigRepository)
        {
            _emailConfigRepository = emailConfigRepository;
        }

        public async Task<bool> SendEmail(string module = "", string subject = "", string sendTo = "", string content = "", string recipientEmail = "", List<Attachment> files = null, string remarks = "")
        {
            bool result = false;
            string errorMsg = string.Empty;

            #region Email Notificaiton Save/Upate

            var vEmailNotification_RequestObj = new EmailNotification_Request()
            {
                Module = module,
                Subject = subject,
                SendTo = sendTo,
                Content = content,
                EmailTo = recipientEmail,
                RefValue1 = remarks,
                IsSent = false,
            };

            int resultEmailNotification = await _emailConfigRepository.SaveEmailNotification(vEmailNotification_RequestObj);

            #endregion

            try
            {
                string sSmtpServerName = string.Empty;
                string sSmtpServer = string.Empty;
                string sSmtpUsername = string.Empty;
                string sSmtpPassword = string.Empty;
                bool? bSmtpUseDefaultCredentials = false;
                bool? bSmtpEnableSSL = false;
                int iSmtpPort = 0;
                int? iSmtpTimeout = 0;
                string sFromAddress = string.Empty;
                string sEmailSenderName = string.Empty;
                string sEmailSenderCompanyLogo = string.Empty;
                bool? bIsActive = false;

                #region Email Config

                var vEmailConfig_Search = new EmailConfig_Search() { IsActive = true };
                var vEmailConfigObj = _emailConfigRepository.GetEmailConfigList(vEmailConfig_Search).Result.ToList().FirstOrDefault();
                if (vEmailConfigObj != null)
                {
                    sSmtpServerName = vEmailConfigObj.SmtpServerName;
                    sSmtpServer = vEmailConfigObj.SmtpServer;
                    sSmtpUsername = vEmailConfigObj.SmtpUsername;
                    sSmtpPassword = vEmailConfigObj.SmtpPassword;
                    bSmtpUseDefaultCredentials = vEmailConfigObj.SmtpUseDefaultCredentials;
                    bSmtpEnableSSL = vEmailConfigObj.SmtpEnableSSL;
                    iSmtpPort = Convert.ToInt32(vEmailConfigObj.SmtpPort);
                    iSmtpTimeout = vEmailConfigObj.SmtpTimeout;
                    sFromAddress = vEmailConfigObj.FromAddress;
                    sEmailSenderName = vEmailConfigObj.EmailSenderName;
                    sEmailSenderCompanyLogo = vEmailConfigObj.EmailSenderCompanyLogo;
                    bIsActive = vEmailConfigObj.IsActive;
                }

                #endregion

                if (vEmailConfigObj != null)
                {
                    await Task.Run(() =>
                    {
                        using (MailMessage mail = new MailMessage())
                        {
                            if (!string.IsNullOrWhiteSpace(recipientEmail))
                            {
                                mail.From = new MailAddress(sFromAddress);
                                mail.To.Add(recipientEmail);
                                mail.Subject = subject;
                                mail.Body = content;
                                mail.IsBodyHtml = true;

                                if (files != null)
                                {
                                    for (int f = 0; f < files.Count; f++)
                                    {
                                        mail.Attachments.Add(new Attachment(files[f].ContentStream, files[f].Name));
                                    }
                                }

                                using (SmtpClient smtp = new SmtpClient(sSmtpServer, iSmtpPort))
                                {
                                    smtp.Credentials = new NetworkCredential(sSmtpUsername, sSmtpPassword);
                                    smtp.EnableSsl = Convert.ToBoolean(bSmtpEnableSSL);
                                    smtp.Port = iSmtpPort;

                                    //smtp.SendAsync(mail, "EmailAlert");
                                    try
                                    {
                                        smtp.Send(mail);

                                        result = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                        errorMsg = ex.Message;
                                    }
                                }
                            }
                        }
                    });
                }

                #region Email Notificaiton Save/Upate

                if (resultEmailNotification > 0)
                {
                    var vResultObj = await _emailConfigRepository.GetEmailNotificationById(resultEmailNotification);
                    if (vResultObj != null)
                    {
                        vEmailNotification_RequestObj.Id = resultEmailNotification;
                        vEmailNotification_RequestObj.IsSent = result;
                        vEmailNotification_RequestObj.ErrorMessage = errorMsg;

                        int resultEmailNotificationUpdate = await _emailConfigRepository.SaveEmailNotification(vEmailNotification_RequestObj);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                if (resultEmailNotification > 0)
                {
                    var vResultObj = await _emailConfigRepository.GetEmailNotificationById(resultEmailNotification);
                    if (vResultObj != null)
                    {
                        vEmailNotification_RequestObj.Id = resultEmailNotification;
                        vEmailNotification_RequestObj.ErrorMessage = ex.Message;

                        int resultEmailNotificationUpdate = await _emailConfigRepository.SaveEmailNotification(vEmailNotification_RequestObj);
                    }
                }

                result = false;
            }
            return result;
        }
    }
}
