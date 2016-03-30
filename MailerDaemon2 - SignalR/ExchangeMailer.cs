using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MailerDaemon;



namespace MailerDaemon2
{
    class ExchangeMailer
    {
        public MailStatus sendMailWorker(List<Mails> m, int appID, int agentID)
        {

            //int fileCount = 0;


            // Determine whether to parallelize file processing on each folder based on processor count. 
            // int procCount = System.Environment.ProcessorCount;

            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

            using (MailerDaemonEntities db = new MailerDaemonEntities())
            {

                //try {

                AppMailAgents appagent = (from a in db.appmailagents
                                          join b in db.appmailagentassignments on a.id equals b.AppMailAgentID
                                          where a.isActive == 1 && b.ApplicationID == appID && b.AppMailAgentID == agentID
                                          select new AppMailAgents
                                              {
                                                  id = a.id,
                                                  username = a.username,
                                                  password = a.password,
                                                  domain = a.domain,
                                                  DateCreated = a.DateCreated
                                              }).ToList<AppMailAgents>().FirstOrDefault();

                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                service.Credentials = new NetworkCredential(appagent.username, appagent.password, appagent.domain);
                // service.Credentials = new WebCredentials(appagent.username, appagent.password);
                service.Url = new Uri("https://smic1eexphc1.smic.sm.ph/EWS/Exchange.asmx");

                //service.UseDefaultCredentials = true;
                //service.AutodiscoverUrl("nico.bulusan@sminvestments.com");
                //Parallel.ForEach(m, mm => {
                //    EmailMessage message = new EmailMessage(service);
                //    message.Body=mm.content;
                //    message.Subject = mm.subject;
                //    message.Body.BodyType = BodyType.HTML;

                //    /// ADD MULTIPLE RECIPIENTS
                //    foreach (string r in mm.recipients)
                //    {
                //        message.ToRecipients.Add(r);
                //    }
                //    /// ADD BCC RECIPIENTS
                //    foreach (string b in mm.bcc)
                //    {
                //        message.BccRecipients.Add(b);
                //    }
                //    /// ADD MULTIPLE ATTACHMENTS
                //    foreach (AppMailAttachments amt in mm.attachments)
                //    {
                //         message.Attachments.AddFileAttachment(amt.Filename, amt.Data);                
                //    }
                //    message.Update(ConflictResolutionMode.AutoResolve);
                //    message.Send();

                //});


                ////FEATURE NOT IMPLEMENTED DUE TO
                ////NO SUPPORT TO FILE ATTACHMENT
                //Console.WriteLine(service.Url);

                //List<EmailMessage> messages = new List<EmailMessage>();
                Collection<EmailMessage> messageItems = new Collection<EmailMessage>();

                foreach (Mails mm in m)
                {
                    EmailMessage newmessage = new EmailMessage(service);
                    newmessage.Body = mm.content;
                    newmessage.Subject = mm.subject;
                    newmessage.Body.BodyType = BodyType.HTML;
                    newmessage.Importance = Importance.High;
                    /// ADD MULTIPLE RECIPIENTS
                    /// 
                    if (mm.recipients != null)
                    {
                        foreach (string r in mm.recipients)
                        {
                            newmessage.ToRecipients.Add(r);
                        }
                    }

                    /// ADD CC RECIPIENTS
                    /// 
                    if (mm.cc != null)
                    {
                        foreach (string c in mm.cc)
                        {
                            newmessage.CcRecipients.Add(c);
                        }
                    }


                    /// ADD BCC RECIPIENTS
                    /// 
                    if (mm.bcc != null)
                    {
                        foreach (string b in mm.bcc)
                        {
                            newmessage.BccRecipients.Add(b);
                        }
                    }

                    /// ADD MULTIPLE ATTACHMENTS
                    /// 
                    if (mm.attachments != null)
                    {
                        foreach (AppMailAttachments amt in mm.attachments)
                        {
                            // newmessage.Attachments.AddFileAttachment(amt.Filename, amt.Data);                
                        }
                    }



                    // Create a custom extended property and add it to the message. 
                    Guid myPropertySetId = new Guid("{20B5C09F-7CAD-44c6-BDBF-8FCBEEA08544}");
                    Guid g;
                    g = Guid.NewGuid();

                    ExtendedPropertyDefinition myExtendedPropertyDefinition = new ExtendedPropertyDefinition(myPropertySetId, "UUID", MapiPropertyType.String);
                    newmessage.SetExtendedProperty(myExtendedPropertyDefinition, g.ToString());
                    newmessage.IsDeliveryReceiptRequested = true;

                    appmail appm = db.appmails.Where(x => x.id == mm.id).FirstOrDefault();
                    appm.UID = g.ToString();
                    db.SaveChanges();

                    messageItems.Add(newmessage);
                }



                //// Create the batch of email messages on the server.
                //// This method call results in an CreateItem call to EWS.
                //ServiceResponseCollection<ServiceResponse> response = service.CreateItems(messageItems, WellKnownFolderName.Drafts, MessageDisposition.SaveOnly, null);




                // Create and send the batch of email messages on the server.
                // This method call results in an CreateItem call to EWS.
                ServiceResponseCollection<ServiceResponse> response = service.CreateItems(messageItems, WellKnownFolderName.SentItems, MessageDisposition.SendAndSaveCopy, null);




                // Instantiate a collection of item IDs to populate from the values that are returned by the Exchange server.
                Collection<ItemId> itemIds = new Collection<ItemId>();

                // Collect the item IDs from the created email messages.
                foreach (EmailMessage message in messageItems)
                {
                    try
                    {

                        itemIds.Add(message.Id);
                        string UUID = message.ExtendedProperties[0].Value.ToString();


                        Console.WriteLine("Email message '{0}' created successfully. UID {1}.", message.Subject, UUID);
                        appmail appm = db.appmails.Where(x => x.UID == UUID).FirstOrDefault();
                        appm.isSent = 1;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {



                        //Console.WriteLine(ex.Message);
                        // Print out the exception and the last eight characters of the item ID.
                        Console.WriteLine("Exception while creating message {0}: {1}", message.Id.ToString().Substring(144), ex.Message);
                    }
                }

                // Check for success of the CreateItems method call.
                if (response.OverallResult == ServiceResult.Success)
                {
                    Console.WriteLine("All locally created messages were successfully saved to the Drafts folder.");
                    Console.WriteLine("\r\n");
                }

                // If the method did not return success, print the result message for each email.
                else
                {
                    int counter = 1;

                    foreach (ServiceResponse resp in response)
                    {
                        // Print out the result and the last eight characters of the item ID.
                        Console.WriteLine("Result (message {0}), id {1}: {2}", counter, itemIds[counter - 1].ToString().Substring(144), resp.Result);
                        Console.WriteLine("Error Code: {0}", resp.ErrorCode);
                        Console.WriteLine("ErrorMessage: {0}\r\n", resp.ErrorMessage);
                        Console.WriteLine("\r\n");

                        counter++;
                    }
                }

                //}
                //catch (Exception ex) {

                //    Exception e = ex.GetBaseException();
                //    //throw new Exception(e.ToString());

                //}
                //finally {
                //    db.Database.Connection.Close();
                //}

            }






            return null;
        }

        public void sendMailWithoutAttachment(List<Mails> m) { }

        public void sendMailWithAttachment(List<Mails> m) { }

        private static bool CertificateValidationCallBack(object sender,System.Security.Cryptography.X509Certificates.X509Certificate certificate,System.Security.Cryptography.X509Certificates.X509Chain chain,System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }
        
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }


    }

    public class MailStatus
    {

        public string status { get; set; }
        public string message { get; set; }

    }



}
