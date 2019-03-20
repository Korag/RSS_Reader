using MongoDB.Bson;
using MongoDB.Driver;
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
        public ActionResult CancelNewsletter(string EmailAddress, string ID)
        {
            ObjectId id;
            try
            {
                id = ObjectId.Parse(ID);
            }
            catch (Exception)
            {
                return RedirectToAction("ConfirmationOfCancellingNewsletter", new { emailAddress = EmailAddress });
            }
      
            _context = new RssDocumentsRepository();

            var filter = Builders<SubscriberEmail>.Filter.Eq(x => x.EmailAddress, EmailAddress) & Builders<SubscriberEmail>.Filter.Eq(x => x.Id, id);
            var subscriber = _context.GetSubscribersList().Find(filter);

            if (subscriber != null)
            {
                _context.DeleteFromMailingList(EmailAddress);
            }
            else
            {
                return RedirectToAction("ConfirmationOfCancellingNewsletter", new { emailAddress = EmailAddress });
            }

            return RedirectToAction("CancelNewsletter", new { emailAddress = EmailAddress });
        }

        [HttpGet]
        public ActionResult ConfirmationOfCancellingNewsletter(string emailAddress)
        {
            ViewBag.emailAddress = emailAddress;
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