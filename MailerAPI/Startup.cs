using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Hangfire.MySql;

namespace MailerAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseStorage(new MySqlStorage("mailerdaemonJob"));
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        
        }
    }
}