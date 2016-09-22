using MailerAPI.Models;
using Microsoft.Exchange.WebServices.Data;
using SMIC.MailerDaemon.Client.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;

namespace MailerAPI.EWS
{
    /// <summary>
    /// Summary description for MailerService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MailerService : System.Web.Services.WebService
    {

        [WebMethod]
        public MailResponse Send(Mails mails)
        {
            ExchangeMailer excMailer = new ExchangeMailer();
            return excMailer.SendMail(mails);

        }

        [WebMethod]
        public MailResponse SendLater(Mails mails, DateTime datetime,bool oneTimeOnly=true,int triggerAt=0)
        {
            ExchangeMailer excMailer = new ExchangeMailer();

            return excMailer.SendLater(mails, datetime,oneTimeOnly,triggerAt);
        }


        [WebMethod]
        public MailResponse UpdateMailJob(int mailID, string jobID, DateTime datetime, Boolean oneTimeOnly = true,int triggerAt=0)
        {
            ExchangeMailer excMailer = new ExchangeMailer();



            return excMailer.UpdateMailJob(mailID, jobID, datetime, oneTimeOnly,triggerAt);
        }

        [WebMethod]
        public MailResponse DeleteMailJob(int mailID, string jobID)
        {
            ExchangeMailer excMailer = new ExchangeMailer();

            return excMailer.DeleteMailJob(mailID, jobID);
        }




    }
}
