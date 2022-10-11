using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Infrastructure
{
    public class ProductoService
    {
        private ProductoDb productoDb = new ProductoDb();
        public List<Producto> Listar()
        {
            return productoDb.Listar();
        }
        public int Registrar(Producto model, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(model.Nombre) || string.IsNullOrWhiteSpace(model.Nombre))
            {
                mensaje = "El nombre del producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(model.Descripcion) || string.IsNullOrWhiteSpace(model.Descripcion))
            {
                mensaje = "La descripcion del producto no puede ser vacio";
            }
            else if (model.MarcaId.Id == 0)
            {
                mensaje = "Debe seleccionar una Marca";
            }
            else if (model.CategoriaId.Id == 0)
            {
                mensaje = "Debe seleccionar una Categoria";
            }
            else if(model.Precio == 0)
            {
                mensaje = "Debe ingresar el precio del producto";
            }
            else if (model.Stock == 0)
            {
                mensaje = "Debe ingresar el stock del producto";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                return productoDb.Registrar(model, out mensaje);
            }
            else
            {
                return 0;
            }
        }
        public bool Editar(Producto model, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(model.Nombre) || string.IsNullOrWhiteSpace(model.Nombre))
            {
                mensaje = "El nombre del producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(model.Descripcion) || string.IsNullOrWhiteSpace(model.Descripcion))
            {
                mensaje = "La descripcion del producto no puede ser vacio";
            }
            else if (model.MarcaId.Id == 0)
            {
                mensaje = "Debe seleccionar una Marca";
            }
            else if (model.CategoriaId.Id == 0)
            {
                mensaje = "Debe seleccionar una Categoria";
            }
            else if (model.Precio == 0)
            {
                mensaje = "Debe ingresar el precio del producto";
            }
            else if (model.Stock == 0)
            {
                mensaje = "Debe ingresar el stock del producto";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                return productoDb.Editar(model, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool GuardarDatosImagen(Producto model, out string mensaje)
        {
            return productoDb.GuardarDatosImagen(model, out mensaje);
        }
        public bool Eliminar(int id, out string mensaje)
        {
            return productoDb.Eliminar(id, out mensaje);
        }
    }
}
