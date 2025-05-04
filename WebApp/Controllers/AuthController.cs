using Business.Services;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        [HttpGet]
        public IActionResult Login(string returnUrl = "~/")
        {
            ViewBag.ErrorMessage = "";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginForm form, string returnUrl = "~/")
        {
            // debug-line
            System.Diagnostics.Debug.WriteLine($"RETURN URL: {returnUrl}");

            ViewBag.ErrorMessage = "";

            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(form);

                System.Diagnostics.Debug.WriteLine($"LOGIN RESULT: {result.Succeeded}");

                if (result.Succeeded)
                {
                    await _authService.SignInAsync(form);
                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorMessage = result.Error ?? "Login failed.";
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login form.";
            }

            return View(form);
        }

        [HttpGet]
        public IActionResult SignUp(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? "/projects";
            ViewData["Title"] = "Sign Up";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(MemberSignUpForm form)
        {
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                var result = await _authService.SignUpAsync(form);

                if (result.Succeeded)
                    return LocalRedirect("~/");

                ViewBag.ErrorMessage = result.Error ?? "Signup failed.";
            }
            else
            {
                ViewBag.ErrorMessage = "Form validation failed.";
            }

            return View(form);
        }
    }
}