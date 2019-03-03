using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RSS_Reader.Controllers
{
    public class ReaderController : Controller
    {
        // GET: Reader
        public ActionResult Index()
        {
            return View();
        }
    }
}