using Microsoft.EntityFrameworkCore;

namespace NewsRealm.Models
{
    public class NewsModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string LinkToResource { get; set; }

        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;


    }
}
