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
                ViewData["MENSAJEACCESODENEGADO"] = "Mail/Password incorrectos";
                return View();
            }
            else
            {
                Usuario user = await this.service.GetUsuarioToken(token);
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.Mail));
                identity.AddClaim(new Claim("idusuario", user.IdUsuario.ToString()));
                identity.AddClaim(new Claim("password", user.Password));
                identity.AddClaim(new Claim("username", user.UserName));
                identity.AddClaim(new Claim("nombre", user.Nombre));
                identity.AddClaim(new Claim("apellidos", user.Apellidos));
                identity.AddClaim(new Claim("nivel", user.Nivel.ToString()));
                identity.AddClaim(new Claim("imagen", user.Imagen));
                identity.AddClaim(new Claim("partidosJugados", user.PartidosJugados.ToString()));
                identity.AddClaim(new Claim("telefono", user.Telefono));
                identity.AddClaim(new Claim("direccion", user.Direccion));
                identity.AddClaim(new Claim("preguntaseguridad", user.PreguntaSeguridad));
                identity.AddClaim(new Claim("token", token));
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
        public async Task<IActionResult> Register(string username, string mail, string password, string nombre, string apellidos, string pregunta)
        {
            await this.service.CrearUsuario(username, mail, password, nombre, apellidos, pregunta);
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
