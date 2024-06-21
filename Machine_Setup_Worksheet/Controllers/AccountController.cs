using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;


        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("/register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(u=>u.Errors).Select(u=>u.ErrorMessage).ToList();
                return View(registerDTO);
            }

            IdentityResult identityResult = await _userService.CreateUserAsync(registerDTO);
            if (identityResult.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ViewBag.Register = identityResult.Errors.Select(u=>u.Description).ToList();
            return View(registerDTO);
        }

        [HttpGet("/login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO, [FromQuery]string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(u => u.ErrorMessage).ToList();
                return View(loginDTO);
            }

            bool isLoggedIn = await _userService.LoginUserAsync(loginDTO);
            if(isLoggedIn)
            {
                if(!String.IsNullOrWhiteSpace(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ViewBag.Login = new[] { "Invalid Creadentials" }.ToList();
            return View(loginDTO);
        }



        [HttpPost("/logout")]
        public async Task<IActionResult> Logout() 
        {
            await _userService.LogoutUserAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
