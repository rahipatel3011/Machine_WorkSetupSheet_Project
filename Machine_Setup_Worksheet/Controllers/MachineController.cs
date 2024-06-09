﻿using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    [Route("/machines")]
    public class MachineController : Controller
    {
        private readonly IMachineService _machineService;

        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
            return View(machines);
        }


        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
                ViewBag.Open = true;
                return View("Index", machines);
            }
            catch (Exception ex)
            {

                ViewBag.Errors = new[] { ex.Message }.ToList();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] MachineDTO machineDTO)
        {

            ViewBag.Open = true;
            if (!ModelState.IsValid)
            {
                IEnumerable<MachineDTO> machines= await _machineService.GetAllMachines();
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(u => u.ErrorMessage);
                return View("Index", machines);
            }

            try
            {
                await _machineService.SaveMachine(machineDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Errors = new[] { ex.Message }.ToList();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("edit/{MachineId}")]
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


        [HttpGet("delete/{MachineId}")]
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

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMachine([FromForm] Guid MachineId)
        {
            int affectedRow = await _machineService.DeleteMachine(MachineId);
            if (affectedRow > 0)
            {
                return RedirectToAction("Index", "Machine");
            }

            ViewBag.Errors = new[] { "Machine cannot be deleted please try again later" }.ToList();
            await SetViewBag(MachineId, "Delete");
            IEnumerable<MachineDTO> machines = await _machineService.GetAllMachines();
            return View("Index", machines);
        }



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
                ViewBag.Delete = true;
            }

        }
    }
}
