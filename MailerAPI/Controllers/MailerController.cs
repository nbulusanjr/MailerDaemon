using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using MailerAPI.Models;
using MailerAPI.Filter;
using SMIC.MailerDaemon.Client.API;
using MailerAPI.Models;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.ObjectModel;



namespace MailerAPI.Controllers
{


    // [RoutePrefix("api/mailer")]
    public class MailerController : ApiController
    {
        // GET api/mailer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/mailer/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/mailer
        //  public void Post([FromBody]string value)
        // {
        // }

        // PUT api/mailer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/mailer/5
        public void Delete(int id)
        {
        }
        // public string Post(MailerAPI.Models.appmail AppMail)

        //[HMACAuthentication]
        public IHttpActionResult Post(Mails mails)
        {


            try
            {

                using (mailerdaemonEntities db = new mailerdaemonEntities())
                {
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
                                       // throw new Exception("Invalid email address: " + z);

                                        Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (Recipient): " + z);

                                        Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);

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
                                       // throw new Exception("Invalid email address: " + z);
                                        Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (CC): " + z);

                                        Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
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
                                       // throw new Exception("Invalid email address: " + z);
                                        Exception emEx = new Exception("EmailID :" + mail.id + Environment.NewLine + "Invalid email address (BCC): " + z);

                                        Elmah.ErrorSignal.FromCurrentContext().Raise(emEx);
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
                            service.Credentials = new NetworkCredential(appClient.MailUsername,appClient.MailPassword, appClient.MailDomain);
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

                               
                                mail.UID = g.ToString();
                                mail.isSent = 1;
                                db.SaveChanges();

                                messageItems.Add(newmessage);
                            }
                            ServiceResponseCollection<ServiceResponse> response = service.CreateItems(messageItems, WellKnownFolderName.SentItems, MessageDisposition.SendAndSaveCopy, null);




                            transaction.Commit();





                        }
                        catch (Exception ex1)
                        {
                            transaction.Rollback();
                            throw new Exception(ex1.Message);
                        }
                        finally
                        {
                            db.Database.Connection.Close();
                        }
                    }
                }




                var result = new
                {
                    Result = "OK"
                };





                return Ok(result);

                //  return Newtonsoft.Json.JsonConvert.SerializeObject(result);


            }
            catch (Exception ex)
            {
                Exception e = ex.GetBaseException();

                var result = new
                {
                    Result = "ERROR",
                    Message = e.Message
                };


                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);

                //return Content(HttpStatusCode.InternalServerError, result);

                return Ok(result);
                // return Newtonsoft.Json.JsonConvert.SerializeObject(result);

            }




        }
    }
}
