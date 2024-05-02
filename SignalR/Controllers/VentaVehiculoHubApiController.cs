using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaVehiculoHubApiController : ControllerBase
    {
        private readonly IHubContext<VentaVehiculoHub> _ventaVehiculoHub;
        private readonly IConfiguration _configuration;

        public VentaVehiculoHubApiController(IHubContext<VentaVehiculoHub> ventaVehiculoHub, IConfiguration configuration)
        {
            _ventaVehiculoHub = ventaVehiculoHub;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AddVentaVehiculo(VentaVehiculo ventaVehiculo)
        {
            if (ventaVehiculo.Tipo != null && ventaVehiculo.Marca != null && ventaVehiculo.Modelo != null && ventaVehiculo.Precio != null)
            {
                ventaVehiculo.Origen = "EXTERNO";
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

                    await _ventaVehiculoHub.Clients.All.SendAsync(_configuration["MetodoVentaVehiculoHub"], ventaVehiculo.Id, ventaVehiculo.Origen, ventaVehiculo.Tipo,
                        ventaVehiculo.Marca, ventaVehiculo.Modelo, precioFormateado, ventaVehiculo.Fecha);
                }
            }

            return Ok();
        }
    }
}
