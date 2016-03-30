using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MailerAPI.Models;
using MailerAPI.Filter;




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

        [HMACAuthentication]
        public IHttpActionResult Post(MailerAPI.Models.appmail AppMail)
        {

            try {

                using (mailerdaemonEntities2 db = new mailerdaemonEntities2())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.appmails.Add(AppMail);
                            db.SaveChanges();

                            List<MailerAPI.Models.appmailrecipient> recipients = AppMail.appmailrecipients.ToList();

                            if (recipients.Count == 0)
                            {
                                throw new Exception("Mail Recipient is required!");
                            }

                            foreach (var y in recipients)
                            {
                                ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();

                                if (!cv.IsEmail(y.To))
                                {
                                    throw new Exception("Invalid email address: " + y.To);
                                }

                                y.AppMailID = AppMail.id;
                                
                            }

                            
                       
                            db.appmailrecipients.AddRange(recipients);
                            db.SaveChanges();


                            List<MailerAPI.Models.appmailcc> ccs = AppMail.appmailccs.ToList();

                            if (ccs.Count > 0)
                            {
                                foreach (var y in ccs)
                                {

                                    ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();
                                    if (!cv.IsEmail(y.To))
                                    {
                                        throw new Exception("Invalid email address: " + y.To);
                                    }
                                    y.AppMailID = AppMail.id;
                                }

                                db.appmailccs.AddRange(ccs);
                                db.SaveChanges();
                            }



                            List<MailerAPI.Models.appmailbcc> bccs = AppMail.appmailbccs.ToList();
                            if (bccs.Count > 0)
                            {
                                foreach (var y in bccs)
                                {
                                    
                                    ValidatorTool.CustomValidator cv = new ValidatorTool.CustomValidator();
                                    if (!cv.IsEmail(y.To))
                                    {
                                        throw new Exception("Invalid email address: " + y.To);
                                    }

                                    y.AppMailID = AppMail.id;
                                }

                                db.appmailbccs.AddRange(bccs);
                                db.SaveChanges();
                            }



                            List<MailerAPI.Models.appmailattachment> attachments = AppMail.appmailattachments.ToList();
                            if (attachments.Count > 0)
                            {
                                foreach (var y in attachments)
                                {
                                    y.AppMailID = AppMail.id;
                                }
                                db.appmailattachments.AddRange(attachments);
                                db.SaveChanges();
                            }
                           

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
                    Status="OK"
                };


                return Ok(result);

              //  return Newtonsoft.Json.JsonConvert.SerializeObject(result);


            }
            catch (Exception ex)
            {
                Exception e = ex.GetBaseException();

                var result = new
                {
                    Status = "ERROR",
                    Message = e.Message
                };

                return Ok(result);
               // return Newtonsoft.Json.JsonConvert.SerializeObject(result);

            }

        
           
            
        }
    }
}
