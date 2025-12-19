using Microsoft.EntityFrameworkCore;

namespace NewsRealm.Models
{
    public class NewsModel
    {
        public long Id { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string MainText { get; set; }

        public string ImageUrl {  get; set; }

        public string Authors {  get; set; }

        public string Category { get; set; }

        public string DatePublish { get; set; }

        public string TimePublish { get; set; }

        public string Language { get; set; }

        public string Url { get; set; }

        public string CreatedAt { get; set; }
    }
}
