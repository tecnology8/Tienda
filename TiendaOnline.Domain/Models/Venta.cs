using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaOnline.Domain.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Contacto { get; set; }
        public string DistritoId { get; set; }
        public string Telefono { get; set; }
        public string TransaccionId { get; set; }
        public DateTime  FechaVenta { get; set; }
        public List<DetalleVenta> DetalleVentas { get; set; }
    }
}