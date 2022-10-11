using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaOnline.Domain.Models
{
    public class Municipio
    {
        public int Id { get; set; }
        public string ProvinciaId { get; set; }
        public string Descripcion { get; set; }
    }
}
