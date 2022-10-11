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
    public class ProductoDb
    {
        public List<Producto> Listar()
        {
            var lista = new List<Producto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT p.Id, p.Nombre, p.Descripcion,");
                    sb.AppendLine("m.Id,m.Descripcion DescripcionMarca,");
                    sb.AppendLine("c.Id,c.Descripcion DescripcionCategoria,");
                    sb.AppendLine("p.Precio,p.Stock, p.RutaImagen,p.NombreImagen,p.Activo");
                    sb.AppendLine("FROM Producto p");
                    sb.AppendLine("INNER JOIN Marca m ON m.Id = p.MarcaId");
                    sb.AppendLine("INNER JOIN Categoria c on c.Id = p.CategoriaId");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Producto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                MarcaId = new Marca
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Descripcion = reader["DescripcionMarca"].ToString()
                                },
                                CategoriaId = new Categoria
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Descripcion = reader["DescripcionCategoria"].ToString()
                                },
                                Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-DO")),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                RutaImagen = reader["RutaImagen"].ToString(),
                                NombreImagen = reader["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])
                            });
                        }
                    }
                }
            }
            catch (Exception )
            {
                lista = new List<Producto>();
            }
            return lista;
        }
        public int Registrar(Producto model, out string mensaje)
        {
            int idautogenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarProducto", conn);
                    cmd.Parameters.AddWithValue("Nombre", model.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", model.Descripcion);
                    cmd.Parameters.AddWithValue("MarcaId", model.MarcaId.Id);
                    cmd.Parameters.AddWithValue("CategoriaId", model.CategoriaId.Id);
                    cmd.Parameters.AddWithValue("Precio", model.Precio);
                    cmd.Parameters.AddWithValue("Stock", model.Stock);
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
        public bool Editar(Producto model, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarProducto", conn);
                    cmd.Parameters.AddWithValue("Id", model.Id);
                    cmd.Parameters.AddWithValue("Nombre", model.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", model.Descripcion);
                    cmd.Parameters.AddWithValue("MarcaId", model.MarcaId.Id);
                    cmd.Parameters.AddWithValue("CategoriaId", model.CategoriaId.Id);
                    cmd.Parameters.AddWithValue("Precio", model.Precio);
                    cmd.Parameters.AddWithValue("Stock", model.Stock);
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

        public bool GuardarDatosImagen(Producto model, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.connection))
                {
                    string query = "update producto set RutaImagen = @rutaimagen, NombreImagen = @nombreimagen where Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@rutaimagen", model.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", model.NombreImagen);
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                        mensaje = "No se pudo actualizar imagen";
                    }
                }
            }
            catch (Exception ex)
            {
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
                    SqlCommand cmd = new SqlCommand("SP_EliminarProducto", conn);
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
    }
}