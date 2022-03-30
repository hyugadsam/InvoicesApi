using Dtos.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebSite.Models;
using WebSite.Models.Authors;
using WebSite.Services;

namespace WebSite.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ILogger<AuthorsController> logger;
        private readonly IWebApiService apiService;

        public AuthorsController(ILogger<AuthorsController> logger, IWebApiService apiService)
        {
            this.logger = logger;
            this.apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            var result = await apiService.GetAuthorList();
            ViewBag.Message = message;
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int AuthorId)
        {
            var author = await apiService.GetAuthor(AuthorId);
            if (author == null || author?.AuthorId != AuthorId)
                return NotFound();

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AuthorDto model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await apiService.UpdateAuthor(model);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = $"Error: {result?.Message}";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int AuthorId)
        {
            var result = await apiService.Delete(AuthorId);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new { result.Message });
        }


        [HttpPost]
        public async Task<IActionResult> Create(NewAuthorModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await apiService.CreateAuthor(model.Name);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = $"Error: {result?.Message}";
                return View(model);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Details(int AuthorId)
        {
            var author = await apiService.GetAuthor(AuthorId);
            if (author == null || author?.AuthorId != AuthorId)
                return NotFound();

            return View(author);
        }




    }
}
