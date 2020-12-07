using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ehr.Controllers
{
    public class UnpermissionedController : Controller
    {
        // GET: Unpermissioned
        public ActionResult Index()
        {
            return View();
        }
    }
}