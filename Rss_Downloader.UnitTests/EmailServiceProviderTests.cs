using EmailServicePV.Services;
using NUnit.Framework;
using RssModelsLib.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Rss_Downloader.UnitTests
{
    [TestFixture]
    public class EmailServiceProviderTests
    {
        private EmailServiceProvider _emailSP;
        private EmailServiceProvider _emailSPcomb;
        [SetUp]
        public void SetUp()
        {
            _emailSP = new EmailServiceProvider();
            _emailSPcomb = new EmailServiceProvider(true);
        }

        [Test]
        public void SendEmailWithCombinationString_ExceptionWhenEmailAddressIsNull()
        {
            Assert.That(() => _emailSPcomb.SendEmailWithCombinationString("test", null), Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void SendEmailWithCombinationString_WrongPathToEmailView()
        {
            Assert.That(() => _emailSP.SendEmailWithCombinationString("test", "vertisio.com@gmail.com"), Throws.Exception.TypeOf<System.Exception>());
        }

        [Test]
        public void SendEmailWithCombinationString_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _emailSPcomb.SendEmailWithCombinationString("test", "vertisio.com@gmail.com"));
        }


        [Test]
        public void SendSubscriberEmail_ExceptionWhenEmailAddressIsNull()
        {
            SubscriberEmail se = new SubscriberEmail()
            {
                EmailAddress = null,
                SubscriberList = new List<string>()
                { "Kultura", "Sport" }
            };

            Assert.That(() => _emailSP.SendSubscriberEmail(se), Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void SendSubscriberEmail_WrongPathToEmailView()
        {
            SubscriberEmail se = new SubscriberEmail()
            {
                EmailAddress = "vertisio.com@gmail.com",
                SubscriberList = new List<string>()
                { "Kultura", "Sport" }
            };

            Assert.That(() => _emailSPcomb.SendSubscriberEmail(se), Throws.Exception.TypeOf<System.Exception>());
        }
     
        [Test]
        public void SendSubscriberEmail_DoesNotThrow()
        {
            SubscriberEmail se = new SubscriberEmail()
            {
                EmailAddress = "vertisio.com@gmail.com",
                SubscriberList = new List<string>()
                { "Kultura", "Sport" }
            };

            Assert.DoesNotThrow(() => _emailSP.SendSubscriberEmail(se));
        }

   


    }
}
