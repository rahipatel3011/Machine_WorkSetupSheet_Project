using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
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

    }
}
