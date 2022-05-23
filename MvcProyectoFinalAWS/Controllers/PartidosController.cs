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

        public async Task<IActionResult> MisPartidos()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            int idusuario = int.Parse(HttpContext.User.FindFirst("idusuario").Value);
            List<Partido> partidos = await this.service.GetPartidosUsuario(idusuario, token);
            return View(partidos);
        }

        public async Task<IActionResult> CrearPartido(int idcampo)
        {
          CampoPadel campo = await this.service.FindCampo(idcampo);
            return View(campo);
        }
        [HttpPost]
        public async Task<IActionResult> CrearPartido(Partido partido)
        {

           int idusuario =  int.Parse(HttpContext.User.FindFirst("idusuario").Value);
           await this.service.CrearPartido(partido.IdCampo, partido.IdPista, partido.Fecha, partido.HoraInicio, partido.HoraFinal, partido.Precio, idusuario, partido.NombrePista);

            return RedirectToAction("MisPartidos");
        }


        public async Task<ActionResult> BorrarPartido(int idpartido)
        {
            await this.service.BorrarPartido(idpartido);
            return RedirectToAction("MisPartidos");
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
