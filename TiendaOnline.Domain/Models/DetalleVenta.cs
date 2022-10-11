using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaOnline.Domain.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public Producto ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string TransaccionId { get; set; }
    }
}