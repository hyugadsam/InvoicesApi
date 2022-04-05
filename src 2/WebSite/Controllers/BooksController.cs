using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebSite.Models.Books;
using WebSite.Interfaces;
using Dtos.Common;

namespace WebSite.Controllers
{
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> logger;
        private readonly IBooksAppService service;
        private readonly IBaseAppService baseService;

        public BooksController(ILogger<BooksController> logger, IBooksAppService service, IBaseAppService baseService)
        {
            this.logger = logger;
            this.service = service;
            this.baseService = baseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Models.Common.PaginationDto model, string message)
        {
            if (model == null)  //Si se entra directo a index es null y manda a la pagina 1
                model = baseService.GetDefaultPagination();
            else
            {
                if (!(model.RecordsPerPage != null && model.RecordsPerPage > 0))
                {
                    model.RecordsPerPage = baseService.GetRecordsPerPage();   //Si no viene la pagina, solo usamos el int por defecto
                }
            }

            var result = await service.GetList(model);
            ViewBag.Message = message;
            ViewBag.Page = model.Page;
            ViewBag.RecordsPerPage = model.RecordsPerPage;
            ViewBag.ListRecords = baseService.GetRecordsList();

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewBookModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await service.Create(model);
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
        public async Task<IActionResult> Update(int BookId)
        {

            var model = await service.GetUpdateModel(BookId);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBookModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await service.Update(model);

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
        public async Task<IActionResult> Details(int BookId)
        {
            var book = await service.Get(BookId);
            if (book == null || book?.BookId != BookId)
                return NotFound();

            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int BookId)
        {
            var result = await service.Delete(BookId);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new { result.Message });
        }

    }
}
