using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsRealm.Models
{
    public class NewsModel
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("source")]

        public string Source { get; set; }
        [Column("title")]

        public string Title { get; set; }
        [Column("description")]

        public string Description { get; set; }
        [Column("maintext")]

        public string MainText { get; set; }
        [Column("image_url")]

        public string ImageUrl {  get; set; }
        [Column("authors")]

        public string Authors {  get; set; }
        [Column("category")]

        public string Category { get; set; }
        [Column("date_publish")]
        public string DatePublish { get; set; }
        [Column("time_publish")]
        public string TimePublish { get; set; }
        [Column("language")]
        public string Language { get; set; }
        [Column("url")]
        public string Url { get; set; }
        [Column("created_at")]
        public string CreatedAt { get; set; }
    }
}
