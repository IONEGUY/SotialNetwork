using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using SotialNetwork.Api.App_Start;

[assembly: OwinStartup(typeof(SotialNetwork.Api.Startup))]

namespace SotialNetwork.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            httpConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            ConfigureAuth(app);
            app.ConfigureContainer(httpConfig);
        }
    }
}
