namespace Rss_Reader.Models
{
    public class RssDocumentItem
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public object Image { get; set; }
        public string Links { get; set; }
        public string DateOfPublication { get; set; }
        public string Category { get; set; }
    }
}