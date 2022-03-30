using AutoMapper;
using Dtos.Common;
using Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Models.Authors;
using WebSite.Models.Books;
using WebSite.Models.Clients;
using WebSite.Services;

namespace WebSite.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> logger;
        private readonly IWebApiService apiService;
        private readonly IMapper mapper;

        public ClientsController(ILogger<ClientsController> logger, IWebApiService apiService, IMapper mapper)
        {
            this.logger = logger;
            this.apiService = apiService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            var result = await apiService.GetClientList();
            ViewBag.Message = message;
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewAuthorModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await apiService.CreateClient(model.Name);
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
        public async Task<IActionResult> Update(int ClientId)
        {

            var client = await apiService.GetClient(ClientId);
            if (client == null || client?.ClientId != ClientId)
                return NotFound();

            var books = await apiService.GetBookList();
            var model = new UpdateClientModel()
            {
                ClientId = client.ClientId,
                BookList = MarkSelectedBooks(books, client.BorrowedBooks),
                Name = client.Name,
            };

            return View(model);
        }

        private List<SelectBookModel> MarkSelectedBooks(List<BookDto> books, List<BookDto> ClientBooks)
        {
            var list = new List<SelectBookModel>();
            if (books == null || books?.Count() == 0)
                return list;
            list = books.Select(b => new SelectBookModel
            {
                Book = b, Checked = ClientBooks != null ? ClientBooks.Any(bk => bk.BookId == b.BookId) : false //Marcar los asignados
            }).ToList();

            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateClientModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var request = new UpdateClientRequest
            {
                BorrowedBooks = model.BooksId,
                ClientId = model.ClientId,
                Name = model.Name,
            };

            var result = await apiService.UpdateClient(request);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index), new { message = $"Error: {result?.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int ClientId)
        {
            var client = await apiService.GetClient(ClientId);
            if (client == null || client?.ClientId != ClientId)
                return NotFound();

            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int ClientId)
        {
            var result = await apiService.DeleteClient(ClientId);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new { result.Message });
        }





    }
}
