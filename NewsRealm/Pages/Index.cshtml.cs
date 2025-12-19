using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsRealm.Data;
using NewsRealm.Models;
using Microsoft.EntityFrameworkCore;

namespace NewsRealm.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NewsRealmContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(NewsRealmContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<NewsModel> NewsItems { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                NewsItems = await _context.NewsModel
                    .OrderByDescending(n => n.Id)
                    .Take(100)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке новостей");
                NewsItems = new List<NewsModel>();
            }
        }
    }
}

