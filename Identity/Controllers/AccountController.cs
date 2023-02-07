using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


using WebApi.Models;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<DbUser> userManager;
        private readonly SignInManager<DbUser> signInManager;

        //ApplicationSignInManager man



        public AccountController(UserManager<DbUser> userManager, SignInManager<DbUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        //[HttpPost]
        public async Task<IActionResult> Register1(RegisterViewModel model)
        {
            var user = new DbUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                Age = model.Age,
            };
            var result= await userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Role, "e"));
            }    
            return Redirect("/home/index");

        }
  
        [AllowAnonymous]
        public IActionResult Register()
        {
            var vm = new RegisterViewModel()
            {
                Email = "aa@mmm.ru",
                Age = 37,           
            };
            AspNetUserManager<DbUser> userManager;
            SignInManager<DbUser> signIn;

            return View(vm);
        }
        
        
        [AllowAnonymous]
        public async Task<IActionResult> Login1(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            
            if(user== null)
                return Redirect("/Account/Login");
            var pass = await signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if(!pass.Succeeded)
                return Redirect("/Account/Login");
            
            return Redirect("/home/Privacy");
        }
        
        [AllowAnonymous]
        public IActionResult Login() => View();

        [Authorize(Policy = "Manager")]
        public IActionResult Manager() => View();
        
        [Authorize(Policy = "Administrator")]
        public IActionResult Administrator() => View();

        public async Task<IActionResult> Logout()
        { 
            await signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }
    }
}
