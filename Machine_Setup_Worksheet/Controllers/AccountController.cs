using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Machine_Setup_Worksheet.Utility;

namespace Machine_Setup_Worksheet.Controllers
{
    /// <summary>
    /// Controller responsible for user account management (register, login, logout).
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// Constructor to initialize the AccountController with IUserService dependency.
        /// </summary>
        /// <param name="userService">Instance of IUserService for user operations.</param>
        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// GET: /register
        /// Displays the registration form for new users.
        /// </summary>
        /// <returns>Returns a view to register a new user.</returns>
        [HttpGet("/register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            _logger.LogWarning("Inside Register Controller");
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        /// <summary>
        /// POST: /register
        /// Handles user registration form submission.
        /// </summary>
        /// <param name="registerDTO">Registration form data.</param>
        /// <returns>Returns a redirect to home if registration is successful; otherwise, returns to the registration form with errors.</returns>
        [HttpPost("/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(u => u.ErrorMessage).ToList();
                return View(registerDTO);
            }

            IdentityResult identityResult = await _userService.CreateUserAsync(registerDTO);
            if (identityResult.Succeeded)
            {
                IdentityResult roleIdentityResult = await _userService.AssignRoleAsync(registerDTO.Email, registerDTO.Role);
                if (roleIdentityResult.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            ViewBag.Register = identityResult.Errors.Select(u => u.Description).ToList();
            return View(registerDTO);
        }

        /// <summary>
        /// GET: /login
        /// Displays the login form for users to authenticate.
        /// </summary>
        /// <returns>Returns a view to login.</returns>
        [HttpGet("/login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            _logger.LogWarning("Inside Login Controller");
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        /// <summary>
        /// POST: /login
        /// Handles user login form submission.
        /// </summary>
        /// <param name="loginDTO">Login form data.</param>
        /// <param name="ReturnUrl">Optional return URL to redirect after successful login.</param>
        /// <returns>Returns a redirect to home or the specified return URL if login is successful; otherwise, returns to the login form with errors.</returns>
        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO, [FromQuery] string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(u => u.ErrorMessage).ToList();
                return View(loginDTO);
            }

            bool isLoggedIn = await _userService.LoginUserAsync(loginDTO);
            if (isLoggedIn)
            {
                if (!String.IsNullOrWhiteSpace(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ViewBag.Login = new[] { "Invalid Credentials" }.ToList();
            return View(loginDTO);
        }

        /// <summary>
        /// POST: /logout
        /// Handles user logout.
        /// </summary>
        /// <returns>Returns a redirect to home after logging out the user.</returns>
        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUserAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
