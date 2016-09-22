using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMIC.MailerDaemon.Client.API;
using System.Net;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.ObjectModel;
using Hangfire;

namespace MailerAPI.Models
{

    public enum JobStatus
    {
        NEW = 1,
        UPDATED = 2,
        DELETED = 3
    }

    public class ExchangeMailer
    {
        /// <summary>
        /// SEND SCHEDULED MAIL
        /// </summary>
        /// <param name="mailID"></param>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public MailResponse SendScheduledMail(int mailID, string jobID, Boolean oneTimeOnly = true)
        {
            mailerdaemonEntities db = new mailerdaemonEntities();



            using (var transaction = db.Database.BeginTransaction())
            {

                try
                {

                    var mail = db.appmails.Where(x => x.id == mailID).FirstOrDefault();
                    var appClient = db.applications.Where(x => x.id == mail.ApplicationID).FirstOrDefault();


                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                    service.Credentials = new NetworkCredential(appClient.MailUsername, appClient.MailPassword, appClient.MailDomain);
                    service.Url = new Uri(appClient.MailServiceUrl);

                    Collection<EmailMessage> messageItems = new Collection<EmailMessage>();

                    EmailMessage newmessage = new EmailMessage(service);
                    newmessage.Body = mail.Content;
                    newmessage.Subject = mail.Subject;
                    newmessage.Body.BodyType = BodyType.HTML;
                    newmessage.Importance = Importance.High;
                    /// ADD MULTIPLE RECIPIENTS
                    /// 
                    if (mail.appmailrecipients != null)
                    {
                        foreach (var recipient in mail.appmailrecipients)
                        {
                            newmessage.ToRecipients.Add(recipient.To);
                        }
                    }

                    /// ADD CC RECIPIENTS
                    /// 
                    if (mail.appmailccs != null)
                    {
                        foreach (var cc in mail.appmailccs)
                        {
                            newmessage.CcRecipients.Add(cc.To);
                        }
                    }


                    /// ADD BCC RECIPIENTS
                    /// 
                    if (mail.appmailbccs != null)
                    {
                        foreach (var bcc in mail.appmailbccs)
                        {
                            newmessage.BccRecipients.Add(bcc.To);
                        }
                    }

                    /// ADD MULTIPLE ATTACHMENTS
                    /// 
                    if (mail.appmailattachments != null)
                    {
                        foreach (var amt in mail.appmailattachments)
                        {
                            newmessage.Attachments.AddFileAttachment(amt.Filename, amt.Data);
                        }
                    }



                    // Create a custom extended property and add it to the message. 
                    Guid myPropertySetId = new Guid("{20B5C09F-7CAD-44c6-BDBF-8FCBEEA08544}");
                    Guid g;
                    g = Guid.NewGuid();

                    ExtendedPropertyDefinition myExtendedPropertyDefinition = new ExtendedPropertyDefinition(myPropertySetId, "UUID", MapiPropertyType.String);
                    newmessage.SetExtendedProperty(myExtendedPropertyDefinition, g.ToString());
                    newmessage.IsDeliveryReceiptRequested = true;
                    newmessage.IsReadReceiptRequested = true;

                    messageItems.Add(newmessage);
                    ServiceResponseCollection<ServiceResponse> response = service.CreateItems(messageItems, WellKnownFolderName.SentItems, MessageDisposition.SendAndSaveCopy, null);


                    mail.UID = g.ToString();
                    mail.isSent = 1;
                    mail.Retries += 1;
                    db.SaveChanges();


                    appmailjob mailjob = db.appmailjobs.Where(x => x.JobID == jobID).FirstOrDefault();

                    if (mailjob != null)
                    {   
                        mailjob.DateLastUpdated = DateTime.Now;                    
                        mailjob.Status = (int)JobStatus.UPDATED;
                        db.SaveChanges();
                    }


                 

                    if (oneTimeOnly)
                    {
                        RecurringJob.RemoveIfExists(jobID);
                    }

                    MailResponse mailResp = new MailResponse();
                    mailResp.Result = "OK";
                    mailResp.MessageID = mail.id;
                    mailResp.MailGUID = mail.UID;
                    mailResp.JobID = jobID;

                    transaction.Commit();

                    return mailResp;

                }

                catch (Exception ex)
                {

                    transaction.Rollback();
                    MailResponse mailResp = new MailResponse();
                    mailResp.Result = "ERROR";
                    mailResp.MessageID = -1;
                    mailResp.ErrorMessage = ex.GetBaseException().Message;
                    return mailResp;
                }
                finally
                {
                    db.Database.Connection.Close();
                }



            }








        }

        /// <summary>
        /// UPDATE SCHEDULED MAIL
        /// </summary>
        /// <param name="mailID"></param>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public MailResponse UpdateMailJob(int mailID, string jobID, DateTime newDatetime, Boolean oneTimeOnly = true, int triggerAt = 0)
        {

            mailerdaemonEntities db = new mailerdaemonEntities();

            try
            {

                var mail = db.appmails.Where(x => x.id == mailID).FirstOrDefault();

                if (mail == null)
                {
                    throw new Exception(String.Format("Mail # {0} does not exist!", mailID));
                }

                var appClient = db.applications.Where(x => x.id == mail.ApplicationID).FirstOrDefault();

                appmailjob mailjob = new appmailjob();
                mailjob = db.appmailjobs.Where(x => x.JobID == jobID).FirstOrDefault();
                if (mailjob != null)
                {
                    mailjob.Status = (int)JobStatus.DELETED;
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception(String.Format("Job ID {0} does not exist!", jobID));
                }

                int dayOfMonth = newDatetime.Day;
                int monthField = newDatetime.Month;

                var recurringJobs = db.appmailjobs.Where(x => x.MailID == mailID).ToList();

                foreach (var recurringJob in recurringJobs)
                {

                    recurringJob.Status = (int)JobStatus.DELETED;
                    db.SaveChanges();
                    RecurringJob.RemoveIfExists(recurringJob.JobID);
                }

                RecurringJob.AddOrUpdate(jobID, () => SendScheduledMail(mail.id, appClient.Description + "-" + jobID, oneTimeOnly), String.Format("0 8 {0} {1} *", dayOfMonth, monthField), TimeZoneInfo.Local);
                for (int y = 1; y < triggerAt; y++)
                {
                    appmailjob mailjobRecurring = new appmailjob();
                    mailjobRecurring.ApplicationID = appClient.id.ToString();
                    mailjobRecurring.DateCreated = DateTime.Now;
                    mailjobRecurring.DateLastUpdated = DateTime.Now;
                    mailjobRecurring.Duration = 0;
                    mailjobRecurring.JobID = jobID + "-" + y.ToString();
                    mailjobRecurring.MailID = mail.id;
                    mailjobRecurring.Status = (int)JobStatus.NEW;
                    db.appmailjobs.Add(mailjobRecurring);
                    db.SaveChanges();


                    RecurringJob.AddOrUpdate(jobID + "-" + y.ToString(), () => SendScheduledMail(mailID, appClient.Description + "-" + jobID, oneTimeOnly), String.Format("0 8 {0} {1} *", dayOfMonth - y, monthField), TimeZoneInfo.Local);


                }



                MailResponse mailResp = new MailResponse();
                mailResp.Result = "OK";
                mailResp.MessageID = mail.id;
                mailResp.MailGUID = mail.UID;
                mailResp.JobID = jobID;

                return mailResp;
            }
            catch (Exception ex)
            {
                MailResponse mailResp = new MailResponse();
                mailResp.Result = "ERROR";
                mailResp.MessageID = -1;
                mailResp.ErrorMessage = ex.GetBaseException().Message;
                return mailResp;
            }


            return null;
        }

        /// <summary>
        /// DELETE SCHEDULED MAIL
        /// </summary>
        /// <param name="mailID"></param>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public MailResponse DeleteMailJob(int mailID, string jobID)
        {
            mailerdaemonEntities db = new mailerdaemonEntities();

            try
            {

                var mail = db.appmails.Where(x => x.id == mailID).FirstOrDefault();

                if (mail == null)
                {
                    throw new Exception(String.Format("Mail # {0} does not exist!", mailID));
                }


                var appClient = db.applications.Where(x => x.id == mail.ApplicationID).FirstOrDefault();

                RecurringJob.RemoveIfExists(jobID);

                appmailjob mailjob = db.appmailjobs.Where(x => x.JobID == jobID).FirstOrDefault();
                if (mailjob != null)
                {

                    mailjob.Status = (int)JobStatus.DELETED;
                    db.SaveChanges();

                    var recurringJobs = db.appmailjobs.Where(x => x.MailID == mailID).ToList();

                    foreach (var recurringJob in recurringJobs)
                    {
                        RecurringJob.RemoveIfExists(recurringJob.JobID);
                    }


                }
                else
                {
                    throw new Exception(String.Format("Job ID {0} does not exist!", jobID));
                }

                MailResponse mailResp = new MailResponse();
                mailResp.Result = "OK";
                mailResp.MessageID = mail.id;
                mailResp.MailGUID = mail.UID;
                mailResp.JobID = jobID;

                return mailResp;


            }
            catch (Exception ex)
            {
                MailResponse mailResp = new MailResponse();
                mailResp.Result = "ERROR";
                mailResp.MessageID = -1;
                mailResp.ErrorMessage = ex.GetBaseException().Message;
                return mailResp;
            }



        }

        public MailResponse SendMail(Mails mails)
        {
            mailerdaemonEntities db = new mailerdaemonEntities();

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var appClient = db.applications.Where(x => x.ApplicationGUID == mails.ApplicationGUID).FirstOrDefault();

                    appmail mail = new appmail();
                    mail.ApplicationID = appClient.id;
                    mail.Content = mails.Content;
                    mail.DateCreated = DateTime.Now;
                    mail.DateLastUpdated = DateTime.Now;
                    mail.From = mails.From;
                    mail.Subject = mails.Subject;

                    db.appmails.Add(mail);
                    db.SaveChanges();

                    if (mails.To != null)
                    {
                        foreach (var z in mails.To)
                        {
                            ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();
                            if (!cv.IsEmail(z))
                            {


                                Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (Recipient): " + z);

                                Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
                                throw new Exception("Invalid email address: " + z);

                            }

                            appmailrecipient recipient = new appmailrecipient();
                            recipient.AppMailID = mail.id;
                            recipient.DateCreated = DateTime.Now;
                            recipient.DateLastUpdated = DateTime.Now;
                            recipient.To = z;

                            db.appmailrecipients.Add(recipient);
                            db.SaveChanges();
                        }


                    }

                    if (mails.Cc != null)
                    {

                        foreach (var z in mails.Cc)
                        {

                            ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();

                            if (!cv.IsEmail(z))
                            {

                                Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (CC): " + z);

                                Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
                                throw new Exception("Invalid email address: " + z);
                            }

                            appmailcc cc = new appmailcc();
                            cc.AppMailID = mail.id;
                            cc.DateCreated = DateTime.Now;
                            cc.DateLastUpdated = DateTime.Now;
                            cc.To = z;

                            db.appmailccs.Add(cc);
                            db.SaveChanges();


                        }

                    }

                    if (mails.Bcc != null)
                    {
                        foreach (var z in mails.Bcc)
                        {

                            ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();

                            if (!cv.IsEmail(z))
                            {

                                Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (BCC): " + z);

                                Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
                                throw new Exception("Invalid email address: " + z);
                            }

                            appmailbcc bcc = new appmailbcc();
                            bcc.AppMailID = mail.id;
                            bcc.DateCreated = DateTime.Now;
                            bcc.DateLastUpdated = DateTime.Now;
                            bcc.To = z;

                            db.appmailbccs.Add(bcc);
                            db.SaveChanges();


                        }
                    }





                    List<Mails> m = new List<Mails>();
                    m.Add(mails);


                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                    service.Credentials = new NetworkCredential(appClient.MailUsername, appClient.MailPassword, appClient.MailDomain);
                    // service.Credentials = new WebCredentials(appagent.username, appagent.password);
                    service.Url = new Uri(appClient.MailServiceUrl);




                    Collection<EmailMessage> messageItems = new Collection<EmailMessage>();

                    foreach (Mails mm in m)
                    {
                        EmailMessage newmessage = new EmailMessage(service);
                        newmessage.Body = mm.Content;
                        newmessage.Subject = mm.Subject;
                        newmessage.Body.BodyType = BodyType.HTML;
                        newmessage.Importance = Importance.High;
                        /// ADD MULTIPLE RECIPIENTS
                        /// 
                        if (mm.To != null)
                        {
                            foreach (string r in mm.To)
                            {
                                newmessage.ToRecipients.Add(r);
                            }
                        }

                        /// ADD CC RECIPIENTS
                        /// 
                        if (mm.Cc != null)
                        {
                            foreach (string c in mm.Cc)
                            {
                                newmessage.CcRecipients.Add(c);
                            }
                        }


                        /// ADD BCC RECIPIENTS
                        /// 
                        if (mm.Bcc != null)
                        {
                            foreach (string b in mm.Bcc)
                            {
                                newmessage.BccRecipients.Add(b);
                            }
                        }

                        /// ADD MULTIPLE ATTACHMENTS
                        /// 
                        if (mm.Attachments != null)
                        {
                            foreach (MailAttachment amt in mm.Attachments)
                            {
                                newmessage.Attachments.AddFileAttachment(amt.Filename, amt.Data);
                            }
                        }



                        // Create a custom extended property and add it to the message. 
                        Guid myPropertySetId = new Guid("{20B5C09F-7CAD-44c6-BDBF-8FCBEEA08544}");
                        Guid g;
                        g = Guid.NewGuid();

                        ExtendedPropertyDefinition myExtendedPropertyDefinition = new ExtendedPropertyDefinition(myPropertySetId, "UUID", MapiPropertyType.String);
                        newmessage.SetExtendedProperty(myExtendedPropertyDefinition, g.ToString());
                        newmessage.IsDeliveryReceiptRequested = true;
                        newmessage.IsReadReceiptRequested = true;


                        mail.UID = g.ToString();
                        mail.isSent = 1;
                        db.SaveChanges();

                        messageItems.Add(newmessage);
                    }
                    ServiceResponseCollection<ServiceResponse> response = service.CreateItems(messageItems, WellKnownFolderName.SentItems, MessageDisposition.SendAndSaveCopy, null);




                    transaction.Commit();




                    MailResponse mailResp = new MailResponse();
                    mailResp.Result = "OK";
                    mailResp.MessageID = mail.id;
                    mailResp.MailGUID = mail.UID;
                    mailResp.ErrorMessage = "";

                    return mailResp;


                }
                catch (Exception ex1)
                {
                    transaction.Rollback();

                    MailResponse mailResp = new MailResponse();
                    mailResp.Result = "ERROR";
                    mailResp.MessageID = -1;
                    mailResp.ErrorMessage = ex1.GetBaseException().Message;
                    return mailResp;
                    // throw new Exception(ex1.Message);
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }

        public MailResponse SendLater(Mails mails, DateTime datetime, Boolean oneTimeOnly = true, int triggerAt = 0)
        {


            mailerdaemonEntities db = new mailerdaemonEntities();
            MailResponse mailResp = new MailResponse();
            int mailID = -1;
            int dayOfMonth = 0;
            int monthField = 0;

            Guid g = Guid.NewGuid();
            string jobID = g.ToString();

            var appClient = db.applications.Where(x => x.ApplicationGUID == mails.ApplicationGUID).FirstOrDefault();



            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    if (appClient == null)
                    {
                        throw new Exception("Application client does not exist!");
                    }

                    appmail mail = new appmail();
                    mail.ApplicationID = appClient.id;
                    mail.Content = mails.Content;
                    mail.DateCreated = DateTime.Now;
                    mail.DateLastUpdated = DateTime.Now;
                    mail.From = mails.From;
                    mail.Subject = mails.Subject;

                    db.appmails.Add(mail);
                    db.SaveChanges();

                    if (mails.To != null)
                    {
                        foreach (var z in mails.To)
                        {
                            ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();

                            if (!cv.IsEmail(z))
                            {


                                Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (Recipient): " + z);

                                Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
                                throw new Exception("Invalid email address: " + z);

                            }

                            appmailrecipient recipient = new appmailrecipient();
                            recipient.AppMailID = mail.id;
                            recipient.DateCreated = DateTime.Now;
                            recipient.DateLastUpdated = DateTime.Now;
                            recipient.To = z;

                            db.appmailrecipients.Add(recipient);
                            db.SaveChanges();
                        }


                    }

                    if (mails.Cc != null)
                    {
                        foreach (var z in mails.Cc)
                        {
                            ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();
                            if (!cv.IsEmail(z))
                            {

                                Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (CC): " + z);

                                Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
                                throw new Exception("Invalid email address: " + z);
                            }

                            appmailcc cc = new appmailcc();
                            cc.AppMailID = mail.id;
                            cc.DateCreated = DateTime.Now;
                            cc.DateLastUpdated = DateTime.Now;
                            cc.To = z;

                            db.appmailccs.Add(cc);
                            db.SaveChanges();

                        }
                    }

                    if (mails.Bcc != null)
                    {
                        foreach (var z in mails.Bcc)
                        {
                            ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();

                            if (!cv.IsEmail(z))
                            {

                                Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (BCC): " + z);

                                Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
                                throw new Exception("Invalid email address: " + z);
                            }

                            appmailbcc bcc = new appmailbcc();
                            bcc.AppMailID = mail.id;
                            bcc.DateCreated = DateTime.Now;
                            bcc.DateLastUpdated = DateTime.Now;
                            bcc.To = z;

                            db.appmailbccs.Add(bcc);
                            db.SaveChanges();


                        }
                    }


                    string jobUniqueID = appClient.Description + "-" + jobID;


                    mailResp.Result = "OK";
                    mailResp.MessageID = mail.id;
                    mailResp.MailGUID = "";
                    mailResp.ErrorMessage = "";
                    mailResp.JobID = jobUniqueID;

                    mailID = mail.id;



                    appmailjob mailjob = new appmailjob();
                    mailjob.ApplicationID = appClient.id.ToString();
                    mailjob.DateCreated = DateTime.Now;
                    mailjob.DateLastUpdated = DateTime.Now;
                    mailjob.Duration = 0;
                    mailjob.JobID = jobUniqueID;
                    mailjob.MailID = mail.id;
                    mailjob.Status = (int)JobStatus.NEW;
                    db.appmailjobs.Add(mailjob);
                    db.SaveChanges();

                    for (int y = 1; y <= triggerAt; y++)
                    {

                        appmailjob mailjobRecurring = new appmailjob();
                        mailjobRecurring.ApplicationID = appClient.id.ToString();
                        mailjobRecurring.DateCreated = DateTime.Now;
                        mailjobRecurring.DateLastUpdated = DateTime.Now;
                        mailjobRecurring.Duration = 0;
                        mailjobRecurring.JobID = jobUniqueID + "-" + y.ToString();
                        mailjobRecurring.MailID = mail.id;
                        mailjobRecurring.Status = (int)JobStatus.NEW;
                        db.appmailjobs.Add(mailjobRecurring);
                        db.SaveChanges();


                    }

                    transaction.Commit();


                    dayOfMonth = datetime.Day;
                    monthField = datetime.Month;

                    RecurringJob.AddOrUpdate(jobUniqueID, () => SendScheduledMail(mailID, appClient.Description + "-" + jobID, oneTimeOnly), String.Format("0 8 {0} {1} *", dayOfMonth, monthField), TimeZoneInfo.Local);
                    for (int y = 1; y <= triggerAt; y++)
                    {

                        RecurringJob.AddOrUpdate(jobUniqueID + "-" + y.ToString(), () => SendScheduledMail(mailID, appClient.Description + "-" + jobID, oneTimeOnly), String.Format("0 8 {0} {1} *", dayOfMonth - y, monthField), TimeZoneInfo.Local);
                        
                    }







                }
                catch (Exception ex1)
                {
                    transaction.Rollback();


                    mailResp.Result = "ERROR";
                    mailResp.MessageID = -1;
                    mailResp.ErrorMessage = ex1.GetBaseException().Message;

                    // throw new Exception(ex1.Message);
                }
                finally
                {
                    db.Database.Connection.Close();
                }


            }



            return mailResp;

        }

       

    }




}