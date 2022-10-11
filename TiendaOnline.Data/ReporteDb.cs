using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Data
{
    public class ReporteDb
    {
        public Dashboard VerDashboard()
        {
            var dashboard = new Dashboard();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_ReporteDashboard", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard = new Dashboard
                            {
                                TotalCliente = Convert.ToInt32(reader["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(reader["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(reader["TotalProducto"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dashboard = new Dashboard();
            }
            return dashboard;
        }

        public List<Reporte> Ventas(string fechainicio, string fechafin, string transaccionid)
        {
            var lista = new List<Reporte>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_ReporteVentas", conn);
                    cmd.Parameters.AddWithValue("FechaInicio", fechainicio);
                    cmd.Parameters.AddWithValue("FechaFin", fechafin);
                    cmd.Parameters.AddWithValue("TransaccionId", transaccionid);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Reporte
                            {
                                FechaVenta = reader["FechaVenta"].ToString(),
                                Cliente  = reader["Cliente"].ToString(),
                                Producto = reader["Producto"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Total = Convert.ToDecimal(reader["Total"]),
                                TransaccionId = reader["TransaccionId"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Reporte>();
            }
            return lista;
        }
    }
}