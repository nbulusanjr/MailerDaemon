﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MailerDaemon2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MailerDaemonEntities : DbContext
    {
        public MailerDaemonEntities()
            : base("name=MailerDaemonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<application> applications { get; set; }
        public DbSet<appmailattachment> appmailattachments { get; set; }
        public DbSet<appmailbcc> appmailbccs { get; set; }
        public DbSet<appmailcc> appmailccs { get; set; }
        public DbSet<appmailjob> appmailjobs { get; set; }
        public DbSet<appmailrecipient> appmailrecipients { get; set; }
        public DbSet<appmail> appmails { get; set; }
        public DbSet<role> roles { get; set; }
        public DbSet<user> users { get; set; }
    }
}
