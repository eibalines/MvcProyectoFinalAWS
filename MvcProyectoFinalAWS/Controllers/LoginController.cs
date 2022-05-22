using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoFinalAWS.Services;
using NugetPadelAWS_DSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcProyectoFinalAWS.Controllers
{
    public class LoginController : Controller
    {
        private ServicePadel service;

        public LoginController(ServicePadel service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string mail, string password)
        {
            string token =
                await this.service.GetTokenAsync(mail, password);
            if (token == null)
            {
                ViewData["MENSAJE"] = "Mail/Password incorrectos";
                return View();
            }
            else
            {
                Usuario user = await this.service.GetUsuario(token);
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.Mail));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()));
                identity.AddClaim(new Claim("password", user.Password));
                identity.AddClaim(new Claim("username", token));
                identity.AddClaim(new Claim("TOKEN", token));
                identity.AddClaim(new Claim("TOKEN", token));
                identity.AddClaim(new Claim("TOKEN", token));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string mail, string password, string nombre, string apellidos, string pregunta)
        {
            ViewData["MENSAJE"] = "Usuario añadido correctamente";

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
            (CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("TOKEN");

            return RedirectToAction("Index", "Home");



        }


    }
}
