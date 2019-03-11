using MongoDB.Driver;
using RssDbContextLib.Db_Context;
using RssModelsLib.Models;
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

        public List<RssDocumentSingle> GetAllRssDocuments()
        {
            return _context.GetAllRssDocumentsFromDatabase().Find(x => true).ToList();
        }

        public RssDocumentSingle GetRssDocument(string id)
        {
            return _context.GetDocumentByIdFromDatabase(id);
        }
    }
}
