using MailerDaemon;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Exchange.WebServices.Data;
using System.Net.NetworkInformation;
using System.Net;

namespace MailerDaemon2
{
    class Program
    {
        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);


        static void Main(string[] args)
        {   
         
            Console.WriteLine("STARTED.....");

            string url = "http://*:9098";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                
            }




            int appID = 0;
            string applicationGUID = "";
            string applicationKey = "";

            MailerDaemonEntities db = new MailerDaemonEntities();

            application app = new application();
            app = db.applications.Where(x => x.ApplicationGUID == applicationGUID && x.ApplicationKey == applicationKey).FirstOrDefault();


            if (app == null)
            {
               // MyHub hub = new MyHub();

                var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

                context.Clients.All.addMessage("SYSTEM", "UNABLE TO RETRIEVE RECORD USING THE PROVIDED CREDENTIALS");
            }




            Console.ReadLine();


        }


        private string GetOpenPort()
        {
            int PortStartIndex = 8090;
            int PortEndIndex = 9999;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort.ToString();
        }


        void SendMail(int AppID,string ApplicationGUID,string ApplicationKey, bool AllowAttachments = false)
        {

            //int AppID = 0;
            //bool AllowAttachments = false;


            using (MailerDaemonEntities db = new MailerDaemonEntities())
            {
                try
                {


                    /// GET MAIL AGENTS PER APPLICATION

                    //List<AppMailAgentAssignments> aAsign = (from a in db.appmailagentassignments
                    //                                        where (AppID == 0 ? true : a.ApplicationID == AppID)
                    //                                        select new AppMailAgentAssignments
                    //                                        {
                    //                                            id = a.id,
                    //                                            ApplicationID = a.ApplicationID,
                    //                                            AppMailAgentID = a.AppMailAgentID,
                    //                                            DateCreated = a.DateCreated,
                    //                                            DateLastUpdated = a.DateLastUpdated
                    //                                        }).ToList<AppMailAgentAssignments>();
                    /// GET MAILS PER APPLICATION

                    List<Mails> m = (from a in db.appmails
                                     where (a.ApplicationID == AppID)
                                     select new Mails
                                     {
                                         id = a.id,
                                         applicationID = a.ApplicationID,
                                         applicationName = "Sample APP",
                                         mailAgentID = 0,
                                         content = a.Content,
                                         subject = a.Subject,
                                     }).ToList<Mails>();



                    foreach (Mails im in m)
                    {
                        im.recipients = (db.appmailrecipients.Where(x => x.AppMailID == im.id).Select(y => y.To)).ToList<string>();
                        im.bcc = (db.appmailbccs.Where(x => x.AppMailID == im.id).Select(y => y.To)).ToList<string>();
                        im.cc = (db.appmailccs.Where(x => x.AppMailID == im.id).Select(y => y.To)).ToList<string>();
                        if (AllowAttachments)
                        {


                            im.attachments = (from att in db.appmailattachments
                                              where att.AppMailID == im.id
                                              select new AppMailAttachments
                                              {
                                                  id = im.id,
                                                  AppMailID = att.AppMailID,
                                                  Data = att.Data,
                                                  DateCreated = att.DateCreated,
                                                  Filename = att.Filename,
                                                  Type = att.Type
                                              }).ToList<AppMailAttachments>();

                        }
                    }



                    //List<Mails> m1 = m;

                  //  ExchangeMailer em = new ExchangeMailer();

                 //   foreach (AppMailAgentAssignments apmsign in aAsign)
                  //  {

                       // em.sendMailWorker(m.Where(x => x.applicationID == apmsign.ApplicationID).ToList(), apmsign.ApplicationID, apmsign.AppMailAgentID);

                  //  }


                    // em.sendMailWorker(m,1,1);

                }
                catch (Exception ex)
                {
                    Exception e = ex.GetBaseException();
                    Console.WriteLine(e.ToString());
                }
                finally
                {

                    db.Database.Connection.Close();
                }
            }

             //For diagnostic purposes.
             //Console.WriteLine("Processed in {0} seconds", sw.ElapsedMilliseconds / 1000);



        }
    
    
    }




    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
           // Console.WriteLine(name + ':' + message);

            Clients.All.addMessage(name, message);
        }
    }



}
