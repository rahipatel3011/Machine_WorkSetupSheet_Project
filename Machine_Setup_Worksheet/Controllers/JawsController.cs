using Machine_Setup_Worksheet.Attributes;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Machine_Setup_Worksheet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Machine_Setup_Worksheet.Controllers
{
    /// <summary>
    /// Controller responsible for managing Jaws.
    /// </summary>
    [Route("/jaws")]
    public class JawsController : Controller
    {
        private readonly IJawService _jawService;

        /// <summary>
        /// Constructor to initialize the JawsController with IJawService dependency.
        /// </summary>
        /// <param name="jawService">Instance of IJawService for jaw operations.</param>
        public JawsController(IJawService jawService)
        {
            _jawService = jawService;
        }

        /// <summary>
        /// GET: /jaws
        /// Displays a list of all Jaws.
        /// </summary>
        /// <returns>Returns a view with a list of Jaws.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            return View(jaws);
        }

        /// <summary>
        /// GET: /jaws/create
        /// Displays the form to create a new Jaw.
        /// </summary>
        /// <returns>Returns a view to create a new Jaw.</returns>
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            ViewBag.Open = true;
            return View("Index", jaws);
        }

        /// <summary>
        /// POST: /jaws/create
        /// Handles submission of the form to create a new Jaw.
        /// </summary>
        /// <param name="jawsDTO">Data of the Jaw to create.</param>
        /// <returns>Returns a redirect to the Index action if creation is successful; otherwise, returns to the create form with errors.</returns>
        [HttpPost("create")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] JawsDTO jawsDTO)
        {
            ViewBag.Open = true;
            if (!ModelState.IsValid)
            {
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(u => u.ErrorMessage);
                return View("Index", jaws);
            }

            await _jawService.SaveJaw(jawsDTO);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: /jaws/edit/{JawId}
        /// Displays the form to edit a Jaw identified by JawId.
        /// </summary>
        /// <param name="JawId">ID of the Jaw to edit.</param>
        /// <returns>Returns a view to edit the specified Jaw.</returns>
        [HttpGet("edit/{JawId}")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        public async Task<IActionResult> EditJaw([FromRoute] Guid? JawId)
        {
            if (JawId == null)
            {
                return RedirectToAction("Index", "Jaws");
            }
            await SetViewBag(JawId.Value, "Save");
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            return View("Index", jaws);
        }

        /// <summary>
        /// GET: /jaws/delete/{JawId}
        /// Displays the confirmation dialog to delete a Jaw identified by JawId.
        /// </summary>
        /// <param name="JawId">ID of the Jaw to delete.</param>
        /// <returns>Returns a view with the delete confirmation dialog for the specified Jaw.</returns>
        [HttpGet("delete/{JawId}")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        public async Task<IActionResult> DeleteJaw([FromRoute] Guid? JawId)
        {
            if (JawId == null)
            {
                return RedirectToAction("Index", "Jaws");
            }
            await SetViewBag(JawId.Value, "Delete");
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            return View("Index", jaws);
        }

        /// <summary>
        /// POST: /jaws/delete
        /// Handles the deletion of a Jaw identified by JawId.
        /// </summary>
        /// <param name="JawId">ID of the Jaw to delete.</param>
        /// <returns>Returns a redirect to the Index action if deletion is successful; otherwise, returns to the Index with errors.</returns>
        [HttpPost("delete/{JawId}")]
        [AuthorizeRoles(Roles.MACHINIST, Roles.ADMIN)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJaw([FromForm] Guid JawId)
        {
            int affectedRow = await _jawService.DeleteJaw(JawId);
            if (affectedRow > 0)
            {
                return RedirectToAction("Index", "Jaws");
            }

            ViewBag.Errors = new[] { "Jaws cannot be deleted, please try again later." }.ToList();
            await SetViewBag(JawId, "Delete");
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            return View("Index", jaws);
        }

        /// <summary>
        /// Sets view bag data for the specified JawId and mode (Save/Delete).
        /// </summary>
        /// <param name="JawId">ID of the Jaw.</param>
        /// <param name="SaveOrDeleteModal">Mode: "Save" for editing, "Delete" for deletion.</param>
        /// <returns>Returns a view with additional data set in ViewBag.</returns>
        private async Task SetViewBag(Guid JawId, string SaveOrDeleteModal)
        {
            JawsDTO jaw = await _jawService.GetJawById(JawId);
            ViewBag.jaw = jaw;
            if (SaveOrDeleteModal.ToLower() == "save")
            {
                ViewBag.Open = true;
            }
            else if (SaveOrDeleteModal.ToLower() == "delete")
            {
                ViewBag.isDeleteModal = true;
            }
        }
    }
}
