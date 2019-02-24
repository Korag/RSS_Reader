using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Downloader.Models
{
    public class RssDocumentItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public object Image { get; set; }
        public string Links { get; set; }
        public string DateOfPublication { get; set; }
        public string Category { get; set; }
    }
}