﻿using System.Web;
using System.Web.Mvc;

namespace AutofacOwinAuth.HomeWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
