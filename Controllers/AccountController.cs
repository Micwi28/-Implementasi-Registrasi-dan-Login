using Microsoft.AspNetCore.Mvc;
using SecureWeb.Models;
using SecureWeb.Data;
using SecureWeb.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SecureWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _userData;

        public AccountController(IUser user)
        {
            _userData = user;
        }
        // GET: AccountController1
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationViewModel registrationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new Models.User
                    {
                        Username = registrationViewModel.Username,
                        Password = registrationViewModel.Password,
                        RoleName = "contributor"
                    };
                    _userData.Registration(user);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View(registrationViewModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                loginViewModel.ReturnUrl = loginViewModel.ReturnUrl ?? Url.Content("~/");
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Username = loginViewModel.Username,
                        Password = loginViewModel.Password
                    };

                    var loginUser = _userData.Login(user);
                    if(loginUser == null)
                    {
                        ViewBag.Message = "Invalid login attempt";
                        return View(loginViewModel);
                    }

                    var claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                    var identity = new ClaimsIdentity(claim, 
                        CookieAuthenticationDefaults.AuthenticationScheme);
                    
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, 
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = loginViewModel.RememberLogin
                        });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Modelstate not valid";
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(loginViewModel);
        }    
    }
}
