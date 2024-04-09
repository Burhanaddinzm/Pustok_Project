using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok_Project.Data.Contexts;
using Pustok_Project.Enums;
using Pustok_Project.Models;
using Pustok_Project.ViewModels;

namespace Pustok_Project.Controllers
{
    public class AccountController : Controller
    {
        readonly AppDbContext _context;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser> signInManager,
                                 AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var existingUser = await _userManager.FindByNameAsync(loginVM.UserName);

            if (existingUser == null)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(existingUser, loginVM.Password, loginVM.RememberMe, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
            }
            else return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            if (await _userManager.FindByNameAsync(registerVM.Username) != null)
            {
                ModelState.AddModelError("Username", "This user already exists!");
                return View(registerVM);
            }

            AppUser newUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", $"{error.Code} - {error.Description}");
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(newUser, Roles.Customer.ToString());

            return RedirectToAction("login");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}
