using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    public class SetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Route("/create")]
        public IActionResult CreateSetup()
        {
            return View();
        }
    }
}
