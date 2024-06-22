using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Machine_Setup_Worksheet.Controllers
{
    /// <summary>
    /// Controller responsible for managing WorkSetup entities.
    /// </summary>
    [Route("WorkSetup")]
    public class WorkSetupController : Controller
    {
        private readonly IWorkSetupService _workSetupService;
        private readonly IMachineService _machineService;

        /// <summary>
        /// Constructor to initialize the WorkSetupController with dependencies.
        /// </summary>
        /// <param name="workSetupService">Service for WorkSetup-related operations.</param>
        public WorkSetupController(IWorkSetupService workSetupService, IMachineService machineService)
        {
            _workSetupService = workSetupService;
            _machineService = machineService;
        }

        /// <summary>
        /// GET: /WorkSetup
        /// Displays the index view of all WorkSetups optionally filtered by searchKey.
        /// </summary>
        /// <param name="json">Flag to indicate if the response should be JSON formatted.</param>
        /// <param name="searchKey">Optional search key to filter WorkSetups.</param>
        /// <returns>Returns a view or JSON formatted response based on the 'json' parameter.</returns>
        [HttpGet()]
        public async Task<IActionResult> Index([FromQuery] bool json, [FromQuery] string searchKey = "")
        {
            IEnumerable<WorkSetupDTO> allWorkSetupDTO = await _workSetupService.GetBySearchAsync(searchKey);

            if (json)
            {
                return new JsonResult(new { setups = allWorkSetupDTO })
                {
                    StatusCode = 200, // Set status code to 200 OK
                    ContentType = "application/json" // Set content type explicitly
                };
            }

            return View(allWorkSetupDTO);
        }

        /// <summary>
        /// GET: /WorkSetup/{Id:Guid}
        /// Displays the details view of a specific WorkSetup identified by Id.
        /// </summary>
        /// <param name="Id">ID of the WorkSetup to display.</param>
        /// <returns>Returns the details view of the specified WorkSetup.</returns>
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

        /// <summary>
        /// GET: /WorkSetup/Create
        /// Displays the form to create a new WorkSetup.
        /// </summary>
        /// <returns>Returns the create view for WorkSetup.</returns>
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();

            ViewBag.Machine = machines.Select(machine =>
            {
                return new SelectListItem(machine.MachineName, machine.MachineId.ToString());
            });
            return View();
        }

        /// <summary>
        /// POST: /WorkSetup/Save
        /// Handles submission of the form to save a new or update an existing WorkSetup.
        /// </summary>
        /// <param name="workSetupDTO">Data of the WorkSetup to save.</param>
        /// <returns>Returns a redirect to the index action if save is successful; otherwise, returns to the create form with errors.</returns>
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

        /// <summary>
        /// GET: /WorkSetup/Update/{Id:Guid}
        /// Displays the form to update a specific WorkSetup identified by Id.
        /// </summary>
        /// <param name="Id">ID of the WorkSetup to update.</param>
        /// <returns>Returns the create view pre-filled with data of the specified WorkSetup.</returns>
        [HttpGet("update/{Id:Guid}")]
        public async Task<IActionResult> Update(Guid Id)
        {
            WorkSetupDTO workSetupDTO = await _workSetupService.GetWorkSetupByIdAsync(Id);
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();

            ViewBag.Machine = machines.Select(machine =>
            {
                return new SelectListItem(machine.MachineName, machine.MachineId.ToString());
            });
            return View("Create", workSetupDTO);
        }

        /// <summary>
        /// GET: /WorkSetup/Delete/{Id:Guid}
        /// Displays the confirmation dialog to delete a specific WorkSetup identified by Id.
        /// </summary>
        /// <param name="Id">ID of the WorkSetup to delete.</param>
        /// <param name="workSetupDTO1">Unused parameter.</param>
        /// <returns>Returns the detail view with delete confirmation dialog for the specified WorkSetup.</returns>
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

        /// <summary>
        /// POST: /WorkSetup/delete/{Id:Guid}
        /// Handles the deletion of a specific WorkSetup identified by Id.
        /// </summary>
        /// <param name="WorkSetupId">ID of the WorkSetup to delete.</param>
        /// <returns>Returns a redirect to the index action if deletion is successful.</returns>
        [HttpPost("delete/{Id:Guid}")]
        public async Task<IActionResult> Delete([FromForm] Guid WorkSetupId)
        {
            await _workSetupService.DeleteWorkSetup(WorkSetupId);
            return RedirectToAction("Index");
        }
    }
}
