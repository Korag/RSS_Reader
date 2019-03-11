﻿using MongoDB.Bson;
using MongoDB.Driver;
using Postal;
using RssDbContextLib.Db_Context;
using RssModelsLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rss_Downloader_Console.Services
{
    internal class EmailServiceProvider
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

            Task t = _service.SendAsync(email);

            int attemptsCounter = 1;

            try
            {
                t.Wait();
            }
            catch (Exception)
            {
                while (t.Status == TaskStatus.Faulted)
                {
                    if (attemptsCounter < 6)
                    {
                        attemptsCounter++;
                        Task.Delay(900000);
                        try
                        {
                            t = _service.SendAsync(email);
                            break;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
        public void SendNewsletterToSubscribers()
        {
            SubscribersCollection = _context.GetSubscribersList();
            Subscribers = SubscribersCollection.Find(new BsonDocument()).ToList();

            Parallel.ForEach(Subscribers, subscriber =>
            {
                SendSubscriberEmail(subscriber);
            });
        }
    }
}
