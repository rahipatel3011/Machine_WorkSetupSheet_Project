using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    /// <summary>
    /// Controller responsible for handling home-related actions.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IWorkSetupService _workSetupService;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Constructor to initialize HomeController with dependencies.
        /// </summary>
        /// <param name="workSetupService">Service for WorkSetup-related operations.</param>
        public HomeController(IWorkSetupService workSetupService, ILogger<HomeController> logger)
        {
            _workSetupService = workSetupService;
            _logger = logger;
        }

        /// <summary>
        /// GET: /
        /// Default action to display the home page.
        /// </summary>
        /// <returns>Returns the index view.</returns>
        [HttpGet("/")]
        public IActionResult Index()
        {
            _logger.LogWarning("Inside Home Controller");
            return View();
        }

        /// <summary>
        /// GET: /error
        /// Action to display the error page.
        /// </summary>
        /// <returns>Returns the error view.</returns>
        [HttpGet("/error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// GET: /status/403
        /// Action to handle access denied (403) status.
        /// </summary>
        /// <returns>Returns the access denied view.</returns>
        [HttpGet("/status/403")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
