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

        [Test]
        public void AddNewContentToDocuments_WhenThereIsNoDocumentsWithNewContent_ReturnUnchangedListOfDocuments()
        {
            List<RssDocumentSingle> documentsWithNewContentAvailable = new List<RssDocumentSingle>
            {
                new RssDocumentSingle()
                {
                    LastFetched = new DateTime(2020,1,1),
                    Link = "https://www.rmf24.pl/tylko-w-rmf24/feed",
                    RssDocumentContent = new List<RssDocumentItem>()
                }
            };

            var result = _downloader.AddNewContentToDocuments(documentsWithNewContentAvailable);

            Assert.That(result[0].RssDocumentContent.Count, Is.Zero);
        }

        [Test]
        public void AddNewContentToDocuments_WhenThereAreDocumentsWithNewContent_ReturnChangedListOfDocuments()
        {
            List<RssDocumentSingle> documentsWithNewContentAvailable = GetDummyListOfRssDocuments(correctDates: true);

            var result = _downloader.AddNewContentToDocuments(documentsWithNewContentAvailable);

            Assert.That(result[0].RssDocumentContent, Is.Not.Zero);
        }

        [Test]
        public void AddNewContentToDocuments_WhenParameterIsNull_ThrowNullReferenceException()
        {
            List<RssDocumentSingle> documentsWithNewContentAvailable = null;

            Assert.That(() => _downloader.AddNewContentToDocuments(documentsWithNewContentAvailable),
                Throws.Exception.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GetDocumentsWithNewContentAvailable_WhenThereAreDocumentsWithNewContentAvailable_ReturnNotEmptyList()
        {
            List<RssDocumentSingle> documentsWithNewContentAvailable = GetDummyListOfRssDocuments(correctDates: true);

            var result = _downloader.GetDocumentsWithNewContentAvailable(documentsWithNewContentAvailable);

            Assert.That(result, Is.Not.Zero);
        }

        [Test]
        public void GetDocumentsWithNewContentAvailable_WhenThereAreNotDocumentsWithNewContentAvailable_ReturnEmptyList()
        {
            List<RssDocumentSingle> documentsWithNewContentAvailable = GetDummyListOfRssDocuments(correctDates: false);

            var result = _downloader.GetDocumentsWithNewContentAvailable(documentsWithNewContentAvailable);

            Assert.That(result, Is.Empty);
        }



        private List<RssDocumentSingle> GetDummyListOfRssDocuments(bool correctDates)
        {
            List<RssDocumentSingle> dummyRssDocuments = new List<RssDocumentSingle>();
            int i = -1;
            if (!correctDates)
            {
                i = 1;
            }
            for (int j = 0; j < 31; j++)
            {
                dummyRssDocuments.Add(new RssDocumentSingle()
                {
                    LastFetched = DateTime.Now.AddYears(i),
                    Link = "https://www.rmf24.pl/tylko-w-rmf24/feed",
                    RssDocumentContent = new List<RssDocumentItem>()
                });
            }
            return dummyRssDocuments;
        }

    }
}
