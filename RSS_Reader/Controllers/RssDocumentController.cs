using MongoDB.Driver;
using Rss_Reader.Db_Context;
using Rss_Reader.Models;
using System.Collections.Generic;
using System.Web.Http;
namespace RSS_Reader.Controllers
{
    public class RssDocumentController : ApiController
    {
        private IRssDocumentsRepository _context;

        public RssDocumentController()
        {
            _context = new RssDocumentsRepository();
        }

        public List<RSSDocumentSingle> GetAllRssDocuments()
        {
            return _context.GetAllRssDocumentsFromDatabase().Find(x => true).ToList();
        }

        public RSSDocumentSingle GetRssDocument(string id)
        {
            return _context.GetDocumentByIdFromDatabase(id);
        }
    }
}
