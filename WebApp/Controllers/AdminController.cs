using System.Threading.Tasks;
using Business.Models;
using Business.Services;
using Data.Entities;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebApp.Controllers;


[Route("admin")]
[Authorize]
public class AdminController(IMemberService memberService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private object? _clientService;

    public IActionResult Index()
    {
        return View();
    }

    [Route("projects")]
    public IActionResult Projects()
    {
        return View();
    }

    //[Route("members")]
    ////[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> Members()
    //{
    //    var members = await _memberService.GetAllMembers();
    //    return View(members);
    //}

    [Route("members")]
    public async Task<IActionResult> Members()
    {
        var result = await _memberService.GetMembersAsync();

        if (!result.Succeeded || result.Result == null)
        {
            ViewData["Error"] = result.Error ?? "Unable to fetch members.";
            return View(new List<MemberEntity>()); 
        }

        return View(result.Result);
    }

    [Route("clients")]
    //[Authorize(Roles = "Admin")]
    public IActionResult Clients()
    {
        return View();
    }

}

