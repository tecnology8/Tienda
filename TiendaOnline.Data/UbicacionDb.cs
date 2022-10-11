using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Data
{
    public class UbicacionDb
    {
        public List<Provincia> ObtenerProvincia()
        {
            var lista = new List<Provincia>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    string query = "SELECT * FROM Provincia";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Provincia
                            {
                                Id = Convert.ToInt32(reader["Id"]).ToString(),
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Provincia>();
            }
            return lista;
        }
        public List<Municipio> ObtenerMunicipio(string provinciaId)
        {
            var lista = new List<Municipio>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    string query = "SELECT * FROM Municipio where ProvinciaId = @provinciaId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@provinciaId", provinciaId);
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Municipio
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProvinciaId = Convert.ToInt32(reader["ProvinciaId"]).ToString(),
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Municipio>();
            }
            return lista;
        }
        public List<Distrito> ObtenerDistrito(string provinciaId, string municipioId)
        {
            var lista = new List<Distrito>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    string query = "SELECT * FROM Distrito where ProvinciaId = @provinciaId and MunicipioId = @municipioId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@provinciaId", provinciaId);
                    cmd.Parameters.AddWithValue("@municipioId", municipioId);
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Distrito
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProvinciaId = Convert.ToInt32(reader["ProvinciaId"]).ToString(),
                                MunicipioId = Convert.ToInt32(reader["MunicipioId"]).ToString(),
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Distrito>();
            }
            return lista;
        }
    }
}
