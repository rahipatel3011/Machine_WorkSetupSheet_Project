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
            try
            {
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                return View(jaws);
            }catch (Exception ex)
            {
                return StatusCode(500, "an Error Occured while fetching all jaws");
            }
        }



        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                ViewBag.Open = true;
                return View("Index", jaws);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "an Error occured while loading cretae jaw page");
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]JawsDTO jawsDTO)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(400, "an Error Occured while creating a jaw");
            }
        }

        [HttpGet("edit/{JawId}")]
        public async Task<IActionResult> EditJaw([FromRoute]Guid? JawId)
        {
            try
            {
                if (JawId == null)
                {
                    return RedirectToAction("Index", "Jaws");
                }
                await SetViewBag(JawId.Value, "Save");
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                return View("Index", jaws);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "an Error occured while loading edit jaw page");
            }
        }


        [HttpGet("delete/{JawId}")]
        public async Task<IActionResult> DeleteJaw([FromRoute] Guid? JawId)
        {
            try
            {
                if (JawId == null)
                {
                    return RedirectToAction("Index", "Jaws");
                }
                await SetViewBag(JawId.Value, "Delete");
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                return View("Index", jaws);
            }
            catch (Exception ex)
            {
                return StatusCode(401, "an Error occured while loading delete jaw page");
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteJaw([FromForm] Guid JawId)
        {
            try
            {
                int affectedRow = await _jawService.DeleteJaw(JawId);
                if (affectedRow > 0)
                {
                    return RedirectToAction("Index", "Jaws");
                }

                ViewBag.Errors = new[] { "Jaws cannot be deleted please try again later" }.ToList();
                await SetViewBag(JawId, "Delete");
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                return View("Index", jaws);
            }
            catch (Exception ex)
            {
                return StatusCode(401, "an Error occured while deleting a jaw");
            }
        }

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
