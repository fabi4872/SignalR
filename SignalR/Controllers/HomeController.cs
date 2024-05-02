using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<VentaVehiculoHub> _ventaVehiculoHub;

        public HomeController(IHubContext<VentaVehiculoHub> ventaVehiculoHub)
        {
            _ventaVehiculoHub = ventaVehiculoHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult VentaVehiculoForm()
        {
            return View();
        }

        public async Task<IActionResult> AddVentaVehiculo(VentaVehiculo ventaVehiculo)
        {
            if (ModelState.IsValid && ventaVehiculo.Tipo != null && ventaVehiculo.Marca != null && ventaVehiculo.Modelo != null)
            {
                ventaVehiculo.Origen = "INTERNO";
                ventaVehiculo.Tipo = ventaVehiculo.Tipo.ToUpper();
                ventaVehiculo.Marca = ventaVehiculo.Marca.ToUpper();
                ventaVehiculo.Modelo = ventaVehiculo.Modelo.ToUpper();
                ventaVehiculo.Id = Guid.NewGuid().ToString();
                ventaVehiculo.Id = ventaVehiculo.Id.Substring(0, ventaVehiculo.Id.Length - 18);
                ventaVehiculo.Fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                await _ventaVehiculoHub.Clients.All.SendAsync("Receive", ventaVehiculo.Id, ventaVehiculo.Origen, ventaVehiculo.Tipo,
                    ventaVehiculo.Marca, ventaVehiculo.Modelo, ventaVehiculo.Precio, ventaVehiculo.Fecha);
            }
            
            return View("VentaVehiculoForm");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
