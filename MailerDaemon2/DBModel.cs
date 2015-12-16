using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailerDaemon
{
    class DBModel
    {
    }
    public class Applications
    {
        public int id { get; set; }
        public string Description { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateLastUpdated { get; set; }
    }

    public class AppMails
    {
        public int id { get; set; }
        public int ApplicationID { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int Retries { get; set; }
        public int isSent { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
    }
    public  class AppMailAgents
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        public int port { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public int isActive { get; set; }
    }

    public class AppMailAgentAssignments
    {
        public int id { get; set; }
        public int ApplicationID { get; set; }
        public int AppMailAgentID { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
    }

    public class AppMailAttachments
    {
        public int id { get; set; }
        public int AppMailID { get; set; }
        public string Filename { get; set; }
        public string Type { get; set; }
        public Nullable<double> Size { get; set; }
        public byte[] Data { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
    }

    public  class AppMailBccs
    {
        public int id { get; set; }
        public int AppMailID { get; set; }
        public string To { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
    }

    public  class AppMailRecipients
    {
        public int id { get; set; }
        public int AppMailID { get; set; }
        public string To { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
    }

    public  class AppMailCcs
    {
        public int id { get; set; }
        public int AppMailID { get; set; }
        public string To { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
    }


    public class Mails
    {
        public int id { get; set; }
        public int applicationID { get; set; }
        public string applicationName { get; set; }
        public int mailAgentID { get; set; }
        //public string username { get; set; }
        //public string password { get; set; }
        //public string domain { get; set; }
        //public int port { get; set; }
        public string subject { get; set; }
        public List<string> cc { get; set; }
        public List<string> recipients { get; set; }
        public List<string> bcc { get; set; }
        public List<AppMailAttachments> attachments { get; set; }
        public string content { get; set; }
        public string UUID { get; set; }

    }
}
