using RssDbContextLib.Db_Context;
using RssModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RSS_Reader.Controllers
{
    public class ReaderController : Controller
    {
        private IRssDocumentsRepository _context;

        // GET: Reader
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void SubscribeNewsletter(string EmailAddress, ICollection<string> SubscriberList)
        {
            _context = new RssDocumentsRepository();
            _context.InsertToMailingList(EmailAddress, SubscriberList);
        }

        [HttpPost]
        public ActionResult CancelNewsletter(string EmailAddress)
        {
            _context = new RssDocumentsRepository();
            _context.DeleteFromMailingList(EmailAddress);

            return RedirectToAction("CancelNewsletter", new { emailAddress = EmailAddress });
        }

        [HttpGet]
        [ActionName("CancelNewsletter")]
        public ActionResult CancelNewsletterGET(string emailAddress)
        {
            SubscriberEmail Subscriber = new SubscriberEmail()
            {
                EmailAddress = emailAddress
            };

           return View(Subscriber);
        }
    }
}