using Dtos.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebSite.Interfaces;
using WebSite.Models.Clients;

namespace WebSite.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> logger;
        private readonly IClientsAppService service;
        private readonly IBaseAppService baseService;

        public ClientsController(ILogger<ClientsController> logger, IClientsAppService service, IBaseAppService baseService)
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
        public async Task<IActionResult> Create(NewClientModel model)
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
        public async Task<IActionResult> Update(int ClientId)
        {
            var model = await service.GetUpdateModel(ClientId);
            if (model == null || model?.ClientId != ClientId)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateClientModel model)
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
                return RedirectToAction(nameof(Index), new { message = $"Error: {result?.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int ClientId)
        {
            var client = await service.Get(ClientId);
            if (client == null || client?.ClientId != ClientId)
                return NotFound();

            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int ClientId)
        {
            var result = await service.Delete(ClientId);
            if (result?.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new { result.Message });
        }


    }
}
