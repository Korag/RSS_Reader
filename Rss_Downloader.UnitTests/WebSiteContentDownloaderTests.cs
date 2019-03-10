using NUnit.Framework;
using Rss_Downloader.Services;
using RssModelsLib.Models;
using System.Collections.Generic;

namespace Rss_Downloader.UnitTests
{
    [TestFixture]
    public class WebSiteContentDownloaderTests
    {
        private IWebSiteContentDownloader _downloader;
        [SetUp]
        public void SetUp()
        {
            _downloader = new WebSiteContentDownloader();
        }

        [Test]
        public void GetAllDocumentsWithoutSubContent_WhenCalled_ReturnedListIsNotEmpty()
        {
            var result = _downloader.GetAllDocumentsWithoutSubContent();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void GetAllDocumentsWithoutSubContent_WhenCalled_ReturnedListIsNotNull()
        {
            var result = _downloader.GetAllDocumentsWithoutSubContent();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetAllDocumentsWithoutSubContent_WhenCalled_ReturnedListContainsRssDocumentSingleObjects()
        {
            var result = _downloader.GetAllDocumentsWithoutSubContent();

            Assert.That(result, Is.InstanceOf(typeof(List<RssDocumentSingle>)));
        }
    }
}
