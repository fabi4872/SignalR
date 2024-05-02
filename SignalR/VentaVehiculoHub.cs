using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalR
{
    public class VentaVehiculoHub : Hub
    {
        public async Task Send(string Id, string Origen, string Tipo, string Marca, string Modelo, double Precio, string Fecha)
        {
            await Clients.All.SendAsync("Receive", Id, Origen, Tipo, Marca, Modelo, Precio, Fecha);
        }
    }
}
