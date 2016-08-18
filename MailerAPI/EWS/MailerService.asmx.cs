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
            return null;
        }

       

    }
}
