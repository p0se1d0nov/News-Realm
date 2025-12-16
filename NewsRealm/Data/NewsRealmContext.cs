using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsRealm.Models;

namespace NewsRealm.Data
{
    public class NewsRealmContext : DbContext
    {
        public NewsRealmContext (DbContextOptions<NewsRealmContext> options)
            : base(options)
        {
        }

        public DbSet<NewsRealm.Models.NewsModel> NewsModel { get; set; } = default!;
    }
}
