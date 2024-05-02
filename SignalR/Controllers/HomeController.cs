using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public HomeController(IHubContext<VentaVehiculoHub> ventaVehiculoHub, IConfiguration configuration)
        {
            _ventaVehiculoHub = ventaVehiculoHub;
            _configuration = configuration;
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

                // Convertir el precio a decimal
                if (decimal.TryParse(ventaVehiculo.Precio, out decimal precioDecimal))
                {
                    // Formatear el precio
                    string precioFormateado = precioDecimal.ToString("#,##0.00");

                    // Enviar el precio formateado a través de SignalR
                    await _ventaVehiculoHub.Clients.All.SendAsync(_configuration["MetodoVentaVehiculoHub"], ventaVehiculo.Id, ventaVehiculo.Origen, ventaVehiculo.Tipo,
                        ventaVehiculo.Marca, ventaVehiculo.Modelo, precioFormateado, ventaVehiculo.Fecha);
                }
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
