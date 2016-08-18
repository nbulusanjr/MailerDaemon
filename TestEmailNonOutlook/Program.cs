using SMIC.MailerDaemon.Client.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEmailNonOutlook
{
    public class Program
    {
         static void Main(string[] args)
        {
           Test();
        }

        static async System.Threading.Tasks.Task Test()
        {
            Mails mails = new Mails();
            mails.ApplicationGUID = "4d53bce03ec34c0a91182d4c228ee6c";

            mails.From = "smic.onlinepr@sminvestments.com";
            mails.Subject = "TEst ";

            mails.To = new List<string>() { "nbulusanjr@gmail.com" };
            mails.Content = "TEst";

            Mailer.APIKey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";
            Mailer.APPId = "4d53bce03ec34c0a911182d4c228ee6c";
            Mailer.mails = mails;
            Mailer.Endpoint = "http://10.57.31.52:9002/";

            var emailResponse = await Mailer.RunAsync();

            if (emailResponse.Contains("ERROR"))
            {
                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(emailResponse);

                throw new Exception(json["Message"].ToString());
            }
        }
    }
}
