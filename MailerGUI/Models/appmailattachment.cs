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
    
    public partial class appmailattachment
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
}
