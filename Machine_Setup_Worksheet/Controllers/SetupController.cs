using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Machine_Setup_Worksheet.Controllers
{
    [Route("/setup/{id:Guid}")]
    public class SetupController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IJawService _jawService;
        private readonly ISetupService _setupService;

        public SetupController(IJawService jawService, ISetupService setupService, IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
            _jawService = jawService;
            _setupService = setupService;
        }

        public IActionResult Index()
        {
            return View();
        }


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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]SetupDTO setupDTO)
        {
            
                if (!ModelState.IsValid)
                {
                    ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(error => error.ErrorMessage);
                    return View(setupDTO);
                }

                // retrived image url from whole base64string 
                string ImageUrl = setupDTO.SetupImage.Split(",")[1];

                // Convert above imageUrl to Byte Array
                byte[] imageBytes = Convert.FromBase64String(ImageUrl);

                string FileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".png";
                string FilePathDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images/SetupImages/", FileName);

                CreateImageFile(FilePathDirectory, imageBytes);

                setupDTO.SetupImage = "/images/SetupImages/" + FileName;

                await _setupService.SaveSetup(setupDTO);
                return RedirectToAction("Detail", "WorkSetup", new { id = setupDTO.WorkSetupId });
           
        }


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


        [HttpPost("{setupId:Guid}")]
        public async Task<IActionResult> Update([FromForm] SetupDTO setupDTO)
        {
            
                if (!ModelState.IsValid)
                {
                    ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(error => error.ErrorMessage);
                    return View(setupDTO);
                }

                string tempSetupImage = setupDTO.ImageUrl.TrimStart('/'); ;




                // retrived image url from whole base64string 
                string ImageUrl = setupDTO.SetupImage.Split(",")[1];

                // Convert above imageUrl to Byte Array
                byte[] imageBytes = Convert.FromBase64String(ImageUrl);


                string FileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + ".png";
                //string FilePathDirectory = @"wwwroot\images\SetupImages\" + FileName;
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




        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromForm]SetupDTO setupDTO)
        {
            
                JawsDTO jaw = await _jawService.GetJawById(setupDTO.JawId);
                setupDTO.Jaw = jaw;
                ViewBag.isDeleteModal = true;
                //ViewBag.setup = setupDTO; // need it in ViewComponent'View to display setupId
                return View(setupDTO);
           
        }


        [HttpPost("delete/{setupId:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid setupId)
        {
            
                // getting setupDTO to get imageUrl and delete it from Root
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
        /// Delete Image File
        /// </summary>
        /// <param name="Path">Root Path of the Image</param>
        /// <returns>True/False</returns>
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
        /// Create Image File
        /// </summary>
        /// <param name="FilePathDirectory">File Directory</param>
        /// <param name="imageBytes">Byte array of the image</param>
        /// <returns>True/False</returns>
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
