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
    
    public partial class jobqueue
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Queue { get; set; }
        public Nullable<System.DateTime> FetchedAt { get; set; }
        public string FetchToken { get; set; }
    }
}
