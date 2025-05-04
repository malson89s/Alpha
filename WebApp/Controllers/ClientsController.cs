using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Business.Services;

namespace WebApp.Controllers
{
    [Route("admin/clients")]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Clients.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddClientForm form)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                    );
                return BadRequest(new { success = false, errors });
            }

            var result = await _clientService.AddClientAsync(form);

            if (result.Succeeded)
                return Ok(new { success = true });

            return Problem(result.Error ?? "Unable to add client.");
        }



        [HttpPost]
        public async Task<IActionResult> Edit(EditClientForm form)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                    );
                return BadRequest(new { success = false, errors });
            }

            var result = await _clientService.UpdateClientAsync(form);

            if (result.Succeeded)
                return Ok(new { success = true });

            return Problem(result.Error ?? "Unable to update client.");
        }

    }
}

