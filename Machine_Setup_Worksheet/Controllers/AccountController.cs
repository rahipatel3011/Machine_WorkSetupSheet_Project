using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Machine_Setup_Worksheet.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {

            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/register")]
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

            ApplicationUser user = new ApplicationUser()
            {
                Name = registerDTO.Name,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };
            if (registerDTO.Password != null)
            {
                IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                ViewBag.Register = identityResult.Errors.Select(u=>u.Description).ToList();
            }
            
            return View(registerDTO);
        }


        [HttpGet("/login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }


        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(u => u.Errors).Select(u => u.ErrorMessage).ToList();
                return View(loginDTO);
            }

            if (loginDTO.Password != null)
            {
                ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(loginDTO.Email);
                if(applicationUser != null)
                {
                    bool isCreadentialTrue = await _userManager.CheckPasswordAsync(applicationUser, loginDTO.Password);
                    if(isCreadentialTrue)
                    {
                        await _signInManager.SignInAsync(applicationUser, false);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else
                    {
                        ViewBag.Login = new[] { "Invalid Password" }.ToList();
                    }
                }
                else
                {
                    ViewBag.Login = new[] { "Invalid Email Address" }.ToList();
                }
                
            }

            return View(loginDTO);
        }



        [HttpPost("/logout")]
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
