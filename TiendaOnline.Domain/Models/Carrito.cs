using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaOnline.Domain.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public Cliente ClienteId { get; set; }
        public Producto ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}