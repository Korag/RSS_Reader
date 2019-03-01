using Postal;
using Rss_Downloader_Console.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Rss_Downloader_Console.Services
{
    class EmailServiceProvider
    {
        private string _viewsPath { get; set; }
        private ViewEngineCollection _engines { get; set; }
        private EmailService _service { get; set; }
        private string _emailFrom { get; set; }

        private List<SubscriberEmail> Subscribers { get; set; }

        public EmailServiceProvider()
        {
            _viewsPath = Path.GetFullPath(@"..\..\Views\Emails");
            _engines = new ViewEngineCollection();
            _engines.Add(new FileSystemRazorViewEngine(_viewsPath));
            _service = new EmailService(_engines);
            _emailFrom = "vertisio.com@gmail.com";
        }

        public void SendSubscriberEmail()
        {
            dynamic email = new Email("Email");

            email.Message = "Hello, non-asp.net world!";
            email.To = "email@test.net";
            email.Subject = "Test";


            email.From = _emailFrom;

            Task t = _service.SendAsync(email);
        }

   
    }
}
