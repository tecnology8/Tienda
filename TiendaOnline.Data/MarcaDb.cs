using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Data
{
    public class MarcaDb
    {
        public List<Marca> Listar()
        {
            var lista = new List<Marca>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    string query = "SELECT Id, Descripcion, Activo FROM Marca";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Marca
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Marca>();
            }
            return lista;
        }
        public int Registrar(Marca model, out string mensaje)
        {
            int idautogenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarMarca", conn);
                    cmd.Parameters.AddWithValue("Descripcion", model.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", model.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                mensaje = ex.Message;
            }
            return idautogenerado;
        }
        public bool Editar(Marca model, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarMarca", conn);
                    cmd.Parameters.AddWithValue("Id", model.Id);
                    cmd.Parameters.AddWithValue("Descripcion", model.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", model.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarMarca", conn);
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public List<Marca> ListarMarcaPorCategoria(int idcategoria)
        {
            var lista = new List<Marca>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    StringBuilder sb = new StringBuilder();


                    sb.AppendLine("select distinct m.Id, m.Descripcion from Producto p");
                    sb.AppendLine("INNER JOIN Categoria c on c.Id = p.CategoriaId");
                    sb.AppendLine("INNER JOIN Marca m on m.Id = p.MarcaId");
                    sb.AppendLine("where c.Id = IIF(@CategoriaId = 0, c.Id, @CategoriaId)");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                    cmd.Parameters.AddWithValue("@CategoriaId", idcategoria);
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Marca
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Marca>();
            }
            return lista;
        }
    }
}