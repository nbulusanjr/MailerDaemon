using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailerGUI.Models;

namespace MailerGUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult MailList()
        {
            MailerDaemonEntities1 db = new MailerDaemonEntities1();
            return View(db.appmails.Take(100).ToList());
        }

        public ActionResult MailDetails(int id)
        {

            MailerDaemonEntities1 db=new MailerDaemonEntities1();

            var mail = db.appmails.Where(x => x.id == id).FirstOrDefault();
         


            ViewBag.mail = mail;
          
           

            return View();
        }
        
    

    }
}