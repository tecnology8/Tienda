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
    public class ClienteDb
    {
        public List<Cliente> Listar()
        {
            var lista = new List<Cliente>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    string query = "SELECT Id, Nombres, Apellidos, Correo, Clave, Reestablecer FROM Cliente";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Cliente
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombres = reader["Nombres"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(reader["Reestablecer"])
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Cliente>();
            }
            return lista;
        }
        public int Registrar(Cliente model, out string mensaje)
        {
            int idautogenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCliente", conn);
                    cmd.Parameters.AddWithValue("Nombres", model.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", model.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", model.Correo);
                    cmd.Parameters.AddWithValue("Clave", model.Clave);
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
        public bool Editar(Cliente model, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarCliente", conn);
                    cmd.Parameters.AddWithValue("Id", model.Id);
                    cmd.Parameters.AddWithValue("Nombres", model.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", model.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", model.Correo);
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
                    SqlCommand cmd = new SqlCommand("delete top (1) from cliente where Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
        public bool CambiarClave(int id, string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @nuevaclave, reestablecer = 0 where Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@NuevaClave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
        public bool ReestablerClave(int id, string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @clave, reestablecer = 1 where Id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
    }
}