//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MailerGUI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class appmail
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
        public string UID { get; set; }
    }
}
