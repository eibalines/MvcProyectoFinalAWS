using Microsoft.AspNetCore.Mvc;
using NugetPadelAWS_DSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcProyectoFinalAWS.Services;

namespace MvcProyectoFinalAWS.Controllers
{
    public class PartidosController : Controller
    {
        private ServicePadel service;

        public PartidosController(ServicePadel service)
        {
            this.service = service;
        }
        public IActionResult BuscarPartidos()
        {
            return View();
        }

        public async Task<IActionResult> MisPartidos(int idusuario)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            List<Partido> partidos = await this.service.GetPartidosUsuarios(idusuario, token);
            return View(partidos);
        }

       
        public ActionResult BorrarPartido()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
