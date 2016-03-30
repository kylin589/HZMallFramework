using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HZMall.Core;
using HZMall.Service.Common;
using Newtonsoft.Json;


namespace HZMall.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }


        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            var cultureName = cultureCookie != null ? cultureCookie.Value : "zh-HK";
            cultureName = CultureHelper.GetImplementedCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            return base.BeginExecuteCore(callback, state);
        }
    }
}