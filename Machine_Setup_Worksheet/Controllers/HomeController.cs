using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWorkSetupService _workSetupService;

        public HomeController(IWorkSetupService workSetupService) 
        {
            _workSetupService = workSetupService;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("/error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet("/status/403")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
