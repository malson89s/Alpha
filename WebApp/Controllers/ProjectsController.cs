using WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
//using WebApp.ViewModels;
using Business.Services;
using Core.Models;
using Domain.Dtos.Extensions;
using WebApp.Extensions;



namespace WebApp.Controllers
{
    [Route("admin/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var result = await _projectService.GetProjectsAsync();

            if (!result.Succeeded)
                return Problem(result.Error ?? "Could not load projects");

            var viewModel = new ProjectViewModel
            {
                //Projects = result.Result,
                AddProjectFormData = new AddProjectViewModel(),
                EditProjectFormData = null! 
            };

            return View("~/Views/Admin/Projects.cshtml", viewModel);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddProjectViewModel form)
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

            var dto = form.ToDto(); // kräver att du har .ToDto() som extension-metod
            var result = await _projectService.AddProjectAsync(dto);


            if (result.Succeeded)
                return RedirectToAction("Index");

            return Problem(result.Error ?? "Unable to add project.");
        }
    }
}

