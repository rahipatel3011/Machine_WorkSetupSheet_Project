using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Machine_Setup_Worksheet.Controllers
{
    [Route("WorkSetup")]
    public class WorkSetupController : Controller
    {
        private readonly IWorkSetupService _workSetupService;

        public WorkSetupController(IWorkSetupService workSetupService) {
            _workSetupService = workSetupService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<WorkSetupDTO> allWorkSetupDTO = await _workSetupService.GetAllWorkSetupAsync();
            return View(allWorkSetupDTO);
        }


        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> Detail(Guid Id)
        {

            WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
            return View(workSetupDTO);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkSetupDTO workSetupDTO)
        {
            if (ModelState.IsValid)
            {
                 await _workSetupService.SaveWorkSetup(workSetupDTO);
                return RedirectToAction("Index");
            }

            ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(error => error.ErrorMessage);
            return View(workSetupDTO);
        }



    }
}
