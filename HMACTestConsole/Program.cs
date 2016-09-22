using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HMACTestConsole.MailerService;

namespace HMACTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {


            //Mails mail = new Mails();
            //mail.Subject = "TEST";
            //mail.Retries = 1;
            //mail.From = "smic.bizcard@sminvestments.com";
            //mail.Content = "THIS IS A TEST MESSAGE";
            //mail.ApplicationGUID = "B02A6A55E3BD491ABE29E9F3BAC698F8";

            
            //mail.To = new List<string>() { "nbulusanjr@gmail.com","nico.bulusan@sminvestments.com","nicz_bz@yahoo.com" };


            //Mailer.APIKey = "QjAyQTZBNTVFM0JENDkxQUJFMjlFOUYzQkFDNjk4RjgNCg==";
            //Mailer.APPId = "B02A6A55E3BD491ABE29E9F3BAC698F8";
            //Mailer.mails = mail;
            //Mailer.Endpoint = "http://10.57.31.52:9002/";
            //Mailer.RunAsync();

            //Console.ReadLine();

            MailerService.MailerService mailer = new MailerService.MailerService();
            MailerService.Mails mail = new Mails();
            MailerService.MailResponse mailResp = new MailResponse();




            string[] recipients = new string[] { "nico.bulusan@sminvestments.com" };

            
            mail.ApplicationGUID = "B02A6A55E3BD491ABE29E9F3BAC698F8";
            mail.Content = "test";
            mail.From = "nico.bulusan@sminvestments.com";
            mail.Subject = "test";
            mail.To = recipients;


           mailResp =  mailer.SendLater(mail,DateTime.Now.AddSeconds(6));

           Console.WriteLine(mailResp.ErrorMessage);

           Console.Read();
            


        }

    }
}
