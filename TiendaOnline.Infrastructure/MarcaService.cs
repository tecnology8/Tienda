using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Infrastructure
{
    public class MarcaService
    {
        private MarcaDb marcaDb = new MarcaDb();
        public List<Marca> Listar()
        {
            return marcaDb.Listar();
        }
        public int Registrar(Marca model, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(model.Descripcion) || string.IsNullOrWhiteSpace(model.Descripcion))
            {
                mensaje = "La descripcion de la marca no puede ser vacio";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                return marcaDb.Registrar(model, out mensaje);
            }
            else
            {
                return 0;
            }
        }
        public bool Editar(Marca model, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(model.Descripcion) || string.IsNullOrWhiteSpace(model.Descripcion))
            {
                mensaje = "La descripcion de la marca no puede ser vacio";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                return marcaDb.Editar(model, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int id, out string mensaje)
        {
            return marcaDb.Eliminar(id, out mensaje);
        }
        public List<Marca> ListarMarcaPorCategoria(int idcategoria)
        {
            return marcaDb.ListarMarcaPorCategoria(idcategoria);
        }
    }
}