using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using MailerAPI.Models;

namespace MailerAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";


            //mailerdaemonEntities2 db = new mailerdaemonEntities2();

            //appmail appmail = new appmail();
            //appmail.ApplicationID = 1;
            //appmail.Content = "<html></html>";
            //appmail.DateCreated = DateTime.Now;
            //appmail.DateLastUpdated = DateTime.Now;
            //appmail.From = "nico.bulusan";
            //appmail.isSent = 0;
            //appmail.Retries = 0;
            //appmail.Subject = "test";

      
            //appmailrecipient recipient = new appmailrecipient();
            //recipient.DateCreated = DateTime.Now;
            //recipient.DateLastUpdated = DateTime.Now;
            //recipient.To = "nico.bulusan@sminvestments.com";         


            //appmail.appmailrecipients.Add(recipient);

            
            //var testmail = new MailerController().Post(appmail);


            return View();
        }
    }
}
