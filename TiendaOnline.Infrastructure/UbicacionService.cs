using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Infrastructure
{
    public class UbicacionService
    {
        private UbicacionDb ubicacion = new UbicacionDb();

        public List<Provincia> ObtenerProvincia()
        {
            return ubicacion.ObtenerProvincia();
        }
        public List<Municipio> ObtenerMunicipio(string provinciaId)
        {
            return ubicacion.ObtenerMunicipio(provinciaId);
        }
        public List<Distrito> ObtenerDistrito(string provinciaId, string municipioId)
        {
            return ubicacion.ObtenerDistrito(provinciaId, municipioId);
        }
    }
}