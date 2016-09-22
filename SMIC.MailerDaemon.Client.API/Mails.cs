using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMIC.MailerDaemon.Client.API
{
   public class Mails
    {
       public int id { get; set; }
       
       public string ApplicationGUID { get; set; }

       public string From { get; set; }
       public string Subject { get; set; }
       public string Content { get; set; }
       public int Retries { get; set; }
       public int IsSent { get; set; }
       public string UID { get; set;}
       public List<string> To { get; set; }
       public List<string> Cc { get; set; }
       public List<string> Bcc { get; set; }
       public List<MailAttachment> Attachments { get; set; }




    }


   public class MailAttachment
   {
       public string Filename { get; set; }
       public string Type { get; set; }
       public float Size { get; set; }
       public byte[] Data { get; set; }
   }
}
