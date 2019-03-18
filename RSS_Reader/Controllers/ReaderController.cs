using EmailServicePV.Services;
using RssDbContextLib.Db_Context;
using RssModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace RSS_Reader.Controllers
{
    public class ReaderController : Controller
    {
        private IRssDocumentsRepository _context;
        private string CombGlob = "";

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
        public ActionResult CancelNewsletter(string EmailAddress, string Combination)
        {
            if (Combination != CombGlob)
            {
                return RedirectToAction("ConfirmationOfCancellingNewsletter", new { emailAddress = EmailAddress });
            }
            _context = new RssDocumentsRepository();
            _context.DeleteFromMailingList(EmailAddress);

            return RedirectToAction("CancelNewsletter", new { emailAddress = EmailAddress });
        }

        [HttpGet]
        public ActionResult ConfirmationOfCancellingNewsletter(string emailAddress)
        {
            ViewBag.emailAddress = emailAddress;

            Random rng = new Random();
            string identityChecker = "";

            for (int i = 0; i < 4; i++)
            {
                identityChecker += rng.Next(0, 9);
            }

            ViewBag.combinationString = identityChecker;
            CombGlob = identityChecker;

            return View();
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