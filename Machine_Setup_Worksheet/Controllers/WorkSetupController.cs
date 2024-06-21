
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    [Route("WorkSetup")]
    public class WorkSetupController : Controller
    {
        private readonly IWorkSetupService _workSetupService;

        public WorkSetupController(IWorkSetupService workSetupService) {
            _workSetupService = workSetupService;
        }


        [HttpGet()]
        public async Task<IActionResult> Index([FromQuery] bool json, [FromQuery]string searchKey = "")
        {
            
                // IEnumerable<WorkSetupDTO> allWorkSetupDTO = await _workSetupService.GetAllWorkSetupAsync();
                IEnumerable<WorkSetupDTO> allWorkSetupDTO = await _workSetupService.GetBySearchAsync(searchKey);
                if(json == true)
                {
                    return new JsonResult(new { setups = allWorkSetupDTO })
                    {
                        StatusCode = 200, // Set status code to 200 OK
                        ContentType = "application/json" // Set content type explicitly
                    };
                }
                return View(allWorkSetupDTO);
            
        }


        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> Detail(Guid Id)
        {
                WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
                if (workSetupDTO.WorkSetupName == null)
                {
                    return RedirectToAction("Index", "WorkSetup");
                }
                return View(workSetupDTO);
            
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost("Save")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(WorkSetupDTO workSetupDTO)
        {
                if (ModelState.IsValid)
                {
                    await _workSetupService.SaveWorkSetup(workSetupDTO);
                    return RedirectToAction("Index");
                }

                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(error => error.ErrorMessage);
                return View("Create", workSetupDTO);
           
        }

        [HttpGet("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id)
        {
           
                WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
                return View("Create", workSetupDTO);
            
            
        }


        [HttpGet("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, WorkSetupDTO workSetupDTO1)
        {
            
                WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
                if (workSetupDTO.WorkSetupName == null)
                {
                    return RedirectToAction("Index", "WorkSetup");
                }

                ViewBag.isDeleteModal = true;
                return View("Detail", workSetupDTO);
            
        }


        [HttpPost("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete([FromForm]Guid WorkSetupId)
        {
           
                await _workSetupService.DeleteWorkSetup(WorkSetupId);
                return RedirectToAction("Index");
            
        }

    }
}
