using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaOnline.Data
{
    public  class Conexion
    {
        public static string connection = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
    }
}
