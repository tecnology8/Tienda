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
    public class CarritoDb
    {
        public bool ExisteCarrito(int clienteid, int productoid)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_ExisteCarrito", conn);
                    cmd.Parameters.AddWithValue("ClienteId", clienteid);
                    cmd.Parameters.AddWithValue("ProductoId", productoid);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);   
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                
            }
            return resultado;
        }
        public bool OperacionCarrito(int clienteid, int productoid, bool sumar,  out string mensaje)
        {
            bool resultado = true;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_OperacionCarrito", conn);
                    cmd.Parameters.AddWithValue("ClienteId", clienteid);
                    cmd.Parameters.AddWithValue("ProductoId", productoid);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
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
        public int CantidadEnCarrito(int id)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM Carrito where Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }
        public List<Carrito> ListarProducto(int Idcliente)
        {
            var lista = new List<Carrito>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    string query = "select * from fn_obtenerCarritoCliente(@IdCliente)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IdCliente", Idcliente);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Carrito
                            {
                                ProductoId = new Producto()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-DO")),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString(),
                                    MarcaId = new Marca { Descripcion = reader["Marca"].ToString() }
                                },
                                Cantidad = Convert.ToInt32(reader["Cantidad"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Carrito>();
            }
            return lista;
        }
        public bool EliminarCarrito(int clienteid, int productoid)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarCarrito", conn);
                    cmd.Parameters.AddWithValue("ClienteId", clienteid);
                    cmd.Parameters.AddWithValue("ProductoId", productoid);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }
    }
}