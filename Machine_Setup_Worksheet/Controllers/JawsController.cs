using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
            
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                return View(jaws);
        }



        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                ViewBag.Open = true;
                return View("Index", jaws);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]JawsDTO jawsDTO)
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

        [HttpGet("edit/{JawId}")]
        public async Task<IActionResult> EditJaw([FromRoute]Guid? JawId)
        {
            
                if (JawId == null)
                {
                    return RedirectToAction("Index", "Jaws");
                }
                await SetViewBag(JawId.Value, "Save");
                IEnumerable<JawsDTO> jaws = await _jawService.GetAllJaws();
                return View("Index", jaws);
            
        }


        [HttpGet("delete/{JawId}")]
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

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteJaw([FromForm] Guid JawId)
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
