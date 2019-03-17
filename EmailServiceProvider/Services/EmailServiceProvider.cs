using MongoDB.Bson;
using MongoDB.Driver;
using Postal;
using RssDbContextLib.Db_Context;
using RssModelsLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmailServicePV.Services
{
    public class EmailServiceProvider
    {
        private string _viewsPath { get; set; }
        private ViewEngineCollection _engines { get; set; }
        private EmailService _service { get; set; }
        private string _emailFrom { get; set; }

        private IMongoCollection<SubscriberEmail> SubscribersCollection { get; set; }
        private List<SubscriberEmail> Subscribers { get; set; }
        private RssDocumentsRepository _context = new RssDocumentsRepository();

        public EmailServiceProvider(bool EmailCombination = false)
        {
            #region Local
            //_viewsPath = Path.GetFullPath(@"..\..\Views\Emails");
            #endregion
            #region Azure
            _viewsPath = Path.GetFullPath(@"Emails");
            #endregion
            //#region LocalStable
            //_viewsPath = Path.GetFullPath(@"C:\Users\user\Documents\Visual Studio 2017\Projects\RSS_Reader\EmailServiceProvider\Views\Emails");
            //#endregion
            if (EmailCombination == true)
            {
                _viewsPath = Path.GetFullPath(@"..\Files\Views\Emails");
            }
            _engines = new ViewEngineCollection();
            _engines.Add(new FileSystemRazorViewEngine(_viewsPath));
            _service = new EmailService(_engines);
            _emailFrom = "vertisio.com@gmail.com";
        }

        public void SendSubscriberEmail(SubscriberEmail model)
        {
            dynamic email = new Email("Email");

            email.To = model.EmailAddress;
            email.SubscriberList = model.SubscriberList;

            email.From = _emailFrom;

            email.RssList = _context._rssDocumentCollection.Find(new BsonDocument()).ToList();

            email.Subject = "Newsletter RSS Reader RMF24 - " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.AddHours(1).ToShortTimeString() + ".";

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

        public void SendEmailWithCombinationString(string combinationString, string emailAddress)
        {
            dynamic email = new Email("EmailCombination");
            email.To = emailAddress;
            email.From = _emailFrom;

            email.Subject = "Rezygnacja z newslettera - " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.AddHours(1).ToShortTimeString() + ".";
            email.Combination = combinationString;

            _service.Send(email);
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
