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
            return View(db.appmails.ToList());
        }

        public ActionResult MailDetails(int id)
        {

            MailerDaemonEntities1 db=new MailerDaemonEntities1();

            var mail = db.appmails.Where(x => x.id == id).FirstOrDefault();
            var mailcc = db.appmailccs.Where(x => x.AppMailID == id).ToList();
            var mailbcc = db.appmailbccs.Where(x => x.AppMailID == id).ToList();
            var mailattacchment = db.appmailattachments.Where(x => x.AppMailID == id).ToList();


            ViewBag.mail = mail;
            ViewBag.mailcc = mailcc;
            ViewBag.mailbcc = mailbcc;
            ViewBag.mailattachment = mailattacchment;

           

            return View();
        }
        
    

    }
}