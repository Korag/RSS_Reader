﻿using MongoDB.Driver;
using Postal;
using Rss_Downloader_Console.Models;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Rss_Downloader.Db_Context;
using MongoDB.Bson;
using System.Collections.Generic;
using System;
using System.Net.Mail;

namespace Rss_Downloader_Console.Services
{
    class EmailServiceProvider
    {
        private string _viewsPath { get; set; }
        private ViewEngineCollection _engines { get; set; }
        private EmailService _service { get; set; }
        private string _emailFrom { get; set; }

        private IMongoCollection<SubscriberEmail> SubscribersCollection { get; set; }
        private List<SubscriberEmail> Subscribers { get; set; }
        private RssDocumentsRepository _context = new RssDocumentsRepository();

        public EmailServiceProvider()
        {
            _viewsPath = Path.GetFullPath(@"..\..\Views\Emails");
            _engines = new ViewEngineCollection();
            _engines.Add(new FileSystemRazorViewEngine(_viewsPath));
            _service = new EmailService(_engines);
            _emailFrom = "vertisio.com@gmail.com";
        }

        private void SendSubscriberEmail(SubscriberEmail model)
        {
            dynamic email = new Email("Email");

            email.To = model.EmailAddress;
            email.SubscriberList = model.SubscriberList;

            email.From = _emailFrom;

            email.RssList = _context._rssDocumentCollection.Find(new BsonDocument()).ToList();

            email.Subject = "Newsletter RSS Reader RMF24 - " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ".";
            //email.Attach(new Attachment(@"C:\Users\Łukasz\Documents\Visual Studio 2017\Projects\RSS_Reader\Rss_Downloader_Console\Assets\vertisio_logo.PNG"));

            Task t = _service.SendAsync(email);
        }
        public void SendNewsletterToSubscribers()
        {
            SubscribersCollection = _context.GetSubscribersList();
            Subscribers = SubscribersCollection.Find(new BsonDocument( )).ToList();

            Parallel.ForEach(Subscribers, subscriber =>
            {
                SendSubscriberEmail(subscriber);
            });
        }
    }
}
