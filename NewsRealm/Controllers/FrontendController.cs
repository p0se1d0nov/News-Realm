using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsRealm.Models;
using NewsRealm.Data;

namespace NewsRealm.Controllers
{
    public class FrontendController : Controller
    {
        private readonly ILogger<FrontendController> _logger;
        private readonly NewsRealmContext _context;

        public FrontendController(ILogger<FrontendController> logger, NewsRealmContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var newsItems = await _context.NewsModel
                    .OrderByDescending(n => n.Id)
                    .Take(100)
                    .ToListAsync();
                
                return View(newsItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке новостей");
                return View(new List<NewsModel>());
            }
        }

        public IActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNews(NewsModel newsModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    newsModel.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    newsModel.DatePublish = DateTime.Now.ToString("yyyy-MM-dd");
                    newsModel.TimePublish = DateTime.Now.ToString("HH:mm:ss");
                    
                    _context.NewsModel.Add(newsModel);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Новость успешно добавлена!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при добавлении новости");
                    ModelState.AddModelError("", "Произошла ошибка при добавлении новости");
                }
            }
            
            return View(newsModel);
        }

        public async Task<IActionResult> EditNews(int? id)
        {
            if (id == null)
            {
                // Возвращаем пустую модель для отображения формы поиска
                return View(new NewsModel());
            }

            var newsModel = await _context.NewsModel.FindAsync(id);
            if (newsModel == null)
            {
                TempData["ErrorMessage"] = $"Новость с ID {id} не найдена.";
                return View(new NewsModel());
            }

            return View(newsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNews(int id, NewsModel newsModel)
        {
            if (id != newsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsModel);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Новость успешно обновлена!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsModelExists(newsModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при обновлении новости");
                    ModelState.AddModelError("", "Произошла ошибка при обновлении новости");
                }
            }
            
            return View(newsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var newsModel = await _context.NewsModel.FindAsync(id);
            if (newsModel != null)
            {
                _context.NewsModel.Remove(newsModel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Новость успешно удалена!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NewsModelExists(long id)
        {
            return _context.NewsModel.Any(e => e.Id == id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
