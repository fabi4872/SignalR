using System.Text.Json.Serialization;

namespace SignalR.Models
{
    public class VentaVehiculo
    {
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public string Origen { get; set; }
        public string Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Precio { get; set; }
        [JsonIgnore]
        public string Fecha { get; set; }
    }
}
