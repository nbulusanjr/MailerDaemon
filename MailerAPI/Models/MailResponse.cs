using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MailerAPI.Models
{
    public class MailResponse
    {
        public string Result { get; set; }
        public long MessageID { get; set; }
        public string MailGUID { get; set; }
        public string ErrorMessage { get; set; }
        public string JobID { get; set; }
    }
}