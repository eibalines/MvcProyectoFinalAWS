using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcProyectoFinalAWS.Services;
using NugetPadelAWS_DSC;


namespace MvcProyectoFinalAWS.Controllers
{
    public class UsuariosController : Controller
    {
        private ServicePadel service;
        public UsuariosController(ServicePadel service)
        {
            this.service = service;
        }

        public async Task<IActionResult> PerfilUsuario()
        {
            string token = HttpContext.User.FindFirst("token").Value;
            Usuario usuario = await this.service.GetUsuarioToken(token);   
            return View(usuario);
        }
        public async Task<ActionResult> PartidosUsuario()
        {
            string token = HttpContext.User.FindFirst("token").Value;
            int idusuario = int.Parse(HttpContext.User.FindFirst("idusuario").Value);
            List<Partido> partidos = await this.service.GetPartidosUsuario(idusuario, token);
            return View(partidos);
        }
    }
}
