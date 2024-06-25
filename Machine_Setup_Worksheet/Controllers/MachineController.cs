using Machine_Setup_Worksheet.Attributes;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services;
using Machine_Setup_Worksheet.Services.IServices;
using Machine_Setup_Worksheet.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Machine_Setup_Worksheet.Controllers
{
    /// <summary>
    /// Controller responsible for managing Machines.
    /// </summary>
    [Route("/machines")]
    public class MachineController : Controller
    {
        private readonly IMachineService _machineService;

        /// <summary>
        /// Constructor to initialize the MachineController with IMachineService dependency.
        /// </summary>
        /// <param name="machineService">Instance of IMachineService for machine operations.</param>
        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        /// <summary>
        /// GET: /machines
        /// Displays a list of all Machines.
        /// </summary>
        /// <returns>Returns a view with a list of Machines.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
            return View(machines);
        }

        /// <summary>
        /// GET: /machines/create
        /// Displays the form to create a new Machine.
        /// </summary>
        /// <returns>Returns a view to create a new Machine.</returns>
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
            ViewBag.Open = true;
            return View("Index", machines);
        }

        /// <summary>
        /// POST: /machines/create
        /// Handles submission of the form to create a new Machine.
        /// </summary>
        /// <param name="machineDTO">Data of the Machine to create.</param>
        /// <returns>Returns a redirect to the Index action if creation is successful; otherwise, returns to the create form with errors.</returns>
        [HttpPost("create")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] MachineDTO machineDTO)
        {
            ViewBag.Open = true;
            if (!ModelState.IsValid)
            {
                IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(u => u.ErrorMessage);
                return View("Index", machines);
            }
            await _machineService.SaveMachine(machineDTO);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /machines/edit/{MachineId}
        /// Displays the form to edit a Machine identified by MachineId.
        /// </summary>
        /// <param name="MachineId">ID of the Machine to edit.</param>
        /// <returns>Returns a view to edit the specified Machine.</returns>
        [HttpGet("edit/{MachineId}")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        public async Task<IActionResult> EditMachine([FromRoute] Guid? MachineId)
        {
            if (MachineId == null)
            {
                return RedirectToAction(nameof(Index), nameof(JawsController));
            }
            await SetViewBag(MachineId.Value, "Save");
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
            return View("Index", machines);
        }

        /// <summary>
        /// GET: /machines/delete/{MachineId}
        /// Displays the confirmation dialog to delete a Machine identified by MachineId.
        /// </summary>
        /// <param name="MachineId">ID of the Machine to delete.</param>
        /// <returns>Returns a view with the delete confirmation dialog for the specified Machine.</returns>
        [HttpGet("delete/{MachineId}")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        public async Task<IActionResult> DeleteJaw([FromRoute] Guid? MachineId)
        {
            if (MachineId == null)
            {
                return RedirectToAction("Index", "Machine");
            }
            await SetViewBag(MachineId.Value, "Delete");
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
            return View("Index", machines);
        }

        /// <summary>
        /// POST: /machines/delete
        /// Handles the deletion of a Machine identified by MachineId.
        /// </summary>
        /// <param name="MachineId">ID of the Machine to delete.</param>
        /// <returns>Returns a redirect to the Index action if deletion is successful; otherwise, returns to the Index with errors.</returns>
        [HttpPost("delete")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMachine([FromForm] Guid MachineId)
        {
            int affectedRow = await _machineService.DeleteMachine(MachineId);
            if (affectedRow > 0)
            {
                return RedirectToAction("Index", "Machine");
            }

            ViewBag.Errors = new[] { "Machine cannot be deleted, please try again later." }.ToList();
            await SetViewBag(MachineId, "Delete");
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
            return View("Index", machines);
        }

        /// <summary>
        /// Sets view bag data for the specified MachineId and mode (Save/Delete).
        /// </summary>
        /// <param name="machineId">ID of the Machine.</param>
        /// <param name="SaveOrDeleteModal">Mode: "Save" for editing, "Delete" for deletion.</param>
        /// <returns>Returns a view with additional data set in ViewBag.</returns>
        private async Task SetViewBag(Guid machineId, string SaveOrDeleteModal)
        {
            MachineDTO machine = await _machineService.GetMachineById(machineId);
            ViewBag.machine = machine;
            if (SaveOrDeleteModal.ToLower() == "save")
            {
                ViewBag.Open = true;
            }
            else if (SaveOrDeleteModal.ToLower() == "delete")
            {
                ViewBag.IsDeleteModal = true;
            }
        }
    }
}
