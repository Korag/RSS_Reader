using NUnit.Framework;
using Rss_Downloader.Services;
using RssModelsLib.Models;
using System;
using System.Collections.Generic;
using System.IO;

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

        [Test]
        public void FillSingleDocumentWithSubContent_WhenParameterIsNotNull_FillMainDocumentWithSubContent()
        {
            RssDocumentSingle testDocument = new RssDocumentSingle();
            testDocument.RssDocumentContent = new List<RssDocumentItem>();
            testDocument.Link = "https://www.rmf24.pl/tylko-w-rmf24/feed";


            _downloader.FillSingleDocumentWithSubContent(testDocument);

            var result = testDocument.RssDocumentContent.Count;
            Assert.That(result, Is.Not.Zero);
        }

        [Test]
        public void FillSingleDocumentWithSubContent_WhenParameterIsNull_ThrowNullReferenceException()
        {
            RssDocumentSingle testDocument = null;

            Assert.That(() => _downloader.FillSingleDocumentWithSubContent(testDocument), Throws.Exception.TypeOf<NullReferenceException>());
        }

        [Test]
        public void FillSingleDocumentWithSubContent_WhenLinkFromMainDocumentIsInvalid_ThrowFileNotFoundException()
        {
            RssDocumentSingle testDocument = new RssDocumentSingle();
            testDocument.RssDocumentContent = new List<RssDocumentItem>();
            testDocument.Link = "InvalidLink";

            Assert.That(() => _downloader.FillSingleDocumentWithSubContent(testDocument), Throws.TypeOf<FileNotFoundException>());
        }

    }
}
