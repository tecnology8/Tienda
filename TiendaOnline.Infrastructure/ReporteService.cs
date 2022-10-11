using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Infrastructure
{
    public class ReporteService
    {
        private ReporteDb reporteDb = new ReporteDb();
        public Dashboard VerDashboard()
        {
            return reporteDb.VerDashboard();
        }
        public List<Reporte> Ventas(string fechainicio, string fechafin, string transaccionid)
        {
            return reporteDb.Ventas(fechainicio, fechafin, transaccionid);
        }
    }
}