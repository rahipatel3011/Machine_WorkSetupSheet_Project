
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
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


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<WorkSetupDTO> allWorkSetupDTO = await _workSetupService.GetAllWorkSetupAsync();
                return View(allWorkSetupDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an Error occured while fetching all work setups");
            }
        }


        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> Detail(Guid Id)
        {
            try
            {
                WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
                if (workSetupDTO.WorkSetupName == null)
                {
                    return RedirectToAction("Index", "WorkSetup");
                }
                return View(workSetupDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "an Error occured while fetching a work setup");
            }
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
            try
            {
                if (ModelState.IsValid)
                {
                    await _workSetupService.SaveWorkSetup(workSetupDTO);
                    return RedirectToAction("Index");
                }

                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(error => error.ErrorMessage);
                return View("Create", workSetupDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an Error occured while creating a work setup");
            }
        }

        [HttpGet("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id)
        {
            try
            {
                WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
                return View("Create", workSetupDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an Error occured while loading edit work setup page");
            }
        }


        [HttpGet("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id, WorkSetupDTO workSetupDTO1)
        {
            try
            {
                WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
                if (workSetupDTO.WorkSetupName == null)
                {
                    return RedirectToAction("Index", "WorkSetup");
                }

                ViewBag.isDeleteModal = true;
                return View("Detail", workSetupDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "an Error occured while loading delete work setup");
            }
        }


        [HttpPost("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete([FromForm]Guid WorkSetupId)
        {
            try
            {
                await _workSetupService.DeleteWorkSetup(WorkSetupId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an Error occured while creating a work setup");
            }
        }

    }
}
