using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Infrastructure
{
    public class CarritoService
    {
        private CarritoDb carritoDb = new CarritoDb();
        public bool ExisteCarrito(int clienteid, int productoid)
        {
            return carritoDb.ExisteCarrito(clienteid, productoid);
        }
        public bool OperacionCarrito(int clienteid, int productoid, bool sumar, out string mensaje)
        {
            return carritoDb.OperacionCarrito(clienteid, productoid, sumar, out mensaje);
        }
        public int CantidadEnCarrito(int id)
        {
            return carritoDb.CantidadEnCarrito(id);
        }
        public List<Carrito> ListarProducto(int Idcliente)
        {
            return carritoDb.ListarProducto(Idcliente);
        }
        public bool EliminarCarrito(int clienteid, int productoid)
        {
            return carritoDb.EliminarCarrito(clienteid, productoid);
        }
    }
}