using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Machine_Setup_Worksheet.Controllers
{
    /// <summary>
    /// Controller responsible for managing Setups associated with a WorkSetup.
    /// </summary>
    [Route("/setup/{id:Guid}")]
    public class SetupController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IJawService _jawService;
        private readonly ISetupService _setupService;

        /// <summary>
        /// Constructor to initialize the SetupController with dependencies.
        /// </summary>
        /// <param name="jawService">Service for Jaw-related operations.</param>
        /// <param name="setupService">Service for Setup-related operations.</param>
        /// <param name="webHostEnvironment">Provides information about the web hosting environment.</param>
        public SetupController(IJawService jawService, ISetupService setupService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _jawService = jawService;
            _setupService = setupService;
        }

        /// <summary>
        /// GET: /setup/{id:Guid}
        /// Displays the index view of Setup.
        /// </summary>
        /// <returns>Returns the index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /setup/{id:Guid}/create
        /// Displays the form to create a new Setup for a specified WorkSetup.
        /// </summary>
        /// <param name="id">ID of the WorkSetup to which the Setup belongs.</param>
        /// <returns>Returns the create view with necessary data.</returns>
        [HttpGet("create")]
        public async Task<IActionResult> Create([FromRoute] Guid id)
        {
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            ViewBag.Jaws = jaws.Select(jaw =>
            {
                return new SelectListItem() { Text = jaw.JawName, Value = jaw.JawId.ToString() };
            }).ToList();

            SetupDTO setupDTO = new SetupDTO() { WorkSetupId = id, SetupNumber = 1 };

            return View("Create", setupDTO);
        }

        /// <summary>
        /// POST: /setup/{id:Guid}/create
        /// Handles submission of the form to create a new Setup.
        /// </summary>
        /// <param name="setupDTO">Data of the Setup to create.</param>
        /// <returns>Returns a redirect to the Detail action of WorkSetup if creation is successful; otherwise, returns to the create form with errors.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SetupDTO setupDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(error => error.ErrorMessage);
                return View(setupDTO);
            }

            // Retrieve image URL from base64 string 
            string ImageUrl = setupDTO.SetupImage.Split(",")[1];

            // Convert imageUrl to byte array
            byte[] imageBytes = Convert.FromBase64String(ImageUrl);

            string FileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".png";
            string FilePathDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images/SetupImages/", FileName);

            CreateImageFile(FilePathDirectory, imageBytes);

            setupDTO.SetupImage = "/images/SetupImages/" + FileName;

            await _setupService.SaveSetup(setupDTO);
            return RedirectToAction("Detail", "WorkSetup", new { id = setupDTO.WorkSetupId });
        }

        /// <summary>
        /// GET: /setup/{id:Guid}/{setupId:Guid}
        /// Displays the form to update a Setup identified by setupId.
        /// </summary>
        /// <param name="setupId">ID of the Setup to update.</param>
        /// <returns>Returns a view to update the specified Setup.</returns>
        [HttpGet("{setupId:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid setupId)
        {
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            ViewBag.Jaws = jaws.Select(jaw =>
            {
                return new SelectListItem() { Text = jaw.JawName, Value = jaw.JawId.ToString() };
            }).ToList();

            SetupDTO setupDTO = await _setupService.GetBySetupIdAsync(setupId);

            return View(setupDTO);
        }

        /// <summary>
        /// POST: /setup/{id:Guid}/{setupId:Guid}
        /// Handles submission of the form to update a Setup identified by setupId.
        /// </summary>
        /// <param name="setupDTO">Data of the Setup to update.</param>
        /// <returns>Returns a redirect to the Detail action of WorkSetup if update is successful; otherwise, returns to the update form with errors.</returns>
        [HttpPost("{setupId:Guid}")]
        public async Task<IActionResult> Update([FromForm] SetupDTO setupDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(error => error.ErrorMessage);
                return View(setupDTO);
            }

            string tempSetupImage = setupDTO.ImageUrl.TrimStart('/');

            // Retrieve image URL from base64 string 
            string ImageUrl = setupDTO.SetupImage.Split(",")[1];

            // Convert imageUrl to byte array
            byte[] imageBytes = Convert.FromBase64String(ImageUrl);

            string FileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".png";
            string FilePathDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images/SetupImages/", FileName);

            CreateImageFile(FilePathDirectory, imageBytes);

            FileInfo file = new FileInfo(FilePathDirectory);
            setupDTO.SetupImage = "/images/SetupImages/" + FileName;
            await _setupService.SaveSetup(setupDTO);

            // Deleting older image
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, tempSetupImage);
            DeleteImageFromRoot(fullPath);

            return RedirectToAction("Detail", "WorkSetup", new { id = setupDTO.WorkSetupId });
        }

        /// <summary>
        /// POST: /setup/{id:Guid}/delete
        /// Displays the confirmation dialog to delete a Setup.
        /// </summary>
        /// <param name="setupDTO">Data of the Setup to delete.</param>
        /// <returns>Returns a view with the delete confirmation dialog for the specified Setup.</returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromForm] SetupDTO setupDTO)
        {
            JawsDTO jaw = await _jawService.GetJawById(setupDTO.JawId);
            setupDTO.Jaw = jaw;
            ViewBag.isDeleteModal = true;
            return View(setupDTO);
        }

        /// <summary>
        /// POST: /setup/{id:Guid}/delete/{setupId:Guid}
        /// Handles the deletion of a Setup identified by setupId.
        /// </summary>
        /// <param name="setupId">ID of the Setup to delete.</param>
        /// <returns>Returns a redirect to the Detail action of WorkSetup if deletion is successful; otherwise, returns to the delete confirmation view with errors.</returns>
        [HttpPost("delete/{setupId:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid setupId)
        {
            // Get setupDTO to get imageUrl and delete it from Root
            SetupDTO dbSetupDTO = await _setupService.GetBySetupIdAsync(setupId);
            string imageFullPath = Path.Combine(_webHostEnvironment.WebRootPath, dbSetupDTO.SetupImage.TrimStart('/'));

            int affectedRows = await _setupService.DeleteSetup(setupId);

            if (affectedRows <= 0)
            {
                return View(dbSetupDTO);
            }

            DeleteImageFromRoot(imageFullPath);
            return RedirectToAction("Detail", "WorkSetup", new { Id = dbSetupDTO.WorkSetupId });
        }

        #region Private Methods

        /// <summary>
        /// Deletes an image file from the root directory.
        /// </summary>
        /// <param name="Path">Root path of the image.</param>
        /// <returns>True if the image was successfully deleted; otherwise, false.</returns>
        private bool DeleteImageFromRoot(string Path)
        {
            FileInfo file1 = new FileInfo(Path);
            if (file1.Exists)
            {
                file1.Delete();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates an image file in the specified directory from byte array data.
        /// </summary>
        /// <param name="FilePathDirectory">File directory path.</param>
        /// <param name="imageBytes">Byte array of the image data.</param>
        /// <returns>True if the image file was successfully created; otherwise, false.</returns>
        private bool CreateImageFile(string FilePathDirectory, byte[] imageBytes)
        {
            using (FileStream stream = new FileStream(FilePathDirectory, FileMode.Create))
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
                return true;
            }
            return false;
        }

        #endregion
    }
}
