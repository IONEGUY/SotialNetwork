using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api
{
    public class Constants
    {
        public static TimeSpan AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1);
        public static TimeSpan RefreshTokenExpireTimeSpan = TimeSpan.FromMinutes(2);
    }
}