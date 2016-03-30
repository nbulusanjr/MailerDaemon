using MailerDaemon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailerDaemon2
{
    class Program
    {
        static void Main(string[] args)
        {


            int AppID = 0;
            bool AllowAttachments = false;

            var sw = Stopwatch.StartNew();

            Console.WriteLine("STARTED.....");





            using (MailerDaemonEntities db = new MailerDaemonEntities())
            {
                try
                {


                    /// GET MAIL AGENTS PER APPLICATION

                    List<AppMailAgentAssignments> aAsign = (from a in db.appmailagentassignments
                                                            where (AppID == 0 ? true : a.ApplicationID == AppID)
                                                            select new AppMailAgentAssignments
                                                            {
                                                                id = a.id,
                                                                ApplicationID = a.ApplicationID,
                                                                AppMailAgentID = a.AppMailAgentID,
                                                                DateCreated = a.DateCreated,
                                                                DateLastUpdated = a.DateLastUpdated
                                                            }).ToList<AppMailAgentAssignments>();
                    /// GET MAILS PER APPLICATION

                    List<Mails> m = (from a in db.appmails
                                     where (AppID == 0 ? true : a.ApplicationID == AppID)
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

                    ExchangeMailer em = new ExchangeMailer();

                    foreach (AppMailAgentAssignments apmsign in aAsign)
                    {

                        em.sendMailWorker(m.Where(x => x.applicationID == apmsign.ApplicationID).ToList(), apmsign.ApplicationID, apmsign.AppMailAgentID);

                    }


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

            // For diagnostic purposes.
            Console.WriteLine("Processed in {0} seconds", sw.ElapsedMilliseconds / 1000);

            Console.ReadLine();


        }
    }
}
