using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.Design;

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
            string FilePathDirectory = @"wwwroot\images\SetupImages\" + FileName;


            using (FileStream stream = new FileStream(FilePathDirectory, FileMode.Create))
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
            }

            FileInfo file = new FileInfo(FilePathDirectory);
            setupDTO.SetupImage = "/images/SetupImages/" + FileName;

            await _setupService.SaveSetup(setupDTO);
            return RedirectToAction("Detail", "WorkSetup", new { id=setupDTO.WorkSetupId});
        }


        [HttpGet("{setupId:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid setupId, [FromRoute] Guid id)
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
            string FilePathDirectory = @"wwwroot\images\SetupImages\" + FileName;


            using (FileStream stream = new FileStream(FilePathDirectory, FileMode.Create))
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
            }

            FileInfo file = new FileInfo(FilePathDirectory);
            setupDTO.SetupImage = "/images/SetupImages/" + FileName;



            // Deleting older image
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, tempSetupImage);
            FileInfo file1 = new FileInfo(fullPath);
            if (file1.Exists)
            {
                file1.Delete();
            }

            await _setupService.SaveSetup(setupDTO);

            return RedirectToAction("Detail", "WorkSetup", new { id = setupDTO.WorkSetupId });
        }




        [HttpGet("")]
        public async Task<IActionResult> Delete([FromQuery] Guid setupId)
        {
            IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
            ViewBag.Jaws = jaws.Select(jaw =>
            {
                return new SelectListItem() { Text = jaw.JawName, Value = jaw.JawId.ToString() };
            }).ToList();

            SetupDTO setupDTO = await _setupService.GetBySetupIdAsync(setupId);
            ViewBag.setup = setupDTO;
            return View(setupDTO);
        }
    }
}
