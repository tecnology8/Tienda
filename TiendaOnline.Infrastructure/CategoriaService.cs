using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Infrastructure
{
    public class CategoriaService
    {
        private CategoriaDb categoriaDb = new CategoriaDb();
        public List<Categoria> Listar()
        {
            return categoriaDb.Listar();
        }
        public int Registrar(Categoria model, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(model.Descripcion) || string.IsNullOrWhiteSpace(model.Descripcion))
            {
                mensaje = "La descripcion de la categoria no puede ser vacio";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                return categoriaDb.Registrar(model, out mensaje);
            }
            else
            {
                return 0;
            }
        }
        public bool Editar(Categoria model, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(model.Descripcion) || string.IsNullOrWhiteSpace(model.Descripcion))
            {
                mensaje = "La descripcion de la categoria no puede ser vacio";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                return categoriaDb.Editar(model, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int id, out string mensaje)
        {
            return categoriaDb.Eliminar(id, out mensaje);
        }
    }
}
