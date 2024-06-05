using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    [Route("/jaws")]
    public class JawsController : Controller
    {
        private readonly IJawService _jawService;

        public JawsController(IJawService jawService) { 
            _jawService = jawService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Jaw> jaws = await _jawService.getAllJaws();
            return View(jaws);
        }


        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]JawsDTO jawsDTO)
        {
            
            return View();
        }

        [HttpGet("edit")]
        public async Task<IActionResult> EditJaw([FromRoute]Guid? JawId)
        {
            if(JawId == null)
            {
                return View();
            }
            Jaw jaw = await _jawService.getJawById(JawId);

            return View(jaw);
        }
    }
}
