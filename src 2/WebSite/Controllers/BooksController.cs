using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebSite.Models.Books;
using WebSite.Services;
using System.Linq;
using WebSite.Models.Authors;
using Dtos.Request;
using System.Collections.Generic;

namespace WebSite.Controllers
{
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> logger;
        private readonly IWebApiService apiService;
        private readonly IMapper mapper;

        public BooksController(ILogger<BooksController> logger, IWebApiService apiService, IMapper mapper)
        {
            this.logger = logger;
            this.apiService = apiService;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            var result = await apiService.GetBookList();
            ViewBag.Message = message;
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

            var result = await apiService.CreateBook(model.Title);
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

            var book = await apiService.GetBook(BookId);
            if (book == null || book?.BookId != BookId)
                return NotFound();

            var authors = await apiService.GetAuthorList();
            var model = mapper.Map<UpdateBookModel>(book);
            model.AuthorList = mapper.Map<List<SelectAuthorModel>>(authors);
            model.AuthorList.ForEach( a => a.Check = book.AuthorsList.Any(ba => ba.AuthorId == a.AuthorId));    //Marcar los asignados
            //model.AuthorList = authors.Select(a => 
            //    new SelectAuthorModel
            //    {
            //        Author = a,
            //        Check = book.AuthorsList.Any(b => b.AuthorId == a.AuthorId) 
            //    }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBookModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var request = mapper.Map<UpdateBookRequest>(model);

            var result = await apiService.UpdateBook(request);
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
            var book = await apiService.GetBook(BookId);
            if (book == null || book?.BookId != BookId)
                return NotFound();

            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int BookId)
        {
            var result = await apiService.DeleteBook(BookId);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new { result.Message });
        }










    }
}
