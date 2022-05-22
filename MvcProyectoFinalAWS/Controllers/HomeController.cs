using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProyectoFinalAWS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NugetPadelAWS_DSC;
using MvcProyectoFinalAWS.Services;

namespace MvcProyectoFinalAWS.Controllers
{
    public class HomeController : Controller
    {
        private ServicePadel service;
        public HomeController(ServicePadel service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<CampoPadel> campos =
                await this.service.GetTodosCampos();
            return View(campos);
        }
    }
}
